import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map, take } from "rxjs/operators";
import { OptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class PerfomersService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getPerformers(performerUserId?: number, workingGroupId?: number) {
        let params = { };
        if (performerUserId != null) Object.assign(params, { performerUserId: performerUserId });
        if (workingGroupId != null) Object.assign(params, { workingGroupId: workingGroupId });
        return this.getJson(this.buildResourseUrl('/api/perfomers/get', params, true, true))
            .pipe(
                take(1),
                map((jsItems: any) => {
                  return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
                })
            );// TODO: error handling
    }
}