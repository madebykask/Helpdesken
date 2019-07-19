import { Injectable } from '@angular/core';
import { StoreBase } from '../modules/shared-module/store/store-base';
import { CaseTemplateModel } from '../models/caseTemplate/case-template.model';
import { FavoriteFilterModel } from '../modules/case-overview-module/models/cases-overview/favorite-filter.model';

//app state keys
export const AppStoreKeys = {
  Templates: 'templates',
  FavoriteFilters: 'favFilters'
};

//app state
export class AppState {
  templates: CaseTemplateModel[] = [];
  favFilters: FavoriteFilterModel[] = [];
}

@Injectable({ providedIn: 'root' })
export class AppStore extends StoreBase<AppState> {
  constructor() {
    super(new AppState());
  }
}
