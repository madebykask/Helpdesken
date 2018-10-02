import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { CaseHelper } from "./case-helper";
import { map } from "rxjs/operators";
import { OptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class DepartmentsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: CaseHelper) {
            super(http, localStorageService);
    }

    getDepartmentsByRegion(regionId?: number) {
        let params = { };
        if (regionId != null) params = { regionId: regionId };
        return this.getJson(this.buildResourseUrl('/api/departments/getbyregion', params))
        .pipe(
            map((jsItems: any) => {
                return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        );//TODO: error handling
    }
}