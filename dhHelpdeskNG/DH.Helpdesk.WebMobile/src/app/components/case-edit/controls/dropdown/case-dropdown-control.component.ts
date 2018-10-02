import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField, OptionItem } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscSelectOptions } from "@mobiscroll/angular";

@Component({
    selector: 'case-dropdown-control',
    templateUrl: './case-dropdown-control.component.html',
    styleUrls: ['./case-dropdown-control.component.scss']
  })
  export class CaseDropdownComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: OptionItem[] = [];
    private options: MbscSelectOptions = {
      readOnly: true,
      disabled: true
    }

    ngOnInit(): void {
      if(this.options.disabled) this.getFormControl(this.field.name).setValue(this.getText(this.field.value));
    }

    ngOnDestroy(): void {
    }

    getText(id: any) {
      if (this.dataSource == null || this.dataSource.length === 0) return "";
      let items = this.dataSource.filter((elem: OptionItem) => elem.value == id);
      return  items.length > 0 ? items[0].text : "";
    }

  }