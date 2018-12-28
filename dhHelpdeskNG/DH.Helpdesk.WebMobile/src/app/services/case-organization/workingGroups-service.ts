import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map, take } from "rxjs/operators";
import { WorkingGroupInputModel } from "src/app/models/workinggroups/workingGroup-input.model";
import { OptionItem } from "src/app/modules/shared-module/models";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";

@Injectable({ providedIn: 'root' })
export class WorkingGroupsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getWorkingGroups() {
        return this.getJson(this.buildResourseUrl('/api/workinggroups/options'))
        .pipe(
            take(1),
            map((jsItems: any) => {
              return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        );// TODO: error handling
    }

    getWorkingGroup(id: number) {
      return this.getJson(this.buildResourseUrl(`/api/workinggroups/${id}`, null, true, true))
      .pipe(
          take(1),
          map((jsItems: any) => {
            let model = new WorkingGroupInputModel();
            model.id = jsItems.id;
            model.changedDate = new Date(jsItems.changedDate);
            model.createdDate = new Date(jsItems.createdDate)
            model.code = jsItems.code;
            model.customerId = jsItems.customerId;
            model.isActive = jsItems.isActive;
            model.isDefault = jsItems.isDefault;
            model.stateSecondaryId = jsItems.stateSecondaryId;
            model.workingGroupName = jsItems.workingGroupName;
            return model;
          })
      );// TODO: error handling
  }
}