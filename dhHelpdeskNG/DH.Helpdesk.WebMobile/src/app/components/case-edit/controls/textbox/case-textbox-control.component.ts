import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";

@Component({
    selector: 'case-textbox-control',
    templateUrl: './case-textbox-control.component.html',
    styleUrls: ['./case-textbox-control.component.scss']
  })
  export class CaseTextboxComponent extends BaseControl implements OnChanges, OnInit, OnDestroy {
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