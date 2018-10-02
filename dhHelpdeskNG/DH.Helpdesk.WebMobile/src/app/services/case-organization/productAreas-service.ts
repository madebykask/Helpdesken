import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { CaseHelper } from "./case-helper";
import { map } from "rxjs/operators";
import { MultiLevelOptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class ProductAreasService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: CaseHelper) {
            super(http, localStorageService);
    }

    getProductAreas(caseTypeId?: number, includeId?: number) {
        let params = { };
        if (caseTypeId != null) Object.assign(params, { caseTypeId: caseTypeId });
        if (includeId != null) Object.assign(params, { includeId: includeId });
        return this.getJson(this.buildResourseUrl('/api/productareas/getbycasetype', params))
        .pipe(
            map((jsItems: any) => {
                let result = new Array<MultiLevelOptionItem>();
                let jsArr = (jsItems as Array<any>);
                if (jsArr == null) return result;

                const createOption = (jsItem: any): MultiLevelOptionItem => { //TODO: stop condition
                    let option = new MultiLevelOptionItem(jsItem.id, jsItem.name, jsItem.parentId);
                    if (jsItem.subProductAreas != null) {
                        option.childs = (jsItem.subProductAreas as Array<any>).map(createOption);
                    }
                    return option;
                };

                result = jsArr.map(createOption);

                return result;
            })
        );//TODO: error handling
    }
}