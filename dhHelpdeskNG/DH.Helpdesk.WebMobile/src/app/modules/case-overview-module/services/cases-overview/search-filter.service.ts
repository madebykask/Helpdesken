import { Injectable } from '@angular/core';
import { CasesOverviewFilter } from '../../models/cases-overview/cases-overview-filter.model';
import { FavoriteFilterModel, CustomerFavoriteFilterModel } from '../../models/cases-overview/favorite-filter.model';
import { FilterFields, CaseStandardSearchFilters, InitiatorSearchScope } from '../../models/cases-overview/enums';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';
import { SearchFilterApiService } from './search-filter-api.service';
import { map, catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import * as cm from '../../../shared-module/utils/common-methods';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseSearchStateModel } from 'src/app/modules/shared-module/models/cases-overview/case-search-state.model';
import { TranslateService } from '@ngx-translate/core';
import { CaseOverviewConstants } from 'src/app/modules/shared-module/constants';

@Injectable({ providedIn: 'root' })
export class SearchFilterService {
  constructor(private searchFilterApiService: SearchFilterApiService,
              private localStorageService: LocalStorageService,
              private ngxTranslateService: TranslateService) {
  }

  getFilterIdFromState(): string {
    const searchState = this.localStorageService.getCaseSearchState();
    const filterId = searchState && searchState.filterId ? searchState.filterId : CaseStandardSearchFilters.AllCases;
    return filterId || CaseStandardSearchFilters.AllCases;
  }

  saveFilterIdToState(filterId: string): any {
    const state = this.localStorageService.getCaseSearchState() || new CaseSearchStateModel();
    state.filterId = filterId;
    this.localStorageService.setCaseSearchState(state);
  }

  loadFavoriteFilters(includeStandard: boolean = true): Observable<Array<CustomerFavoriteFilterModel>> {
    return this.searchFilterApiService.getFavoritersFilters().pipe(
      map((data: any[]) => {
        let filters = new Array<CustomerFavoriteFilterModel>();

        const allCustomers = new CustomerFavoriteFilterModel();
        allCustomers.customerId = -1;
        allCustomers.customerName = 'All';
        allCustomers.favorites = this.createDefaultSearchFilters(CaseStandardSearchFilters.MyCases);
        filters.push(allCustomers);

        if (data && data.length) {
          filters = filters.concat(data.map(f => <CustomerFavoriteFilterModel>Object.assign(new CustomerFavoriteFilterModel(), {
            customerId: f.customerId,
            customerName: f.customerName,
            favorites: this.createFavoritesFilters(f.favorites, includeStandard, f.customerId)
          })));
        }

        return filters;
      }),
      catchError(err => throwError(err))
    );
  }

  applyFavoriteFilter(filter: CasesOverviewFilter, selectedFilter: FavoriteFilterModel) {
    if (selectedFilter || (!isNaN(+selectedFilter.id) && +selectedFilter.id > 0) && selectedFilter.fields) {
      for (const fieldKey of Object.keys(selectedFilter.fields)) {
          const filterField = <FilterFields>FilterFields[fieldKey];
          switch (filterField) {
            case FilterFields.InitiatorFilter:
              filter.Initiator = this.getFilterFieldValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.InitiatorSearchScopeFilter:
              filter.InitiatorSearchScope = this.getFilterFieldSingleValue(fieldKey, selectedFilter.fields);
              filter.InitiatorSearchScope = filter.InitiatorSearchScope == null 
                ? +InitiatorSearchScope.UserAndIsAbout 
                : filter.InitiatorSearchScope
              break;
            case FilterFields.RegionFilter:
              filter.RegionIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.DepartmentFilter:
              filter.DepartmentIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.CaseTypeFilter:
              filter.CaseTypeId = this.getFilterFieldSingleValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.ProductAreaFilter:
              filter.ProductAreaId = this.getFilterFieldSingleValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.WorkingGroupFilter:
              filter.WorkingGroupIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.ResponsibleFilter:
              filter.ResponsibleUserIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.AdministratorFilter:
              filter.PerfomerUserIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.PriorityFilter:
              filter.PriorityIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.StatusFilter:
              filter.StatusIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.SubStatusFilter:
              filter.StateSecondaryIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.RemainingTimeFilter:
              filter.RemainingTime = this.getFilterFieldSingleValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.ClosingReasonFilter:
              filter.CaseClosingReasonId = this.getFilterFieldSingleValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.RegisteredByFilter:
              filter.RegisteredByIds = this.getFilterFieldMultiValue(fieldKey, selectedFilter.fields);
              break;
            case FilterFields.RegistrationDateFilter:
              const registrationDateRange = this.getFilterFieldDateRangeValue(fieldKey, selectedFilter.fields);
              filter.CaseRegistrationDateStartFilter = registrationDateRange[0];
              filter.CaseRegistrationDateEndFilter = registrationDateRange[1];
              break;
            case FilterFields.WatchDateFilter:
              const watchDateRange = this.getFilterFieldDateRangeValue(fieldKey, selectedFilter.fields);
              filter.CaseWatchDateStartFilter = watchDateRange[0];
              filter.CaseWatchDateEndFilter = watchDateRange[1];
              break;
            case FilterFields.ClosingDateFilter:
              const caseClosingDateRange = this.getFilterFieldDateRangeValue(fieldKey, selectedFilter.fields);
              filter.CaseClosingDateStartFilter = caseClosingDateRange[0];
              filter.CaseClosingDateEndFilter = caseClosingDateRange[1];
              break;

            default:
              break;
          }
      }
    }
  }

  getFilterFieldValue(fieldKey: string, fields: { [key: string]: string }): string | null {
    let res = null;
    if (fieldKey in fields) {
      const fieldVal = fields[fieldKey];
      res = fieldVal;
    }
    return res;
  }

  getFilterFieldSingleValue(fieldKey: string, fields: { [key: string]: string }): number | null {
    let res = null;
    if (fieldKey in fields) {
      const fieldVal = fields[fieldKey];
      res = fieldVal;
    }
    return res;
  }

  getFilterFieldMultiValue(fieldKey: string, fields: { [key: string]: string }): number[] | null {
    let res = null;
    if (fieldKey in fields) {
      const fieldVal = fields[fieldKey] || '';
      if (fieldVal) {
        const values = fieldVal.split(',');
        if (values && values.length) {
          res = values.map(x => +x).filter(x => !isNaN(x));
        }
      }
    }
    return res;
  }

  getFilterFieldDateRangeValue(fieldKey: string, fields: { [key: string]: string }): [Date?, Date?] {
    let startDate = null;
    let endDate = null;
    if (fieldKey in fields) {
        const fieldVal = fields[fieldKey] || '';
        if (fieldVal) {
          const values = fieldVal.split(',');
          if (values && values.length === 2) {
            startDate = DateUtil.tryConvertToDate(values[0], true);
            endDate = DateUtil.tryConvertToDate(values[1], true);
          }
        }
    }
    return [startDate, endDate];
  }

  private createFavoritesFilters(favorites: any, includeStandard: boolean, customerId: number): FavoriteFilterModel {
    let filters = favorites && favorites.length ? favorites.map(f => <FavoriteFilterModel>Object.assign(new FavoriteFilterModel(), {
      id: '' + f.id,
      name: f.name,
      fields: cm.convertNameValueArrayToObject(f.fields)
    })) : [];

    //add fixed filters if required
    if (includeStandard) {
      const standardFilters = this.createDefaultSearchFilters(CaseOverviewConstants.CaseOverviewCustomerPrefix + customerId);
      filters = [...standardFilters, ...filters];
    }
    return filters;
  }

  private createDefaultSearchFilters(id: any): FavoriteFilterModel[] {
    // standard/fix filters will have negative Ids
    const myCasesFilter = new FavoriteFilterModel();
    myCasesFilter.id = id;
    myCasesFilter.name = this.ngxTranslateService.instant('Mina ärenden'); // My cases
    return [ myCasesFilter ];
  }

}

