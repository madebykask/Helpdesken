import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { map } from 'rxjs/operators';
import { CaseEditInputModel, CaseOptionsFilterModel } from '../../models';
import { throwError } from 'rxjs';
import { OptionItem, CaseOptions } from '../../models/case/case-options.model';

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

    getCaseOptions(filter: CaseOptionsFilterModel) {
        return this.postJson(this.buildResourseUrl('/api/caseoptions/bundle'), filter)//TODO: error handling
            .pipe(
                map((jsOptions: any) => {
                    if(jsOptions == null) throwError('No options received.')
                    let options = new CaseOptions();
                    const mapArray = (arr: any) => (arr as Array<any>).map(item => new OptionItem(item.Name, item.Value));
                    
                    if(jsOptions.Regions != null) {                                                
                        options.regions = mapArray(jsOptions.Regions);
                    }
                    if(jsOptions.Departments != null) {
                        options.departments = mapArray(jsOptions.Departments);
                    }
                    if(jsOptions.OUs != null) {
                        options.oUs = mapArray(jsOptions.OUs);
                    }
                    if(jsOptions.IsAboutDepartments != null) {
                        options.isAboutDepartments = mapArray(jsOptions.IsAboutDepartments);
                    }
                    if(jsOptions.IsAboutOUs != null) {
                        options.isAboutOUs = mapArray(jsOptions.IsAboutOUs);
                    }
                    if(jsOptions.CustomerRegistrationSources != null) {
                        options.customerRegistrationSources = mapArray(jsOptions.CustomerRegistrationSources);
                    }   
                    if(jsOptions.Systems != null) {
                        options.systems = mapArray(jsOptions.Systems);
                    }  
                    if(jsOptions.Urgencies != null) {
                        options.urgencies = mapArray(jsOptions.Urgencies);
                    }  
                    if(jsOptions.Impacts != null) {
                        options.impacts = mapArray(jsOptions.Impacts);
                    }  
                    if(jsOptions.Suppliers != null) {
                        options.suppliers = mapArray(jsOptions.Suppliers);
                    }  
                    if(jsOptions.Countries != null) {
                        options.countries = mapArray(jsOptions.Countries);
                    }  
                    if(jsOptions.Currencies != null) {
                        options.currencies = mapArray(jsOptions.Currencies);
                    }  

                    return options;
                }) 
            )
    }
}