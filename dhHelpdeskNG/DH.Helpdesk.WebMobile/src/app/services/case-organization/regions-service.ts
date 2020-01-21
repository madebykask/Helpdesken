import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../helpers/options-helper';
import { map, take } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';

@Injectable({ providedIn: 'root' })
export class RegionsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getRegions(customerId?: number) {
        const params = isNaN(customerId) ? {} : {cid: customerId};
        return this.getJson(this.buildResourseUrl('/api/regions/options', params, isNaN(params.cid)))
        .pipe(
            take(1),
            map((jsItems: any) => {
                return this.caseHelper.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        ); //TODO: error handling
    }
}
