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
  export class CaseMultiDropdownComponent extends BaseControl {
    @ViewChild('textbox') textbox: any;
    @ViewChild('select') select: MbscSelect;
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: MultiLevelOptionItem[] = [];
    @Input() disabled = false;
    text: string = "";
    settings: MbscSelectOptions = {
      display: 'bottom',
      focusOnClose: false,
      filter: false,
      group: {
        groupWheel: true,
        header: false,
        clustered: true
      },
      maxWidth: [40, 500],
      data: [],
      headerText: () => this.getHeader,
      input: 'select-input',
      onInit: (event, inst) => {
        if (this.field.isReadonly) {
          inst.disable();
        }
      },
      onBeforeShow: (event, inst) => {
        inst.refresh(this.getPreviousData(inst.getVal()));
      },
      onItemTap: (event, inst) => {
        if (event.value) { 
          let chain = this.getOptionsChain(event.value);
          if (this.hasChilds(chain, chain.length - 1)) {
            inst.refresh(this.getNextData(event.value));
          }
        } else { // if event.value = 0 - "<" back button clicked
          if (this.hasParent()) {
            inst.refresh(this.getPreviousData(this.parentValue));
          }
        }
      },
      onSet: (event, inst) => {
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
      this.select.setText = this.ngxTranslateService.instant('Välj');
      this.select.cancelText  = this.ngxTranslateService.instant('Avbryt');
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
      return this.field.label || defaultValue;
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
          group: this.hasParent() ? '<span style="float: left;" class="mbsc-ic mbsc-ic-fa-arrow-left"></span>' : '',
          value: elem.value,
          text: elem.text,
          html: elem.text + (elem.childs != null && elem.childs.length ? '<span style="float: right;" class="mbsc-ic mbsc-ic-fa-arrow-right"></span>' : '')
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

    private getOptionsChain(id: any) {
      if (this.dataSource == null || this.dataSource.length === 0) return [];

      let elems = new Array<MultiLevelOptionItem>();
      const searchNode = (elem: MultiLevelOptionItem, targetId: any): string => {
        if (elem.value == targetId) {
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
  }