import { Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MultiLevelOptionItem } from "src/app/modules/shared-module/models";
import { FormStatuses } from "src/app/modules/shared-module/constants";
import { switchMap, takeUntil } from "rxjs/operators";
import { of } from "rxjs";
import { MbscSelectOptions, MbscSelect } from "@mobiscroll/angular";
import { TranslateService } from "@ngx-translate/core";
import { CommunicationService, DropdownValueChangedEvent, Channels } from "src/app/services/communication";


@Component({
    selector: 'case-multi-dropdown-control',
    templateUrl: './case-multi-dropdown-control.component.html',
    styleUrls: ['./case-multi-dropdown-control.component.scss']
  })
  export class CaseMultiDropdownComponent extends BaseControl<number> {
    @ViewChild('textbox') textbox: any;
    @ViewChild('select') select: MbscSelect;
    @Input() dataSource: MultiLevelOptionItem[] = [];
    @Input() disabled = false;
    text: string = '';
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
              inst.refresh(this.getPreviousData(this.parentValue));
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
        if (this.field.isReadonly) {
          inst.disable();
        }
      },
      onBeforeShow: (event, inst) => {
        let data = this.getPreviousData(inst.getVal());
        inst.refresh(data);
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
        if (event.value) { 
          let chain = this.getOptionsChain(event.value);
          if (this.hasChilds(chain, chain.length - 1)) {
            let data = this.getNextData(event.value);
            inst.refresh(data, '');
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
        this.commService.publish(Channels.DropdownValueChanged, new DropdownValueChangedEvent(value, event.valueText, this.field.name));
      }, 
      onClose: () => {
        this.parentValue = undefined;
      }
    }
    private parentValue?: number;

    constructor(private ngxTranslateService: TranslateService, private commService: CommunicationService) {
      super();
    }

    ngOnInit(): void {
      this.init(this.field);
      this.updateDisabledState();

      this.initEvents()
      this.setText(this.field.value);
    }

    ngOnDestroy(): void {
      this.onDestroy();
    }

    public get getHeader(): string {
      const defaultValue = '';
      if (!this.field) {
        return defaultValue;
      }
      return this.formControl.label || defaultValue;
    }

    public openSelect() {
      this.select.instance.show()
    }

    private setText(value?: number) {
      this.text = this.getTextChain(value);
    }

    private getNextData(value?: number) {
      if (this.dataSource == null || this.dataSource.length === 0) return [];

      let tempDataSource = this.dataSource;
      if (value != null) {
        let chain = this.getOptionsChain(value);
        if (this.hasChilds(chain, chain.length - 1)) {
          tempDataSource = chain[chain.length - 1].childs;
          this.parentValue = value;
        }
      }

      return this.ToSelectDataSource(tempDataSource);
    }

    private getPreviousData(value?: number) {
      if (this.dataSource == null || this.dataSource.length === 0) return [];

      let tempDataSource = this.dataSource;
      let chain = this.getOptionsChain(value);
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
      if (this.dataSource == null || this.dataSource.length === 0) return '';

      let chain = this.getOptionsChain(id);

      return chain.length > 0 ? chain.map((elem) => elem.text).join(' > ') : '';
    }

    private getOptionsChain(id: number | string) {
      if (this.dataSource == null || this.dataSource.length === 0) return [];

      let elems = new Array<MultiLevelOptionItem>();
      const searchNode = (elem: MultiLevelOptionItem, targetId: any): string => {
        let isText = typeof targetId === 'string';
        if ((isText && (elem.text == targetId)) || (elem.value == targetId)) {
          elems.push(elem);
           return elem.text; 
          }
        if (elem.childs != null) {
          for(let i = 0; i < elem.childs.length; i++) {
            let text = searchNode(elem.childs[i], targetId);
            if (text != null) { 
              elems.push(elem);
              return text;
            }
          }
        }
        return null;
      }
      this.dataSource.forEach((elem: MultiLevelOptionItem) => searchNode(elem, id));
      elems = elems.reverse();

      return  elems || [];
    }

    private updateDisabledState() {
      this.textbox.disabled = this.formControl.disabled || this.disabled;
    }

    private get isFormControlDisabled() {
      return this.formControl.status == FormStatuses.DISABLED;
    }
    
    private initEvents() {
      this.formControl.statusChanges // track disabled state in form
        .pipe(switchMap((e: any) => {
            if (this.textbox.disabled != this.isFormControlDisabled) {
              this.updateDisabledState();
            }
            return of(e);
          }),
          takeUntil(this.destroy$)
        )
        .subscribe();
    }

    private markIfRoot(inst) {
      if (inst.markup == null) return;
      
      if (!this.hasParent()) {
        inst.markup.classList.add('root');
      } else {
        inst.markup.classList.remove('root');
      };
    }

    private markIfHasChilds(text: string, inst) {
      if (inst.markup == null) return;

      let chain = this.getOptionsChain(text);
      if (this.hasChilds(chain, chain.length - 1)) {
        inst.markup.classList.add('hasChild');
      } else {
        inst.markup.classList.remove('hasChild');
      };
    }
  }