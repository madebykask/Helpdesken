import { Component, Input, ViewChild, Renderer2, ElementRef } from '@angular/core';
import { BaseControl } from '../base-control';
import { MultiLevelOptionItem } from 'src/app/modules/shared-module/models';
import { takeUntil, map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, DropdownValueChangedEvent, Channels } from 'src/app/services/communication';

@Component({
    selector: 'case-multi-dropdown-control',
    templateUrl: './case-multi-dropdown-control.component.html',
    styleUrls: ['./case-multi-dropdown-control.component.scss']
  })
  export class CaseMultiDropdownComponent extends BaseControl<number> {
    @ViewChild('textbox') textbox: any;
    @ViewChild('select') select: MbscSelect;
    @Input() dataSource: BehaviorSubject<MultiLevelOptionItem[]>;
    @Input() disabled = false;

    private parentValue?: number;

    text = '';
    settings: MbscSelectOptions = {
      theme: 'mobiscroll',
      display: 'center',
      layout: 'liquid',
      anchor: 'select-textarea',
      focusOnClose: false,
      filter: true,
      filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
      filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),
      setText: this.ngxTranslateService.instant('Välj'),
      cancelText: this.ngxTranslateService.instant('Avbryt'),
      buttons:
      [{ // this button is hidden if root element has 'root' class
          text: this.ngxTranslateService.instant('Tillbaka'),
          cssClass: 'mbsc-fr-btn',
          parentClass: 'float-left back-btn',
          handler: (event, inst) => {
            if (this.hasParent()) {
              this.refreshData(inst, this.getPreviousData(this.parentValue));
              this.markIfRoot(inst);
            }
          }
        },
        'cancel',
        ],
      data: [],
      headerText: () => this.getHeader,
      input: 'select-textarea',
      onInit: (event, inst) => {
        if (this.formControl && this.formControl.fieldInfo.isReadonly) {
          inst.disable();
        }
      },
      onBeforeShow: (event, inst) => {
        const data = this.getPreviousData(inst.getVal());
        this.refreshData(inst, data);
      },
      onMarkupReady: (event, inst) => {
        this.markIfRoot(inst);
        this.markIfHasChilds(inst.getVal(), inst);
      },
      onChange: (event, inst) => {
        if (event.valueText) {
          this.markIfHasChilds(event.valueText, inst);
        }
      },
      onItemTap: (event, inst) => {
        if (event.value != null) {
          const chain = this.getOptionsChain(event.value);
          if (this.hasChilds(chain, chain.length - 1)) {
            const data = this.getNextData(event.value);
            this.refreshData(inst, data);
            this.markIfRoot(inst);
          } else {
            inst.setVal(event.value);
            inst.select();
          }
        }
        return false;
      },
      onSet: (event, inst) => { // somehow onset is invoked on scrolling options
        const value = inst.getVal();
        this.setText(value);
        this.commService.publish(Channels.DropdownValueChanged, new DropdownValueChangedEvent(value, event.valueText, this.fieldName));
      },
      onClose: () => {
        this.parentValue = undefined;
      }
    };

    constructor(private ngxTranslateService: TranslateService,
       private commService: CommunicationService,
       private renderer: Renderer2,
       private elem: ElementRef) {
      super();
    }

    ngOnInit(): void {
      this.init(this.fieldName);
      this.updateDisabledState();

      this.initEvents();
      this.setText(this.formControl.value);
    }

    ngAfterViewInit(): void {
      this.addSelectArrow();
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    public get getHeader(): string {
      const defaultValue = '';
      return this.formControl ? this.formControl.label || defaultValue : defaultValue;
    }

    private refreshData(inst, data) {
      inst.refresh(data, '');
      if (inst.markup != null) {
        const elem =  (<HTMLElement>inst.markup).querySelector<HTMLInputElement>('input.mbsc-sel-filter-input');
        elem.value = '';
      }
    }

    public openSelect() {
      this.select.instance.show();
    }

    private setText(value?: number) {
      this.text = this.getTextChain(value);
    }

    private getNextData(value?: number) {
      if (this.dataSource == null || this.dataSource.value.length === 0) return [];

      let tempDataSource = this.dataSource.value;
      if (value != null) {
        const chain = this.getOptionsChain(value);
        if (this.hasChilds(chain, chain.length - 1)) {
          tempDataSource = chain[chain.length - 1].childs;
          this.parentValue = value;
        }
      }

      return this.ToSelectDataSource(tempDataSource);
    }

    private getPreviousData(value?: number) {
      if (this.dataSource == null || this.dataSource.value.length === 0) return [];

      let tempDataSource = this.dataSource.value;
      const chain = this.getOptionsChain(value);
      if (this.hasChilds(chain, chain.length - 2)) {
        tempDataSource = chain[chain.length - 2].childs;
        this.parentValue = chain[chain.length - 2].value;
      } else {
        this.parentValue = undefined;
      }
      return this.ToSelectDataSource(tempDataSource);
    }

    private ToSelectDataSource(dataSource) {
      return dataSource.map((elem: MultiLevelOptionItem) => {
        return {
          value: elem.value,
          text: elem.text,
          html: elem.text + (elem.childs != null && elem.childs.length ?
             '<span class="mbsc-ic mbsc-ic-fa-angle-right calendar-option"></span>' :
              '')
        };
      });
    }

    private hasParent() {
      return !!this.parentValue;
    }

    private hasChilds(chain: MultiLevelOptionItem[], position: number) {
      return chain.length >= 1 && position >= 0 && chain[position].childs != null && chain[position].childs.length;
    }

    private getTextChain(id: any) {
      if (this.dataSource == null || this.dataSource.value.length === 0) return '';

      const chain = this.getOptionsChain(id);
      return chain.length > 0 ? chain.map((elem) => elem.text).join(' > ') : '';
    }

    private getOptionsChain(id: number | string) {
      if (this.dataSource == null || this.dataSource.value.length === 0) return [];

      let elems = new Array<MultiLevelOptionItem>();
      const searchNode = (elem: MultiLevelOptionItem, targetId: any): string => {
        const isText = typeof targetId === 'string';
        if ((isText && (elem.text === targetId)) || (elem.value === targetId)) {
          elems.push(elem);
           return elem.text;
          }
        if (elem.childs != null) {
          for (let i = 0; i < elem.childs.length; i++) {
            const text = searchNode(elem.childs[i], targetId);
            if (text != null) {
              elems.push(elem);
              return text;
            }
          }
        }
        return null;
      };
      this.dataSource.value.forEach((elem: MultiLevelOptionItem) => searchNode(elem, id));
      elems = elems.reverse();

      return  elems || [];
    }

    private updateDisabledState() {
      this.textbox.disabled = this.isFormControlDisabled || this.disabled;
    }

    private initEvents() {
      // track disabled state in form
      this.formControl.statusChanges.pipe(
          takeUntil(this.destroy$)
        ).subscribe(e => {
          if (this.textbox.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
      });

      this.dataSource.pipe(
          map(((options) => {
            options = options || [];
            this.addEmptyIfNotExists(options);
            return options;
          })),
          takeUntil(this.destroy$)
        ).subscribe((options) => {
          if (this.select.instance) {
            const data = this.getPreviousData(this.formControl.value);
            this.refreshData(this.select.instance, data);
          }
          this.resetValueIfNeeded(options);
        });
    }

    private addEmptyIfNotExists(options) {
      if (!options.some((i) => i.value === '')) {
        options.unshift(new MultiLevelOptionItem('', '--'));
      }
    }

    private resetValueIfNeeded(options: MultiLevelOptionItem[]) {
      if (this.getOptionsChain(this.formControl.value).length === 0) {
        this.formControl.setValue('');
      }
    }

    private addSelectArrow() {
      const inputWrap = (<HTMLElement>this.elem.nativeElement).querySelector('mbsc-textarea .mbsc-input-wrap');
      const span = (<HTMLElement>this.elem.nativeElement).querySelector('mbsc-textarea .mbsc-input-fill');
      const arrowHtml = this.renderer.createElement('span');
      this.renderer.addClass(arrowHtml, 'mbsc-select-ic');
      this.renderer.addClass(arrowHtml, 'mbsc-ic');
      this.renderer.addClass(arrowHtml, 'mbsc-ic-arrow-down5');
      this.renderer.insertBefore(inputWrap, arrowHtml, span);
    }

    private markIfRoot(inst) {
      if (inst.markup == null) return;

      if (!this.hasParent()) {
        inst.markup.classList.add('root');
      } else {
        inst.markup.classList.remove('root');
      }
    }

    private markIfHasChilds(text: string, inst) {
      if (inst.markup == null) return;

      const chain = this.getOptionsChain(text);
      if (this.hasChilds(chain, chain.length - 1)) {
        inst.markup.classList.add('hasChild');
      } else {
        inst.markup.classList.remove('hasChild');
      }
    }
  }
