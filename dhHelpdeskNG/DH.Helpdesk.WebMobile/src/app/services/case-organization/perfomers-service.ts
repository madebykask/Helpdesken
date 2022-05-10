import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../helpers/options-helper';
import { map, switchMap, take } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PerfomersService extends HttpApiServiceBase {
    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getPerformers(performerUserId?: number, workingGroupId?: number, customerId?: number) {
      const params = customerId != null ? { cid: customerId } : {};
        if (performerUserId != null) { Object.assign(params, { performerUserId: performerUserId }); }
        if (workingGroupId != null) { Object.assign(params, { workingGroupId: workingGroupId }); }
        return this.getJson(this.buildResourseUrl('/api/perfomers/options', params, false, true))
            .pipe(
                take(1),
                map((jsItems: any) => {
                  return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
                })
            ); // TODO: error handling
    }

    getPerformerEmail(performerUserId?: number) : Observable<any> {
      const params = performerUserId != null ? {id: performerUserId} : {};
      return this.getJson<string>(this.buildResourseUrl('/api/perfomers/GetEmailById', params, false, false))
      .pipe(
          take(1),
          map((jsItem: any) => {
            return jsItem;
          })
      ); // TODO: error handling  
    }
}   
