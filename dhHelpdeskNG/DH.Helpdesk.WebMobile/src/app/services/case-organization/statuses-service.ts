import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../helpers/options-helper';
import { map, take } from 'rxjs/operators';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { Observable } from 'rxjs';
import { StatusInputModel } from 'src/app/models/statuses/statusInput.model';

@Injectable({ providedIn: 'root' })
export class StatusesService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
    private caseHelper: OptionsHelper) {
        super(http, localStorageService);
  }

  getStatus(id: number, customerId: number): Observable<StatusInputModel> {
    return this.getJson(this.buildResourseUrl(`/api/statuses/${id}`, { cid: customerId }, false, true))
    .pipe(
        take(1),
        map((jsItems: any) => {
          const model = new StatusInputModel();
          model.id = jsItems.id;
          model.customerId = jsItems.customerId;
          model.isActive = jsItems.isActive;
          model.isDefault = jsItems.isDefault;
          model.name = jsItems.name;
          model.workingGroupId = jsItems.workingGroupId;
          model.stateSecondaryId = jsItems.stateSecondaryId;
          return model;
        })
    ); // TODO: error handling
  }
}
