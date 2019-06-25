import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { TranslateService } from '@ngx-translate/core';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseSearchStateModel } from 'src/app/modules/shared-module/models/cases-overview/case-search-state.model';
import { CaseRouteReuseStrategy } from 'src/app/helpers/case-route-resolver.stategy';

@Component({
  selector: 'cases-filter',
  templateUrl: './cases-filter.component.html',
  styleUrls: ['./cases-filter.component.scss']
})
export class CasesFilterComponent implements OnInit {

  @ViewChild('navMenu') filterMenu;
  @Output() filterChanged: EventEmitter<any> = new EventEmitter<any>();

  get filterType(): CasesSearchType {
    const sel = this.getSelectedItem();
    return sel ? sel.id : CasesSearchType.AllCases;
  }

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
      //console.log('item tap: ' + (inst.id || ''));
    },
    onMenuShow: function (event, inst) {
      if (!event.menu.element.classList.contains('case-search-filter')) {
        event.menu.element.classList.add('case-search-filter');
      }
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
    //clear case page snapshots in reuse strategy
    CaseRouteReuseStrategy.deleteSnaphots();
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

  applyFilter(searchTypeId: number) {
    let searchType = <CasesSearchType>searchTypeId;
    const selectedItem = this.findItem(searchTypeId);
    selectedItem.selected = !selectedItem.selected;

    if (selectedItem && selectedItem.selected) {
      searchType = searchTypeId;
    } else {
      searchType = CasesSearchType.AllCases;
    }

    // save to storage
    this.saveSearchState(searchType);

    this.raiseFilterChanged(selectedItem);
  }

  private saveSearchState(searchType: CasesSearchType): any {
    let state = new CaseSearchStateModel();
    state.SearchType = searchType;
    this.localStorageService.setCaseSearchState(state);
  }

  private raiseFilterChanged(selectedItem) {
    if (selectedItem && selectedItem.selected) {
      this.filterChanged.emit({ type: selectedItem.id, name: selectedItem.text });
    } else {
      this.filterChanged.emit(null);
    }
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
