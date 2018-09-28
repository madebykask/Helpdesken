import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField } from "../../../../models";
import { BaseControl } from "../base-control";


@Component({
    selector: 'case-multi-dropdown-control',
    templateUrl: './case-multi-dropdown-control.component.html',
    styleUrls: ['./case-multi-dropdown-control.component.scss']
  })
  export class CaseMultiDropdownComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<number>;


    ngOnInit(): void {
    }

    ngOnDestroy(): void {
    }

  }