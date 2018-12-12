import { Injectable } from "@angular/core";
import { CaseDataStore } from "./case-data.store";
import { OptionItem, CaseOptionsFilterModel } from "src/app/models";
import { CaseFieldsNames } from "src/app/helpers/constants";
import { CaseOrganizationService } from "src/app/services/case-organization";
import { defaultIfEmpty, switchMap } from "rxjs/operators";
import { empty, of } from "rxjs";


export class CaseDataReducers {
  private empty$ = () => empty().pipe(defaultIfEmpty(null));
  private fieldExists = (field: any) => field !== undefined;
  constructor(private _caseDataStore: CaseDataStore, private _caseOrganizationService: CaseOrganizationService) {
  }

  caseDataReducer(action: string, caseFilters: CaseOptionsFilterModel) {
    switch (action) {
      case CaseFieldsNames.WorkingGroupId: {
        return this.getPerfomers(caseFilters);
      }
      default:
        return of(null);
       // throw new Error(`Action for case field ${action} is not implemented.`);
    }
  }

  private getPerfomers(caseFilters: CaseOptionsFilterModel) {
    let perfomers$ = this.fieldExists(caseFilters.Performers) ?
      this._caseOrganizationService.getPerformers(null, caseFilters.CaseWorkingGroupId) :
      this.empty$();
    return perfomers$.pipe(switchMap((o: OptionItem[]) => {
      this._caseDataStore.performersStore$.next(o);
      return of(o);
    }));
  }
}

@Injectable({ providedIn: 'root' })
export class CaseDataReducersFactory {
  constructor(private _caseOrganizationService: CaseOrganizationService) {

  }
  public createCaseDataReducers(caseDataStore: CaseDataStore) {
    return new CaseDataReducers(caseDataStore, this._caseOrganizationService);
  }
}