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
    isReadonly = true;
    @ViewChild('ctrl') myRef: any;

    ngOnChanges() {
    }

    ngOnInit(): void {
      mobiscroll.settings
    }

    ngOnDestroy(): void {
    }
     
  }