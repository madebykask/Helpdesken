import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField, OptionItem } from "../../../../models";
import { BaseControl } from "../base-control";
import { MbscSelectOptions } from "@mobiscroll/angular";

@Component({
    selector: 'case-dropdown-control',
    templateUrl: './case-dropdown-control.component.html',
    styleUrls: ['./case-dropdown-control.component.scss']
  })
  export class CaseDropdownComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: OptionItem[] = [];
    private options: MbscSelectOptions = {
      readOnly: true,
      disabled: true
    }

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
    }

  }