import { Component, OnInit, Output, EventEmitter, Input, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FilterMenuItemModel } from '../../models/cases-overview/filter-menu-item-model';
import { CustomerFavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { Subject } from 'rxjs';
import { CaseStandardSearchFilters } from '../../models/cases-overview/enums';
import { LocalStorageService } from 'src/app/services/local-storage';
import { MbscHamburgerNav } from '@mobiscroll/angular';

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

  @ViewChild('overviewFilter', { static: false}) filterCtrl: any;

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

  onItemTap(event) {
    if (event.target.dataset['id']) {
      const itemId = event.target.dataset['id'];
      const selectedItem = this.menuItems.find(m => m.id === itemId);
      if (!selectedItem.disabled) {
        this.applyFilter(selectedItem);
      }
    }
  }

  trackByFn(index, item) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private applyFilter(selectedItem: FilterMenuItemModel) {
    if (selectedItem.disabled) {
      return;
    }
    // update menu item
    const isSelected = !selectedItem.selected;
    // clear all previous values
    this.menuItems.filter(m => m.selected && m.id !== selectedItem.id)
                  .forEach(m => m.selected = false);
    selectedItem.selected = isSelected;
    if (!isSelected) {
      this.filterCtrl.instance.deselect(selectedItem.id);
    }

    if (selectedItem && isSelected) {
      this.filterId = selectedItem.id;
    } else {
      this.filterId = CaseStandardSearchFilters.AllCases;
    }
    this.raiseFilterChanged(selectedItem, isSelected);
  }

  private raiseFilterChanged(selectedItem: FilterMenuItemModel, isSelected: boolean) {
    if (selectedItem && isSelected) {
      this.filterChanged.emit({ filterId: selectedItem.id, filterName: selectedItem.text });
    } else {
      this.filterChanged.emit({ filterId: CaseStandardSearchFilters.AllCases, filterName: null });
    }
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
    // add all customers "My cases" option first
    if (!isOnlyOneCustomer) {
      const allCustomersFilter = this.favoriteFilters.find(cf => cf.customerId === -1).favorites[0];
      if (allCustomersFilter) {
      tempMenuItems.push(new FilterMenuItemModel('' + allCustomersFilter.id,
        this.translateService.instant(allCustomersFilter.name),
        this.filterId && allCustomersFilter.id === this.filterId));
      }
    }

    // add default customer
    this.favoriteFilters.filter(cf => cf.customerId === defaultCustomerId).forEach(cf => {
      if (!isOnlyOneCustomer) {
        tempMenuItems.push(new FilterMenuItemModel('' + cf.customerId, cf.customerName, false, true));
      }
      tempMenuItems = tempMenuItems.concat(cf.favorites.map(f =>
        new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId)));
      });

    // add other customers
    this.favoriteFilters.filter(cf => cf.customerId > 0 && cf.customerId !== defaultCustomerId).forEach(cf => {
      if (!isOnlyOneCustomer) {
        tempMenuItems.push(new FilterMenuItemModel('' + cf.customerId, cf.customerName, false, true));
      }
      tempMenuItems = tempMenuItems.concat(cf.favorites.map(f =>
        new FilterMenuItemModel(f.id, this.translateService.instant(f.name), this.filterId && f.id === this.filterId)));
     });
     this.menuItems = tempMenuItems;
  }
}
