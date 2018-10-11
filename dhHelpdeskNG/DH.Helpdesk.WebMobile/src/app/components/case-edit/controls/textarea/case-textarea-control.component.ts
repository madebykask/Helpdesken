import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscTextarea } from "@mobiscroll/angular";

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
    @ViewChild('input') input: MbscTextarea;
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