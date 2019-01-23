import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map, take } from "rxjs/operators";
import { StateSecondaryInputModel } from "src/app/models/stateSecondaries/stateSecondaryInputModel";
import { OptionItem } from "src/app/modules/shared-module/models";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";

@Injectable({ providedIn: 'root' })
export class StateSecondariesService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getStateSecondaries() {
        return this.getJson(this.buildResourseUrl('/api/statesecondaries/options', null, true, true))
        .pipe(
            take(1),
            map((jsItems: any) => {
                return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        );//TODO: error handling
    }

    getStateSecondary(id: number) {
      return this.getJson(this.buildResourseUrl(`/api/statesecondaries/${id}`, null, true, true))
      .pipe(
          take(1),
          map((jsItems: any) => {
            let model = new StateSecondaryInputModel();
            model.id = jsItems.id;
            model.changedDate = new Date(jsItems.changedDate);
            model.createdDate = new Date(jsItems.createdDate)
            model.customerId = jsItems.customerId;
            model.isActive = jsItems.isActive;
            model.isDefault = jsItems.isDefault;
            model.alternativeStateSecondaryName = jsItems.alternativeStateSecondaryName;
            model.noMailToNotifier = jsItems.noMailToNotifier;
            model.recalculateWatchDate = jsItems.recalculateWatchDate;
            model.workingGroupId = jsItems.workingGroupId;
            model.reminderDays = jsItems.reminderDays;
            return model;
          })
      );// TODO: error handling
    }
}