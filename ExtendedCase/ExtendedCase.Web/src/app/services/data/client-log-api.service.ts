import { Injectable, Inject, forwardRef } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { AppConfig } from '../../shared/app-config/app-config';
import { ExtendedHttpService} from './extended-http.service'
import { ClientLogEntryModel } from '../../models/client-log.model';
import { IAppConfig } from 'src/app/shared/app-config/app-config.interface';

@Injectable()
export class ClientLogApiService {
    constructor(
        @Inject(forwardRef(() => AppConfig)) private config: IAppConfig,
        private httpService: ExtendedHttpService) {
    }

    saveLogEntry(logEntry: ClientLogEntryModel): Observable<any> {
        let url = `${this.config.apiHost}/api/ClientLog`;
        return this.httpService.postJsonNoContent(url, logEntry);
    }
}



