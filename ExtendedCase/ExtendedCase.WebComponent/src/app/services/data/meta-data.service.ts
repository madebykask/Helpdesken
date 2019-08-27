import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfig } from '../../shared/app-config/app-config';
import { ExtendedHttpService } from './extended-http.service'
import { FormMetaDataResponse, FormListItem } from '../../models/form-data.model'
import { FormAssignmentParameters } from '../../models/form-parameters.model';
import { IAppConfig } from 'src/app/shared/app-config/app-config.interface';

@Injectable()
export class FormDataService {
    constructor(private httpService: ExtendedHttpService,
                @Inject(AppConfig) private config: IAppConfig) {
    }

    getFormDataById(id: string): Observable<any> {
        let url = `${this.config.apiHost}/api/Forms/${id}/Data`;
        return this.httpService.getJson<any>(url);
    }
}

@Injectable()
export class MetaDataService {

    constructor(private httpService: ExtendedHttpService,
                @Inject(AppConfig) private config: IAppConfig) {
    }

    getMetaDataById(formId: number, languageId?: number): Observable<FormMetaDataResponse> {

        let url = `${this.config.apiHost}/api/Forms/${formId}/MetaData/` + (languageId || '');
        return this.httpService.getJson<FormMetaDataResponse>(url);
    }

    getMetaDataByAssignment(parameters: FormAssignmentParameters, languageId?: number): Observable<FormMetaDataResponse> {
        let url = `${this.config.apiHost}/api/Forms/ByAssignment/MetaData?`;
        let props: string[] = [];

        for (let p of Object.getOwnPropertyNames(parameters)) {
            props.push(`${encodeURIComponent(p)}=${encodeURIComponent(parameters[p])}`);
        }
        if (languageId) {
            props.push(`${encodeURIComponent('languageId')}=${encodeURIComponent(languageId.toString())}`);
        }
        url += props.join('&');
        return this.httpService.getJson<FormMetaDataResponse>(url);
    }

    getFormsList(): Observable<Array<FormListItem>> {
        const url = `${this.config.apiHost}/api/Forms/List`;
        return this.httpService.getJson<Array<FormListItem>>(url);
    }
}