import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";

@Component({
    selector: 'case-switch-control',
    templateUrl: './case-switch-control.component.html',
    styleUrls: ['./case-switch-control.component.scss']
  })
  export class CaseSwitchComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<boolean>;
    @Input() description: string = "";

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
    }

  }