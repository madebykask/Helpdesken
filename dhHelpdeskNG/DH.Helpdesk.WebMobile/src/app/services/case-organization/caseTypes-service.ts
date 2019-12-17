import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { map, take } from 'rxjs/operators';
import { MultiLevelOptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { CaseTypeInputModel } from 'src/app/models/caseTypes/caseTypeInput.model';

@Injectable({ providedIn: 'root' })
export class CaseTypesService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
            super(http, localStorageService);
    }

    getCaseTypes(customerId?: number) {
      const params = isNaN(customerId) ? {} : {cid: customerId};
        return this.getJson(this.buildResourseUrl('/api/casetypes/options', params, isNaN(params.cid), true))
        .pipe(
            take(1),
            map((jsItems: any) => {
                let result = new Array<MultiLevelOptionItem>();
                const jsArr = (jsItems as Array<any>);
                if (jsArr == null) { return result; }

                const createOption = (jsItem: any): MultiLevelOptionItem => { // TODO: stop condition
                    const option = new MultiLevelOptionItem(jsItem.id, jsItem.name, jsItem.parentId);
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

    getCaseType(id: number, customerId: number) {
      return this.getJson(this.buildResourseUrl(`/api/casetypes/${id}` , { cid: customerId }, false, true))
        .pipe(
            take(1),
            map((jsItem: any) => {
              const model = new CaseTypeInputModel();
              model.id = jsItem.id;
              model.parentId = jsItem.parentId;
              model.workingGroupId = jsItem.workingGroupId;
              model.performerUserId = jsItem.administratorId;
              return model;
            })
        ); // TODO: error handling
    }
}
