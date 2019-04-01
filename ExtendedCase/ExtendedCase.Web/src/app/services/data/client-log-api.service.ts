import { Injectable, Inject, forwardRef } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { IAppConfig, AppConfig } from '../../shared/app-config/app-config';
import { ExtendedHttpService} from './extended-http.service'
import { ClientLogEntryModel } from '../../models/client-log.model';

@Injectable()
export class ClientLogApiService {
    constructor(
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig,
        private httpService: ExtendedHttpService) {
    }

    saveLogEntry(logEntry: ClientLogEntryModel) : Observable<any> {
        let url = `${this.config.apiHost}/api/ClientLog`;
        return this.httpService.postJsonNoContent(url, logEntry);
    }
}



