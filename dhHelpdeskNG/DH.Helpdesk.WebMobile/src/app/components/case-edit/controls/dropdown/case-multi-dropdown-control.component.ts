import { OnInit, OnDestroy, Component, Input, OnChanges, ViewChild } from "@angular/core";
import { BaseCaseField, MultiLevelOptionItem } from "../../../../models";
import { BaseControl } from "../base-control";


@Component({
    selector: 'case-multi-dropdown-control',
    templateUrl: './case-multi-dropdown-control.component.html',
    styleUrls: ['./case-multi-dropdown-control.component.scss']
  })
  export class CaseMultiDropdownComponent extends BaseControl implements OnInit, OnDestroy {
    @ViewChild('control') control: any;
    @Input() field: BaseCaseField<number>;
    @Input() dataSource: MultiLevelOptionItem[] = [];
    text: string = "";
    options: any = {
      readOnly: true,
      disabled: true
    } 

    ngOnInit(): void {
      //if(this.readOnly) set disabled/reaonly mode
      this.init(this.field);
      this.text = this.getText(this.field.value);
      this.control.disabled = true;
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
      return  texts.length > 0 ? texts.join(" > ") : "";
    }
  }