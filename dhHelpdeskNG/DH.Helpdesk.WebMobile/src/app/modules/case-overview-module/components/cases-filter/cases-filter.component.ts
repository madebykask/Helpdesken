import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FilterMenuItemModel } from '../../models/cases-overview/filter-menu-item-model';
import { FavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { Subject } from 'rxjs';
import { CaseStandardSearchFilters } from '../../models/cases-overview/enums';

//presentation component
@Component({
  selector: 'cases-filter',
  templateUrl: './cases-filter.component.html',
  styleUrls: ['./cases-filter.component.scss']
})
export class CasesFilterComponent implements OnInit {

  @Input() favoriteFilters: FavoriteFilterModel[] = [];
  @Input() initialFilterId = 0;
  @Output() filterChanged: EventEmitter<any> = new EventEmitter<any>();

  filterMenuOptions = {
    theme: 'mobiscroll',
    type: 'hamburger',
    menuIcon: 'fa-filter',
    menuText: '',
    onMenuShow: function (event, inst) {
      if (!event.menu.element.classList.contains('case-search-filter')) {
        event.menu.element.classList.add('case-search-filter');
      }
    }
  };

  menuItems: FilterMenuItemModel[] = [];

  get isFilterApplied() {
    return this.menuItems.some(x => x.selected);
  }

  private filterId = 0;
  private destroy$ = new Subject<any>();

  constructor(private translateService: TranslateService) {
  }

  ngOnInit() {
    //console.log('>> casesFilter: ngOnInit');
    this.filterId = this.initialFilterId;
    this.initFilterMenu();
  }

  private initFilterMenu() {
    // build menu items
    this.menuItems =
      this.favoriteFilters && this.favoriteFilters.length
        ? this.favoriteFilters.map(f =>
            new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId))
        : [];
  }

  applyFilter(selectedItem: FilterMenuItemModel) {
    // update menu item
    selectedItem.selected = !selectedItem.selected;

    if (selectedItem && selectedItem.selected) {
      this.filterId = selectedItem.id;
    } else {
      this.filterId = +CaseStandardSearchFilters.AllCases;
    }
    this.raiseFilterChanged(selectedItem);
  }

  private raiseFilterChanged(selectedItem: FilterMenuItemModel) {
    if (selectedItem && selectedItem.selected) {
      this.filterChanged.emit({ filterId: selectedItem.id, filterName: selectedItem.text });
    } else {
      this.filterChanged.emit({ filterId: +CaseStandardSearchFilters.AllCases, filterName: null });
    }
  }

  trackByFn(index, item) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
