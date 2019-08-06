import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CaseSortFieldModel } from 'src/app/modules/case-edit-module/services/model/case-sort-field.model';
import { CasesSortMenuItemModel } from '../../models/cases-overview/case-sort-menu-item.model';
import { SortOrder } from 'src/app/modules/shared-module/constants';

@Component({
  selector: 'cases-sort-menu',
  templateUrl: './cases-sort-menu.component.html',
  styleUrls: ['./cases-sort-menu.component.scss']
})
export class CasesSortMenuComponent implements OnInit {
  @Output() sortingChanged = new EventEmitter<any>();
  @Input() selectedSortField: string;
  @Input() selectedSortFieldOrder = SortOrder.SortAsc;
  @Input() sortFields: CaseSortFieldModel[] = null;

  menuOptions = {
    theme: 'mobiscroll',
    type: 'hamburger',
    menuIcon: 'fa-sort',
    menuText: '',
    onMenuShow: function (event, inst) {
      if (!event.menu.element.classList.contains('cases-sort-menu')) {
        event.menu.element.classList.add('cases-sort-menu');
      }
    }
  };

  get isSortingApplied() {
    return this.menuItems && this.menuItems.some(m => m.selected);
  }

  menuItems: CasesSortMenuItemModel[] = [];

  ngOnInit() {
    this.initMenuItems();
  }

  private initMenuItems() {
    let itemId = 0;
    if (this.sortFields && this.sortFields.length) {
      this.menuItems =
        this.sortFields.map(x => x.fieldId === this.selectedSortField
            ? new CasesSortMenuItemModel(itemId++, x.text, x.fieldId, this.selectedSortFieldOrder, true)
            : new CasesSortMenuItemModel(itemId++, x.text, x.fieldId));
    }
  }

  getMenuItemIcon(item: CasesSortMenuItemModel) {
    if (item.selected) {
      return item.sortOrder === SortOrder.SortAsc ?  'mbsc-ic mbsc-ic-fa-sort-alpha-asc' : 'mbsc-ic mbsc-ic-fa-sort-alpha-desc';
    } else {
      return 'fa-no-icon';
    }
  }

  applySorting(menuItem: CasesSortMenuItemModel) {
    this.menuItems =
        this.menuItems.map(m => {
          if (m.id === menuItem.id) {
            if (menuItem.selected) {
              menuItem.sortOrder = menuItem.sortOrder == SortOrder.SortAsc ? SortOrder.SortDesc : SortOrder.SortAsc;
            } else {
              m.selected = true;
              menuItem.sortOrder = SortOrder.SortAsc;
            }
          } else {
             m.selected = false;
             m.sortOrder = SortOrder.SortAsc;
          }
          return m;
      });

      //raise change event
      this.sortingChanged.emit({ sortField: menuItem.fieldId, sortOrder: menuItem.sortOrder });
  }

  trackFn(item: CasesSortMenuItemModel): number {
    return item.id;
  }

}
