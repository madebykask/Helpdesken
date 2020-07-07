
import { of, Observable, BehaviorSubject, Subject, Subscription } from 'rxjs';
import { tap, mergeMap, filter, takeUntil, take, skip} from 'rxjs/operators';
import { Component, Input, OnInit, OnChanges, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { SingleControlFieldModel } from '../../../models/form.model';
import { ComponentCommService, ControlDataSourceChangeParams } from '../../../services/component-comm.service';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { FormControlsManagerService } from '../../../services/form-controls-manager.service';
import { LogService } from '../../../services/log.service';
import { ItemModel } from '../../../models/form.model';
import { BaseControl } from '../base-control';

@Component({
    selector: 'ec-textbox-search',
    templateUrl: './ec-textbox-search.component.html'
})
export class ExtendedCaseTextBoxSearchComponent extends BaseControl implements OnInit, OnChanges, OnDestroy {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    dataSource: Observable<Array<ItemModel>>;
    typeaheadLoading = false;
    fakeControlName = '';
    private lastSelectedValue = '';
    private destroy$ = new Subject<any>();
    private fakeControl: FormControl;
    private dataSourceSubscription: Subscription;

    private subject$: BehaviorSubject<Array<ItemModel>>;

    constructor(componentCommService: ComponentCommService,
        private formControlsManager: FormControlsManagerService,
        private logService: LogService, changeDetector: ChangeDetectorRef) {
        super(componentCommService, changeDetector);
    }

    ngOnInit(): void {
      this.subject$ = new BehaviorSubject<Array<ItemModel>>(this.fieldModel.items);
      this.componentCommService.controlDataSourceChange$.pipe(
        tap((params: ControlDataSourceChangeParams) => {
            // this.logService.debug(`Typeahead ${this.fieldModel.id} - dataSourceUpdated$ start: ${this.fieldModel.control.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
        }),
        filter((params: ControlDataSourceChangeParams) => { // skip if data is not for this control
            // this.logService.debug(`Typeahead - dataSourceUpdated$ filter: ${params.controlId}, ${this.fieldModel.id}`);
            return params.controlId === this.fieldModel.id;
        }),
        takeUntil(this.destroy$),
      ).subscribe((params: ControlDataSourceChangeParams) => {
          // this.logService.debug(`Typeahead ${this.fieldModel.id} - dataSourceUpdated$ subscribe: ${this.fieldModel.control.value},
          // ${params.controlId}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
          if (!params.isChanged || !this.fieldTemplate.processControlDataSourcesOnly) { return; }
          this.subject$.next(params.items);
      });

      if (this.fieldModel.showSearchResults) {
        this.dataSource = this.subject$.pipe(
            mergeMap((items: Array<ItemModel>) => {
                // this.logService.debug(`Typeahead ${this.fieldModel.id} - flatMap: ${this.fieldModel.control.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
                // this.items = items;
                return of(items);
            }));
      } else {
        this.initFakeControl();
      };
    }

    ngOnChanges(changes: Object): void {
      this.lastSelectedValue = this.fieldModel.control.value as string;
      this.fieldTemplate.processControlDataSourcesOnly = true; // skip main digest cycle

        // this.fieldModel.control.valueChanges.subscribe((value: string) => {
        //    const foundItems = this.fieldModel.items.filter((item: ItemModel) => {
        //        return item.text === value;
        //    });
        //    this.logService.debug(`Typeahead ${this.fieldModel.id} - additionalData: ${value}`);
        //    this.fieldModel.additionalData = foundItems.length > 0 ? foundItems[0].value : '';
        // });
    }

    ngOnDestroy(): void {
      super.ngOnDestroy();

      this.destroy$.next();
      this.destroy$.complete();
    }

    onSelect(e: TypeaheadMatch): void {
        // this.logService.debug(`Typeahead ${this.fieldModel.id} - selected value: ${e.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
        this.changeValue(e.value);
    }

    setValue(value: string) {
        this.changeValue(value);
    }

    onBlur(e: any): void {
      if (this.fieldModel.showSearchResults) {
        this.logService.debugFormatted('Typeahead {0} - blur, {1}', this.fieldModel.id, this.fieldTemplate.processControlDataSourcesOnly);
        if (this.fieldModel.control.value !== this.lastSelectedValue) {
            this.fieldModel.control.setValue(this.lastSelectedValue, { emitEvent: false });
            // this.fieldTemplate.processControlDataSourcesOnly = true;//skip main digest cycle
            this.logService.debugFormatted('Typeahead {0} - changeValue to lastSelectedValue: {1}', this.fieldModel.id, this.lastSelectedValue);
        }
      } else {
        // Force update of datasource by the value and setting additional data
        if (e.target.value !== this.fieldModel.control.value) {
          if (this.dataSourceSubscription != null && !this.dataSourceSubscription.closed) {
            this.dataSourceSubscription.unsubscribe();
          }
          this.dataSourceSubscription = this.subject$.pipe(
            skip(1), // skip default value from behaviorSubject
            filter((items: ItemModel[]) => {
              return items && items.length === 1;
            }),
            take(1),
            takeUntil(this.destroy$)
          ).subscribe((items: ItemModel[]) => {
            let additionalData = items && items.length === 1 ? items[0].value : ''
            this.logService.debugFormatted(`Search {0} - additionalData: {1}`, this.fieldModel.id, additionalData);
            this.fieldTemplate.processControlDataSourcesOnly = false;
            this.fieldModel.setAdditionalData(additionalData);
            this.fieldModel.control.setValue(e.target.value, { onlySelf: false, emitEvent: true }); // start digest again
            // this.logService.debugFormatted(`Search: {0} got 1 item only in result: {1}`, this.fieldModel.id, JSON.stringify(items));
          });
          this.fieldTemplate.processControlDataSourcesOnly = true;
          this.fieldModel.control.setValue(e.target.value, { emitEvent: true }); // start digest again
        }
      }
    }

    onStartTyping(e: any): void {
        // this.logService.debug(`Typeahead ${this.fieldModel.id} - onStartTyping, ${this.fieldTemplate.processControlDataSourcesOnly}`);
        this.fieldTemplate.processControlDataSourcesOnly = true;
    }

    changeTypeaheadLoading(e: boolean): void {
        this.typeaheadLoading = e;
    }

    private initFakeControl() {
      // fake control is required to use updateOn: 'blur' strategy, because no other way to change it in existing control
      this.fakeControl = new FormControl(this.fieldModel.control.value, { updateOn: 'blur' });
        if (this.fieldModel.control.disabled) { this.fakeControl.disable({emitEvent: false}) };
        this.fakeControlName = 'fakeControl_' + this.fieldModel.id;
        this.form.addControl(this.fakeControlName, this.fakeControl);
        this.form.get(this.fieldModel.id).statusChanges.pipe(
          takeUntil(this.destroy$)
        ).subscribe((status: any) => {
            if (status === 'DISABLED') {
              this.fakeControl.disable({emitEvent: true})
            } else {
              this.fakeControl.enable({emitEvent: true})
            }
        });
    }

    private changeValue(value: string) {
      this.lastSelectedValue = value;
      this.fieldTemplate.processControlDataSourcesOnly = false;
      const foundItems = this.fieldModel.items.filter((item: ItemModel) => item.text === value);

      this.logService.debugFormatted(`Typeahead {0} - additionalData: {1}`, this.fieldModel.id, value);
      let additionalData = foundItems.length > 0 ? foundItems[0].value : '';
      this.fieldModel.setAdditionalData(additionalData);
  }
}
