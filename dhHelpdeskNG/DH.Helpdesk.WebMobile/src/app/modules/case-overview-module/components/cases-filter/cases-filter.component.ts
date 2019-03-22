import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { TranslateService } from '@ngx-translate/core';
import { LocalStorageService } from 'src/app/services/local-storage';

@Component({
  selector: 'cases-filter',
  templateUrl: './cases-filter.component.html',
  styleUrls: ['./cases-filter.component.scss']
})
export class CasesFilterComponent implements OnInit {
  searchType = CasesSearchType;
  @ViewChild('navMenu') filterMenu;

  @Output() filterChanged:EventEmitter<any> = new EventEmitter<any>();

  get filterName() {
    const sel = this.getSelectedItem();
    return sel ? sel.text : null;
  }

  filterMenuOptions = {
    theme: 'mobiscroll',
    type: 'hamburger',
    menuIcon: 'fa-filter',
    menuText: '',
    onItemTap: function (event, inst) {
      console.log('item tap: ' + (inst.id || ''));
    },
    onMenuShow: function (event, inst) {
      if (!event.menu.element.classList.contains('case-search-filter'))
        event.menu.element.classList.add('case-search-filter');    
    }
  }

  menuItems = [{
    id: +CasesSearchType.MyCases,
    text: this.translateService.instant('Mina ärenden'),
    selected: false
  }];

  constructor(private router: Router,
              private localStorageService: LocalStorageService,
              private translateService: TranslateService) {
    this.initMenuItems();
  } 

  get isFilterApplied() {
    return this.menuItems.some(x => x.selected);
  }

  ngOnInit() {
  }

  initMenuItems() {
   let selectedItem = null;
    const state = this.localStorageService.getCaseSearchState();
    if (state) {
      selectedItem = this.findItem(+state.SearchType);
      if (selectedItem) {
        selectedItem.selected = true;
      }
    }
    this.raiseFilterChanged(selectedItem);
  }

  applyFilter(searchType: string) {
    const searchTypeId = +searchType;
    
    const selectedItem = this.findItem(searchTypeId);    
    selectedItem.selected = !selectedItem.selected;

    if (selectedItem.selected) {
      this.router.navigate(['/casesoverview', CasesSearchType[selectedItem.id]]);
    } else {
      this.router.navigate(['/casesoverview', CasesSearchType[CasesSearchType.AllCases]]);
    }

    this.raiseFilterChanged(selectedItem);    
  }

  private raiseFilterChanged(selectedItem) {
    this.filterChanged.emit(selectedItem && selectedItem.selected ? selectedItem.text : null);
  }

  private findItem(searchTypeId: number) {
    const res = this.menuItems.filter(x => x.id === searchTypeId);
    if (res && res.length) {
      return res[0];
    }
    return null;
  }

  private getSelectedItem() {
    const res = this.menuItems.filter(x => x.selected);
    if (res && res.length) {
      return res[0];
    }
    return null;
  }

  trackByFn(index, item) {
    return item.id;
  }

}
