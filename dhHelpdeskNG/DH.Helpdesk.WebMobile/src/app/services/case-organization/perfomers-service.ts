import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../helpers/options-helper';
import { map, switchMap, take } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { Observable, throwError } from 'rxjs';
import { EmailEventData } from '../communication/data/email-event-data';
import { Channels, CommunicationService } from '../communication';
import { delay } from 'rxjs-compat/operator/delay';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Injectable({ providedIn: 'root' })
export class PerfomersService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
    private commService: CommunicationService,
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

  getPerformerEmail(performerUserId?: string): Promise<EmailEventData> {
    
    const errorMsg = `Incorrect performeruserid: ${performerUserId}`;
    if (isNaN(parseInt(performerUserId))) {
      return throwError(errorMsg).toPromise();
    }
    else {
      if (parseInt(performerUserId) < 1) {
        return throwError(errorMsg).toPromise();
      }
    }
    let performerUserIdInt = parseInt(performerUserId);
    const params = { id: performerUserIdInt };
    return this.getJson<string>(this.buildResourseUrl('/api/perfomers/GetEmailById', params, false, false))
      .pipe(
        take(1),
        map((jsItem: any) => {
          return jsItem;
        })
      ).toPromise(); // TODO: error handling  
  }

  handlePerformerEmail(performerUserId) {
    this.getPerformerEmail(performerUserId)
      .then((x) => {
        this.commService.publish(Channels.CaseFieldValueChanged, new EmailEventData(x.eMail));
      }).catch(() => {
        this.commService.publish(Channels.CaseFieldValueChanged, new EmailEventData(''));
      });
  }
}


