import { Component, OnInit, Input } from '@angular/core';
import { CaseAction, CaseActionsGroup, CaseLogActionData, CaseHistoryActionData } from '../../models';

@Component({
  selector: 'case-actions',
  templateUrl: './case-actions.component.html',
  styleUrls: ['./case-actions.component.scss']
})
export class CaseActionsComponent implements OnInit {

  @Input()
  items: CaseAction<any>[] = [];

  grouppedItems: CaseActionsGroup[];

  cardSettings = {
    theme: 'ios'
  }

  actionsListViewSettings = {
     
  };

  constructor() {
  }

  ngOnInit() {    
    //todo: move to onchanges?
    this.grouppedItems = this.processGroups(this.items);    
  }

  getActionIcon(caseAction: CaseAction<any>){
      const actionData = caseAction.Data;
      if (actionData instanceof CaseLogActionData){
        return "fa-comment-o";
      } else if (actionData instanceof CaseHistoryActionData){
        return "fa-history";
      } else {
        return "fa-user-secret";
      }
  }

  private processGroups(items: CaseAction<any>[]): CaseActionsGroup[] {
    let groups = new Array<CaseActionsGroup>();

    if (items != null && items.length > 0) {
      
      for (let item of items) {
          let groupIndex = this.findGroupIndex(item, groups);
          if (groupIndex == -1) 
          {
              let newGroup = new CaseActionsGroup(item.CreatedByUserId,  item.CreatedByUserName, item.CreatedAt);
              newGroup.Actions = new Array<CaseAction<any>>();
              groups.push(newGroup);
              groupIndex = groups.length - 1;
          }
          
          const group = groups[groupIndex];
          group.Actions.push(item);
      }
    }
    return groups;
  }

  private findGroupIndex(item:CaseAction<any>, groups: CaseActionsGroup[]){
      let userId = item.CreatedByUserId;
      let userGroups = groups.filter(gr => gr.CreatedByUserId === userId);
      //find matching user actions group by time ~1min
      if (userGroups && userGroups.length) {
          for (const group of groups) {
            const minDiff = (group.CreatedAt.getTime() - item.CreatedAt.getTime()) / 1000 / 60; //minutes
            if (minDiff < 1) {
              return  groups.indexOf(group);
            }
          }
      }       
      return -1;
  }
}
