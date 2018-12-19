import { Injectable } from "@angular/core";
import { CaseDataStore } from "./case-data.store";
import { CaseFieldsNames } from "src/app/helpers/constants";
import { of } from "rxjs";

export class CaseDataReducers {
  constructor(private _caseDataStore: CaseDataStore) {
  }

  caseDataReducer(action: string, payload: any) {
    switch (action) {
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