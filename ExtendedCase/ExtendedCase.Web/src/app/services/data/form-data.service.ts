import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { AppConfig } from '../../shared/app-config/app-config';
import { FormDataSaveModel, FormDataSaveResult } from '../../models/form-data.model';
import { ExtendedHttpService} from './extended-http.service'
import { IAppConfig } from 'src/app/shared/app-config/app-config.interface';

@Injectable()
export class FormDataService {
    constructor(
        @Inject(AppConfig) private config: IAppConfig,
        private httpService: ExtendedHttpService) {
    }

    getFormDataById(uniqueId: string, helpdeskCaseId:number, authToken:string): Observable<any> {
        let url = `${this.config.apiHost}/api/Forms/${uniqueId}/Data/${helpdeskCaseId}`;
        return this.httpService.getJson<any>(url, { helpdeskAuthToken: authToken });
    }

    saveFormData(saveData: FormDataSaveModel, authToken:string): Observable<FormDataSaveResult> {
        let url = `${this.config.apiHost}/api/Forms/Data`;
        return this.httpService.postJson<FormDataSaveResult>(url, saveData, { helpdeskAuthToken: authToken });
    }
}

