import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FilterMenuItemModel } from '../../models/cases-overview/filter-menu-item-model';
import { CustomerFavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { Subject } from 'rxjs';
import { CaseStandardSearchFilters } from '../../models/cases-overview/enums';
import { LocalStorageService } from 'src/app/services/local-storage';

//presentation component
@Component({
  selector: 'cases-filter',
  templateUrl: './cases-filter.component.html',
  styleUrls: ['./cases-filter.component.scss']
})
export class CasesFilterComponent implements OnInit {

  @Input() favoriteFilters: CustomerFavoriteFilterModel[] = [];
  @Input() initialFilterId = '';
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

  private filterId = '';
  private destroy$ = new Subject<any>();

  constructor(private translateService: TranslateService, private localStorageService: LocalStorageService) {
  }

  ngOnInit() {
    //console.log('>> casesFilter: ngOnInit');
    this.filterId = this.initialFilterId;
    this.initFilterMenu();
  }

  private initFilterMenu() {
    // build menu items
    this.menuItems = [];
    if (!this.favoriteFilters || !this.favoriteFilters.length) {
      return;
    }
    const userData = this.localStorageService.getCurrentUser();
    const defaultCustomerId = userData.currentData.selectedCustomerId;
    const isOnlyOneCustomer = this.favoriteFilters.filter(cf => cf.customerId !== -1).length === 1;

    let tempMenuItems: FilterMenuItemModel[] = [];
    if (!isOnlyOneCustomer) {
      const allCustomersFilter = this.favoriteFilters.find(cf => cf.customerId === -1).favorites[0];
      if (allCustomersFilter) {
      tempMenuItems.push(new FilterMenuItemModel('' + allCustomersFilter.id,
        this.translateService.instant(allCustomersFilter.name),
        this.filterId && allCustomersFilter.id === this.filterId));
      }
    }

    this.favoriteFilters.filter(cf => cf.customerId === defaultCustomerId).forEach(cf => {
      if (!isOnlyOneCustomer) {
        tempMenuItems.push(new FilterMenuItemModel('' + cf.customerId, cf.customerName, false, true));
      }
      tempMenuItems = tempMenuItems.concat(cf.favorites.map(f =>
        new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId)));
      });

    this.favoriteFilters.filter(cf => cf.customerId > 0 && cf.customerId !== defaultCustomerId).forEach(cf => {
      if (!isOnlyOneCustomer) {
        tempMenuItems.push(new FilterMenuItemModel('' + cf.customerId, cf.customerName, false, true));
      }
      tempMenuItems = tempMenuItems.concat(cf.favorites.map(f =>
        new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId)));
     });
     this.menuItems = tempMenuItems;
  }

  applyFilter(selectedItem: FilterMenuItemModel) {
    if (selectedItem.disabled) {
      return;
    }
    // update menu item
    // this.menuItems.find(m => m.id === selectedItem.id).selected = !selectedItem.selected;
    const isSelected = !selectedItem.selected;

    if (selectedItem && isSelected) {
      this.filterId = selectedItem.id;
    } else {
      this.filterId = CaseStandardSearchFilters.AllCases;
    }
    this.raiseFilterChanged(selectedItem, isSelected);
    selectedItem.selected = isSelected;
  }

  private raiseFilterChanged(selectedItem: FilterMenuItemModel, isSelected: boolean) {
    if (selectedItem && isSelected) {
      this.filterChanged.emit({ filterId: selectedItem.id, filterName: selectedItem.text });
    } else {
      this.filterChanged.emit({ filterId: CaseStandardSearchFilters.AllCases, filterName: null });
    }
  }

  trackByFn(index, item) {
    return item.id + item.selected;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
