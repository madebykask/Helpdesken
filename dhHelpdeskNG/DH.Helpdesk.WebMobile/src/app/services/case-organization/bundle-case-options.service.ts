import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { OptionsHelper } from "../../helpers/options-helper";
import { map } from "rxjs/operators";
import { throwError } from "rxjs";
import { BundleOptionsFilter, BundledCaseOptions, OptionItem } from "../../models";

@Injectable({ providedIn: 'root' })
export class BundleCaseOptionsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getOptions(filter: BundleOptionsFilter) {
        return this.postJson(this.buildResourseUrl('/api/caseoptions/bundle'), filter)
        .pipe(
            map((jsOptions: any) => {
                if(jsOptions == null) throwError('No options received.')
                const mapArray = (arr: any) => this.caseHelper.toOptionItems(arr as Array<any>) || new Array<OptionItem>();

                let options = new BundledCaseOptions();
                if(jsOptions.customerRegistrationSources != null) {
                    options.customerRegistrationSources = mapArray(jsOptions.customerRegistrationSources);
                }   
                if(jsOptions.systems != null) {
                    options.systems = mapArray(jsOptions.systems);
                }  
                if(jsOptions.urgencies != null) {
                    options.urgencies = mapArray(jsOptions.urgencies);
                }  
                if(jsOptions.impacts != null) {
                    options.impacts = mapArray(jsOptions.impacts);
                }  
                if(jsOptions.suppliers != null) {
                    options.suppliers = mapArray(jsOptions.suppliers);
                }  
                if(jsOptions.countries != null) {
                    options.countries = mapArray(jsOptions.countries);
                }  
                if(jsOptions.currencies != null) {
                    options.currencies = mapArray(jsOptions.currencies);
                }
                if(jsOptions.workingGroups != null) {
                    options.workingGroups = mapArray(jsOptions.workingGroups);
                }  
                if(jsOptions.responsibleUsers != null) {
                    options.responsibleUsers = mapArray(jsOptions.responsibleUsers);
                }  
                if(jsOptions.performers != null) {
                    options.performers = mapArray(jsOptions.performers);
                }  
                if(jsOptions.priorities != null) {
                    options.priorities = mapArray(jsOptions.priorities);
                }  
                if(jsOptions.statuses != null) {
                    options.statuses = mapArray(jsOptions.statuses);
                }  
                if(jsOptions.stateSecondaries != null) {
                    options.stateSecondaries = mapArray(jsOptions.stateSecondaries);
                }  
                if(jsOptions.projects != null) {
                    options.projects = mapArray(jsOptions.projects);
                }  
                if(jsOptions.problems != null) {
                    options.problems = mapArray(jsOptions.problems);
                }    
                if(jsOptions.changes != null) {
                    options.changes = mapArray(jsOptions.changes);
                }    
                if(jsOptions.solutionsRates != null) {
                    options.solutionsRates = mapArray(jsOptions.solutionsRates);
                }    
/*                 if(jsOptions.causingParts != null) {
                    options.causingParts = mapArray(jsOptions.causingParts);
                }  */

                return options;
            }));//TODO: error handling
    }
}