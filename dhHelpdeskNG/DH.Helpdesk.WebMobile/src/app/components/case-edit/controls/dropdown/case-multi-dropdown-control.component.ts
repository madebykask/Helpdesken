import { OnInit, OnDestroy, Component, Input, OnChanges } from "@angular/core";
import { BaseCaseField, MultiLevelOptionItem } from "../../../../models";
import { BaseControl } from "../base-control";


@Component({
    selector: 'case-multi-dropdown-control',
    templateUrl: './case-multi-dropdown-control.component.html',
    styleUrls: ['./case-multi-dropdown-control.component.scss']
  })
  export class CaseMultiDropdownComponent extends BaseControl implements OnInit, OnDestroy {
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: MultiLevelOptionItem[] = [];
    private options: any = {
      readOnly: true,
      disabled: true
    } 

    ngOnInit(): void {
      if(this.options.disabled) this.getFormControl(this.field.name).setValue(this.getText(this.field.value));
    }

    ngOnDestroy(): void {
    }

    getText(id: any) {
      if (this.dataSource == null || this.dataSource.length === 0) return "";
      let texts = new Array<string>();
      const searchNode = (elem: MultiLevelOptionItem, targetId: any): string => {
        if (elem.value == targetId) {
           texts.push(elem.text);
           return elem.text; 
          }
        if (elem.childs != null) {
          for(let i = 0; i < elem.childs.length; i++) {
            let text = searchNode(elem.childs[i], targetId);
            if (text != null) { 
              texts.push(elem.text);
              return text;
            }
          }
        }
        return null;
      }
      this.dataSource.forEach((elem: MultiLevelOptionItem) => searchNode(elem, id))
      texts = texts.reverse();
      return  texts.length > 0 ? texts.join(" -> ") : "";
    }


  }