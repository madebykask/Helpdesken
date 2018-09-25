import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscDatetimeOptions } from "@mobiscroll/angular";

@Component({
    selector: 'case-date-control',
    templateUrl: './case-date-control.component.html',
    styleUrls: ['./case-date-control.component.scss']
  })
  export class CaseDateComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<Date>;
    options: MbscDatetimeOptions = {
      readOnly: true,
      disabled: true
    }

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
    }

  }