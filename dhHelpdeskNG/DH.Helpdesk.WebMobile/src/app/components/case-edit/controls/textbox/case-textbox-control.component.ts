import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscInput } from "@mobiscroll/angular";

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
    @ViewChild('input') input: MbscInput;
    @Input() field: BaseCaseField<string>;
    options: any = {
      disabled: true,
      readonly: true
    }

    ngOnChanges() {
    }

    ngOnInit(): void {
      this.input.disabled = this.options.disabled
    }

    ngOnDestroy(): void {
    }
     
  }