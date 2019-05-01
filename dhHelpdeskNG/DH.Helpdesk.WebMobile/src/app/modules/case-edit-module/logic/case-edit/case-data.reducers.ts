import { Injectable } from '@angular/core';
import { CaseDataStore } from './case-data.store';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { of } from 'rxjs';

// Updates controls(dropdown & etc.) datasources
export class CaseDataReducers {
  constructor(private _caseDataStore: CaseDataStore) {
  }

  caseDataReducer(action: string, payload: any) {
    switch (action) {
      case CaseFieldsNames.DepartmentId: {
        return this._caseDataStore.departmentsStore$.next(payload.items);
      }
      case CaseFieldsNames.OrganizationUnitId: {
        return this._caseDataStore.oUsStore$.next(payload.items);
      }
      case CaseFieldsNames.IsAbout_DepartmentId: {
        return this._caseDataStore.isAboutDepartmentsStore$.next(payload.items);
      }
      case CaseFieldsNames.IsAbout_OrganizationUnitId: {
        return this._caseDataStore.isAboutOUsStore$.next(payload.items);
      }
      case CaseFieldsNames.ProductAreaId: {
        return this._caseDataStore.productAreasStore$.next(payload.items);
      }
      case CaseFieldsNames.PerformerUserId: {
        return this._caseDataStore.performersStore$.next(payload.items);
      }
      case CaseFieldsNames.WorkingGroupId: {
        return this._caseDataStore.workingGroupsStore$.next(payload.items);
      }

      default:
        return of(null);
      // throw new Error(`Action for case field ${action} is not implemented.`);
    }
  }
  // var workingGroup$ = this._workingGroupsService.getWorkingGroup(payload.value);
}

@Injectable({ providedIn: 'root' })
export class CaseDataReducersFactory {
  constructor() {
  }

  public createCaseDataReducers(caseDataStore: CaseDataStore, ) {
    return new CaseDataReducers(caseDataStore);
  }
}