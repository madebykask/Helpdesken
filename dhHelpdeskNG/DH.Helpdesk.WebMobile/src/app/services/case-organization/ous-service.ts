import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../helpers/options-helper';
import { map, take } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { OrganizationalUnitInputModel } from 'src/app/models/organizationalUnits/organizationalUnitInputModel';

@Injectable({ providedIn: 'root' })
export class OUsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getOUsByDepartment(departmentId: number, customerId?: number) {
      const params = departmentId != null ? { departmentId: departmentId, cid: isNaN(customerId) ? null : customerId } : {};
        return this.getJson(this.buildResourseUrl('/api/organizationalunits/options', params, isNaN(params.cid)))
        .pipe(
            take(1),
            map((jsItems: any) => {
                return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        ); // TODO: error handling
    }

    getOU(id: number, customerId: number) {
      return this.getJson(this.buildResourseUrl(`/api/organizationalunits/${id}` , { cid: customerId}, false, true))
        .pipe(
            take(1),
            map((jsItem: any) => {
              const model = new OrganizationalUnitInputModel();
              model.id = jsItem.id;
              model.parentId = jsItem.parentId;
              model.name = jsItem.name;
              return model;
            })
        ); // TODO: error handling
    }
}
