import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";

@Component({
    selector: 'case-switch-control',
    templateUrl: './case-switch-control.component.html',
    styleUrls: ['./case-switch-control.component.scss']
  })
  export class CaseSwitchComponent extends BaseControl implements OnInit, OnDestroy {
    @ViewChild('control') control: any;
    @Input() field: BaseCaseField<boolean>;
    @Input() description: string = "";

    ngOnInit(): void {
      this.init(this.field);
      this.control.disabled = true;
    }

    ngOnDestroy(): void {
    }

  }