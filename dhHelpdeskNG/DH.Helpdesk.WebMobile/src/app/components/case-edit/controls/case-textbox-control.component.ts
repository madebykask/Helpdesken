import { OnInit, OnDestroy, Component, Input, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../models";
import { BaseControl } from "./base-control";

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<string>;
    isReadonly = true;
    myRef: any;

    ngOnInit(): void {
      this.form.get(this.field.Name).disable();//TODO: dynamic
    }

    ngOnDestroy(): void {
    }
     
  }