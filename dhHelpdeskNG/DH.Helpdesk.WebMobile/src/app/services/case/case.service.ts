import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../localStorage'
import { HttpApiServiceBase } from '../api'
import { map } from 'rxjs/operators';
import { CaseEditInputModel } from '../../models';

@Injectable({ providedIn: 'root' })
export class CaseService extends HttpApiServiceBase {
    
    protected constructor(http: HttpClient, private localStorageService: LocalStorageService){
        super(http);
    }

    getCaseData(caseId: number) {
        var user = this.localStorageService.getCurrentUser();
        return this.getJson(this.buildResourseUrl('/api/case/get', { caseId: caseId, customerId: user.currentData.selectedCustomerId}))//TODO: error handling
            .pipe(
                map((caseData: any) => {
                    let model = new CaseEditInputModel();
                    model.Id = Number(caseData.Id);
                    model.CaseNumber = caseData.CaseNumber;
                    return model;
                }) 
            )
    }
}