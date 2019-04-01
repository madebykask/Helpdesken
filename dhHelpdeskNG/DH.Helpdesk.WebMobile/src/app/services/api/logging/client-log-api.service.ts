import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../local-storage';
import { HttpApiServiceBase } from '../../../modules/shared-module/services/api/httpServiceBase';
import { ClientLogEntryModel } from '../../../models/shared/client-log.model';
import { Observable } from 'rxjs/Rx';

@Injectable({ providedIn: 'root' })
export class ClientLogApiService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  saveLogEntry(logEntry: ClientLogEntryModel): Observable<any> {
    let url = this.buildResourseUrl('/api/ClientLog', null, false, false);
    return this.postJsonNoContent(url, logEntry, null, true);
  }
}



