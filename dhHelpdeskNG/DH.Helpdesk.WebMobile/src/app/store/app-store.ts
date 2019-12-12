import { Injectable } from '@angular/core';
import { StoreBase } from '../modules/shared-module/store/store-base';
import { CaseTemplateModel } from '../models/caseTemplate/case-template.model';
import { CustomerFavoriteFilterModel } from '../modules/case-overview-module/models/cases-overview/favorite-filter.model';

//app state keys
export const AppStoreKeys = {
  Templates: 'templates',
  FavoriteFilters: 'favFilters'
};

//app state
// TODO: split into 2 observables instead of 1. Now on change of 1 property, subscribers of 2nd also notified
export class AppState {
  templates: CaseTemplateModel[] = [];
  favFilters: CustomerFavoriteFilterModel[] = [];
}

@Injectable({ providedIn: 'root' })
export class AppStore extends StoreBase<AppState> {
  constructor() {
    super(new AppState());
  }
}
