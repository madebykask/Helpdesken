import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map } from "rxjs/operators";
import { MultiLevelOptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class CaseTypesService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getCaseTypes() {
        return this.getJson(this.buildResourseUrl('/api/casetypes/get'))
        .pipe(
            map((jsItems: any) => {
                let result = new Array<MultiLevelOptionItem>();
                let jsArr = (jsItems as Array<any>);
                if (jsArr == null) return result;

                const createOption = (jsItem: any): MultiLevelOptionItem => { //TODO: stop condition
                    let option = new MultiLevelOptionItem(jsItem.id, jsItem.name, jsItem.parentId);
                    if (jsItem.subCaseTypes != null) {
                        option.childs = (jsItem.subCaseTypes as Array<any>).map(createOption);
                    }
                    return option;
                };

                result = jsArr.map(createOption);

                return result;
            })
        );//TODO: error handling
    }
}