import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { mobiscroll } from "@mobiscroll/angular";

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
    @Input() field: BaseCaseField<string>;
    private options: any = {
      disabled: true,
      readonly: true
    }

    ngOnChanges() {
    }

    ngOnInit(): void {
      //if(this.readOnly) set disabled/reaonly mode
    }

    ngOnDestroy(): void {
    }
     
  }