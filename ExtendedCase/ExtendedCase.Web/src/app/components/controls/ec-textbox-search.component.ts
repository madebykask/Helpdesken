import { Component, Input, OnInit, OnChanges, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseControlTemplateModel } from '../../models/template.model';
import { SingleControlFieldModel } from '../../models/form.model';
import { ComponentCommService, ControlDataSourceChangeParams } from '../../services/component-comm.service';
import { TypeaheadMatch } from 'ngx-bootstrap'
import { FormControlsManagerService } from '../../services/form-controls-manager.service';
import { LogService } from '../../services/log.service';
import { Observable } from 'rxjs/Rx';
import { ItemModel } from '../../models/form.model';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { BaseControl } from './base-control';

@Component({
    selector: 'ec-textbox-search',
    templateUrl: './ec-textbox-search.component.html'
})

export class ExtendedCaseTextBoxSearchComponent extends BaseControl implements OnInit, OnChanges, OnDestroy {
    @Input() fieldModel: SingleControlFieldModel;
    @Input() form: FormGroup;

    private dataSource: Observable<Array<ItemModel>>;
    private lastSelectedValue = '';
    private typeaheadLoading = false;
    private items: Array<ItemModel> = [];

    private subject$: BehaviorSubject<Array<ItemModel>>;

    constructor(componentCommService: ComponentCommService,
        private formControlsManager: FormControlsManagerService,
        private logService: LogService, private changeDetector: ChangeDetectorRef) {

        super(componentCommService, changeDetector);
        this.componentCommService.controlDataSourceChange$
            .do((params: ControlDataSourceChangeParams) => {
                //this.logService.debug(`Typeahead ${this.fieldModel.id} - dataSourceUpdated$ start: ${this.fieldModel.control.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
            })
            .filter((params: ControlDataSourceChangeParams) => { // skip if data is not for this control
                //this.logService.debug(`Typeahead - dataSourceUpdated$ skip: ${this.fieldModel.control.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
                return params.controlId === this.fieldModel.id;
            })
            .subscribe((params: ControlDataSourceChangeParams) => {
                //this.logService.debug(`Typeahead ${this.fieldModel.id} - dataSourceUpdated$ subscribe: ${this.fieldModel.control.value}, ${params.controlId}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
                if (!params.isChanged || !this.fieldTemplate.processControlDataSourcesOnly) return;

                this.subject$.next(params.items);
            });
    }

    ngOnInit(): void {

    }

    ngOnChanges(changes: Object): void {
        this.items = this.fieldModel.items;
        this.subject$ = new BehaviorSubject<Array<ItemModel>>(this.items);
        this.dataSource = this.subject$
            .flatMap((items: Array<ItemModel>) => {
                //this.logService.debug(`Typeahead ${this.fieldModel.id} - flatMap: ${this.fieldModel.control.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
                this.items = items;
                return Observable.of(this.items);
            });

        this.lastSelectedValue = this.fieldModel.control.value as string;
        this.fieldTemplate.processControlDataSourcesOnly = true;//skip main digest cycle

        //this.fieldModel.control.valueChanges.subscribe((value: string) => {
        //    const foundItems = this.fieldModel.items.filter((item: ItemModel) => {
        //        return item.text === value;
        //    });
        //    this.logService.debug(`Typeahead ${this.fieldModel.id} - additionalData: ${value}`);
        //    this.fieldModel.additionalData = foundItems.length > 0 ? foundItems[0].value : '';
        //});
    }
    
    ngOnDestroy(): void {

    }

    onSelect(e: TypeaheadMatch): void {
        //this.logService.debug(`Typeahead ${this.fieldModel.id} - selected value: ${e.value}, ${this.fieldTemplate.processControlDataSourcesOnly}`);
        this.changeValue(e.value);
    }

    setValue(value:string) {
        this.changeValue(value);
    }

    private changeValue(value:string) {
        this.lastSelectedValue = value;
        this.fieldTemplate.processControlDataSourcesOnly = false;
        const foundItems = this.fieldModel.items.filter((item: ItemModel) => item.text === value);

        this.logService.debugFormatted(`Typeahead {0} - additionalData: {1}`, this.fieldModel.id, value);
        let additionalData = foundItems.length > 0 ? foundItems[0].value : '';
        this.fieldModel.setAdditionalData(additionalData);
    }

    onBlur(e: any): void {
        this.logService.debugFormatted('Typeahead {0} - blur, {1}', this.fieldModel.id, this.fieldTemplate.processControlDataSourcesOnly);
        if (this.fieldModel.control.value !== this.lastSelectedValue) {
            this.fieldModel.control.setValue(this.lastSelectedValue, { emitEvent: false });
            //this.fieldTemplate.processControlDataSourcesOnly = true;//skip main digest cycle
            this.logService.debugFormatted('Typeahead {0} - changeValue to lastSelectedValue: {1}', this.fieldModel.id, this.lastSelectedValue);
        }
    }

    onStartTyping(e: any): void {
        //this.logService.debug(`Typeahead ${this.fieldModel.id} - onStartTyping, ${this.fieldTemplate.processControlDataSourcesOnly}`);
        this.fieldTemplate.processControlDataSourcesOnly = true;
    }

    changeTypeaheadLoading(e: boolean): void {
        this.typeaheadLoading = e;
    }
}
