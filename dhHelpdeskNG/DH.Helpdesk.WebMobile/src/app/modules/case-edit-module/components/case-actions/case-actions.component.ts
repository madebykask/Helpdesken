import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CaseAction, CaseActionsGroup, CaseActionDataType } from '../../models';
import { AuthenticationStateService } from 'src/app/services/authentication';

@Component({
  selector: 'case-actions',
  templateUrl: './case-actions.component.html',
  styleUrls: ['./case-actions.component.scss'],
})
export class CaseActionsComponent implements OnChanges {
  @Input() caseKey: string;
  @Input() customerId: number;
  @Input() items: Array<CaseAction<CaseActionDataType>>;

  isLoaded = false;
  grouppedItems: CaseActionsGroup[];
  cardSettings = {};
  actionsListViewSettings = {};

  constructor(private authStateService: AuthenticationStateService) {
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.items && changes.items.currentValue && changes.items.currentValue.length) {
      this.grouppedItems = this.processGroups(this.items);
      this.isLoaded = true;
    }
  }

  private processGroups(items: CaseAction<CaseActionDataType>[]): CaseActionsGroup[] {
    let groups = new Array<CaseActionsGroup>();

    if (items != null && items.length > 0) {

      for (const item of items) {
          let groupIndex = this.findGroupIndex(item, groups);
          if (groupIndex == -1) {
              const newGroup = new CaseActionsGroup(item.createdBy, item.createdAt);
              newGroup.Actions = new Array<CaseAction<CaseActionDataType>>();
              groups.push(newGroup);
              groupIndex = groups.length - 1;
          }

          const group = groups[groupIndex];
          group.Actions.push(item);
      }
    }
    //sort by time desc
    groups = groups.sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime());
    return groups;
  }

  private findGroupIndex(item: CaseAction<CaseActionDataType>, groups: CaseActionsGroup[]) {
      const userGroups = groups.filter(gr => gr.createdBy === item.createdBy);
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

  isActionOwner(userId: number) {
    return userId === this.authStateService.getUser().id;
  }

  getTrackId(index, item) {
    return item.id;
  }
}
