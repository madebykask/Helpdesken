import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map } from "rxjs/operators";
import { OptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class OUsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getOUsByDepartment(departmentId: number) {
        return this.getJson(this.buildResourseUrl('/api/organizationalunits/get', { departmentId: departmentId }))
        .pipe(
            map((jsItems: any) => {
                return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        );//TODO: error handling
    }
}