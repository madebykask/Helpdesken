import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";

@Component({
    selector: 'case-textarea-control',
    templateUrl: './case-textarea-control.component.html',
    styleUrls: ['./case-textarea-control.component.scss']
  })
  export class CaseTextareaComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
    @ViewChild('input') control: any;
    @Input() field: BaseCaseField<string>;

    ngOnChanges() {
    }

    ngOnInit(): void {
      this.control.disabled = true;
    }

    ngOnDestroy(): void {
    }
         
  }