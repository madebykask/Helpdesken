import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
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