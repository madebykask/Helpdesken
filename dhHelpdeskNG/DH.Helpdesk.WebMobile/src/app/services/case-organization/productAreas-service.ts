import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { map, take } from "rxjs/operators";
import { MultiLevelOptionItem } from "src/app/modules/shared-module/models";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";
import { ProductAreaInputModel } from "src/app/models/productAreas/productAreaInput.model";

@Injectable({ providedIn: 'root' })
export class ProductAreasService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
            super(http, localStorageService);
    }

    getProductAreas(caseTypeId?: number, includeId?: number) {
        let params = { };
        if (caseTypeId != null) { Object.assign(params, { caseTypeId: caseTypeId }); }
        if (includeId != null) { Object.assign(params, { includeId: includeId }); }
        return this.getJson(this.buildResourseUrl('/api/productareas/options', params, true, true))
            .pipe(
                take(1),
                map((jsItems: any) => {
                    let result = new Array<MultiLevelOptionItem>();
                    let jsArr = (jsItems as Array<any>);
                    if (jsArr == null) { return result; }

                    const createOption = (jsItem: any): MultiLevelOptionItem => { // TODO: stop condition
                        let option = new MultiLevelOptionItem(jsItem.id, jsItem.name, jsItem.parentId);
                        if (jsItem.subProductAreas != null) {
                            option.childs = (jsItem.subProductAreas as Array<any>).map(createOption);
                        }
                        return option;
                    };

                    result = jsArr.map(createOption);

                    return result;
                })
            );// TODO: error handling
    }

    getProductArea(id: number) {
      return this.getJson(this.buildResourseUrl(`/api/productareas/${id}` , null, true, true))
      .pipe(
          take(1),
          map((jsItem: any) => {
            let model = new ProductAreaInputModel();
            model.id = jsItem.id;
            model.parentId = jsItem.parentId;
            model.workingGroupId = jsItem.workingGroupId;
            model.priorityId = jsItem.priorityId;
            return model;
          })
      );
    }
}
