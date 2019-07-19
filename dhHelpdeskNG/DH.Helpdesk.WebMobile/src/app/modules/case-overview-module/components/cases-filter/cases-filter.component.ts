import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocalStorageService } from 'src/app/services/local-storage';
import { FilterMenuItemModel } from '../../models/cases-overview/filter-menu-item-model';
import { FavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { takeUntil, distinctUntilChanged, filter } from 'rxjs/operators';
import { AppStore, AppStoreKeys } from 'src/app/store';
import { Subject } from 'rxjs';
import { SearchFilterService } from '../../services/cases-overview/search-filter.service';
import { CaseStandardSearchFilters } from '../../models/cases-overview/enums';

@Component({
  selector: 'cases-filter',
  templateUrl: './cases-filter.component.html',
  styleUrls: ['./cases-filter.component.scss']
})
export class CasesFilterComponent implements OnInit {

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

  get filterName() {
    const sel = this.getSelectedItem();
    return sel ? sel.text : null;
  }

  get isFilterApplied() {
    return this.menuItems.some(x => x.selected);
  }

  get selectedFilter(): FavoriteFilterModel {
    const selectedFilterMenuItem = this.getSelectedItem();
    if (selectedFilterMenuItem && selectedFilterMenuItem.id) {
      const res = this.favoriteFilters.filter(f => f.id === selectedFilterMenuItem.id);
      return res && res.length ? res[0] : null;
    }
    return null;
  }

  private favoriteFilters: FavoriteFilterModel[] = [];
  private filterId = 0;
  private destroy$ = new Subject<any>();

  constructor(private appStore: AppStore,
              private searchFilterService: SearchFilterService,
              private translateService: TranslateService) {
  }

  ngOnInit() {
    //get selected filter state from local storage
    this.filterId = this.searchFilterService.getFilterIdFromState();

    // loading filters from app store state
    this.appStore.select<FavoriteFilterModel[]>(AppStoreKeys.FavoriteFilters).pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      filter(Boolean) // aka new Boolean(val) to filter null values
    ).subscribe((filters: FavoriteFilterModel[]) => {
      this.favoriteFilters = filters || [];
      this.initFilterMenu();
    });
  }

  private initFilterMenu() {
    // build menu items
    this.menuItems =
      this.favoriteFilters && this.favoriteFilters.length
        ? this.favoriteFilters.map(f =>
            new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId))
        : [];

    // update filter state - prev selected filter could be deleted or incorrect...
    const selectedFilter = this.getSelectedItem();
    this.filterId = selectedFilter ? selectedFilter.id : +CaseStandardSearchFilters.AllCases;
    this.searchFilterService.saveFilterIdToState(this.filterId);
  }

  applyFilter(selectedFilterId: number) {
    // update menu item
    const selectedItem = this.findItem(selectedFilterId);
    selectedItem.selected = !selectedItem.selected;

    if (selectedItem && selectedItem.selected) {
      this.filterId = selectedItem.id;
    } else {
      this.filterId = +CaseStandardSearchFilters.AllCases;
    }

    // save to storage
    this.searchFilterService.saveFilterIdToState(this.filterId);

    this.raiseFilterChanged(selectedItem);
  }

  private raiseFilterChanged(selectedItem: FilterMenuItemModel) {
    if (selectedItem && selectedItem.selected) {
      this.filterChanged.emit({ filterId: selectedItem.id, filterName: selectedItem.text });
    } else {
      this.filterChanged.emit({ filterId: +CaseStandardSearchFilters.AllCases, filterName: null }); 
    }
  }

  private findItem(itemId: number) {
    const res = this.menuItems.filter(x => x.id === itemId);
    if (res && res.length) {
      return res[0];
    }
    return null;
  }

  private getSelectedItem(): FilterMenuItemModel {
    const res = this.menuItems.filter(x => x.selected === true);
    if (res && res.length) {
      return res[0];
    }
    return null;
  }

  trackByFn(index, item) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
