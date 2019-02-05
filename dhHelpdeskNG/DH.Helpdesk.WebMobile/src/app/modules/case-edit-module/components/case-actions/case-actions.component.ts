import { Component, OnInit, Input } from '@angular/core';
import { CaseAction, CaseActionsGroup, CaseLogActionData, CaseHistoryActionData, CaseActionDataType } from '../../models';
import { AuthenticationStateService } from 'src/app/services/authentication';

@Component({
  selector: 'case-actions',
  templateUrl: './case-actions.component.html',
  styleUrls: ['./case-actions.component.scss']
})
export class CaseActionsComponent implements OnInit {

  @Input() 
  caseKey:string;

  @Input() 
  items: CaseAction<CaseActionDataType>[] = [];

  grouppedItems: CaseActionsGroup[];
  cardSettings = {}
  actionsListViewSettings = {};

  constructor(private authStateService: AuthenticationStateService) {
  }

  isActionOwner(userId: number) {
    return userId === this.authStateService.getUser().id;
  }

  ngOnInit() {    
    //todo: move to onchanges?
    this.grouppedItems = this.processGroups(this.items);    
  } 

  private processGroups(items: CaseAction<CaseActionDataType>[]): CaseActionsGroup[] {
    let groups = new Array<CaseActionsGroup>();

    if (items != null && items.length > 0) {
      
      for (let item of items) {
          let groupIndex = this.findGroupIndex(item, groups);
          if (groupIndex == -1) 
          {
              let newGroup = new CaseActionsGroup(item.createdBy, item.createdAt);
              newGroup.Actions = new Array<CaseAction<CaseActionDataType>>();
              groups.push(newGroup);
              groupIndex = groups.length - 1;
          }
          
          const group = groups[groupIndex];
          group.Actions.push(item);
      }
    }
    //sort by time desc
    groups = groups.sort((a,b) => b.createdAt.getTime() - a.createdAt.getTime());
    return groups;
  }

  private findGroupIndex(item:CaseAction<CaseActionDataType>, groups: CaseActionsGroup[]){            
      let userGroups = groups.filter(gr => gr.createdBy === item.createdBy);
      //find matching user actions group by time ~1min
      if (userGroups && userGroups.length) {
          for (const group of groups) {            
            const minDiff = (group.createdAt.getTime() - item.createdAt.getTime()) / 1000; //seconds
            if (Math.abs(minDiff) < 5) {
              return  groups.indexOf(group);
            }
          }
      }       
      return -1;
  }
  
  getTrackId(index, item){
    return item.id;
  }
}
