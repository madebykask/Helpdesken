import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { map } from 'rxjs/operators';
import { CaseEditInputModel } from '../../models';

@Injectable({ providedIn: 'root' })
export class CaseService extends HttpApiServiceBase {
    
    protected constructor(http: HttpClient, localStorageService: LocalStorageService){
        super(http, localStorageService);
    }

    getCaseData(caseId: number) {
        var user = this.localStorageService.getCurrentUser();
        return this.getJson(this.buildResourseUrl('/api/case/get',
                             { caseId: caseId, languageId: user.currentData.selectedLanguageId }))//TODO: error handling
            .pipe(
                map((caseData: any) => {
                    let model = CaseEditInputModel.fromJSON(caseData);
                    return model;
                }) 
            )
    }
}