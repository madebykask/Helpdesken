import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { map, take } from "rxjs/operators";
import { MultiLevelOptionItem } from "src/app/modules/shared-module/models";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";
import { CaseTypeInputModel } from "src/app/models/caseTypes/caseTypeInput.model";

@Injectable({ providedIn: 'root' })
export class CaseTypesService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
            super(http, localStorageService);
    }

    getCaseTypes() {
        return this.getJson(this.buildResourseUrl('/api/casetypes/options', null, true, true))
        .pipe(
            take(1),
            map((jsItems: any) => {
                let result = new Array<MultiLevelOptionItem>();
                let jsArr = (jsItems as Array<any>);
                if (jsArr == null) { return result; }

                const createOption = (jsItem: any): MultiLevelOptionItem => { // TODO: stop condition
                    let option = new MultiLevelOptionItem(jsItem.id, jsItem.name, jsItem.parentId);
                    if (jsItem.subCaseTypes != null) {
                        option.childs = (jsItem.subCaseTypes as Array<any>).map(createOption);
                    }
                    return option;
                };

                result = jsArr.map(createOption);

                return result;
            })
        ); // TODO: error handling
    }

    getCaseType(id: number) {
      return this.getJson(this.buildResourseUrl(`/api/casetypes/${id}` , null, true, true))
        .pipe(
            take(1),
            map((jsItem: any) => {
              let model = new CaseTypeInputModel();
              model.id = jsItem.id;
              model.parentId = jsItem.parentId;
              model.workingGroupId = jsItem.workingGroupId;
              model.performerUserId = jsItem.administratorId;
              return model;
            })
        );// TODO: error handling
    }
}