import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { map, mergeMap, mapTo, defaultIfEmpty } from 'rxjs/operators';
import { CaseEditInputModel, CaseOptionsFilterModel, BundleOptionsFilter } from '../../models';
import { throwError, forkJoin, Observable, empty, zip } from 'rxjs';
import { OptionItem, CaseOptions } from '../../models/case/case-options.model';
import { CaseOrganizationService } from '../case-organization';
import { BundleCaseOptionsService } from '../case-organization/bundle-case-options.service';

@Injectable({ providedIn: 'root' })
export class CaseService extends HttpApiServiceBase {
   
    protected constructor(http: HttpClient, localStorageService: LocalStorageService,
         private _caseOrganizationService: CaseOrganizationService,
         private _batchCaseOptionsService: BundleCaseOptionsService ){
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
        const $empty = () => empty().pipe(defaultIfEmpty(null));
        const fieldExists = (field: any) => field !== undefined;

        let $regions = this._caseOrganizationService.getRegions();
        let $departments = fieldExists(filter.RegionId) ? this._caseOrganizationService.getDepartments(filter.RegionId) : $empty();
        let $oUs = fieldExists(filter.DepartmentId) ? this._caseOrganizationService.getOUs(filter.DepartmentId): $empty();
        let $isAboutDepartments =  fieldExists(filter.IsAboutRegionId) ? this._caseOrganizationService.getDepartments(filter.IsAboutRegionId) : $empty();
        let $isAboutOUs = fieldExists(filter.IsAboutDepartmentId) ? this._caseOrganizationService.getOUs(filter.IsAboutDepartmentId) : $empty();
        let $caseTypes = fieldExists(filter.CaseTypes) ? this._caseOrganizationService.getCaseTypes() : $empty();
        let $productAreas = fieldExists(filter.ProductAreas) ? this._caseOrganizationService.getProductAreas(filter.CaseTypeId, filter.ProductAreaId) : $empty();
        let $categories = fieldExists(filter.Categories) ? this._caseOrganizationService.getCategories() : $empty();
        let $closingReasons = $empty(); //fieldExists(filter.ClosingReasons) ? this._caseOrganizationService.getClosingReasons() : $empty();
 
        let $bundledOptions = this._batchCaseOptionsService.getOptions(filter as BundleOptionsFilter);

        return forkJoin($regions, $departments, $oUs, $isAboutDepartments, $isAboutOUs, $bundledOptions, $caseTypes, $productAreas, $categories, $closingReasons)
                    .pipe(
                        map(([regions, departments, oUs, isAboutDepartments, isAboutOUs, bundledOptions, caseTypes, productAreas, categories, closingReasons]) => {
                            let options = new CaseOptions();
                            if(regions != null) {                                                
                                options.regions = regions;
                            }
                            if(departments != null) {                                                
                                options.departments = departments;
                            }
                            if(oUs != null) {
                                options.oUs = oUs;
                            }
                            if(isAboutDepartments != null) {                                                
                                options.isAboutDepartments = isAboutDepartments;
                            }
                            if(isAboutOUs != null) {
                                options.isAboutOUs = isAboutOUs;
                            }

                            Object.assign(options, bundledOptions);

                            if(bundledOptions != null) {
                                Object.assign(options, bundledOptions);
/*                                 if(bundledOptions.customerRegistrationSources != null) {
                                    options.customerRegistrationSources = bundledOptions.customerRegistrationSources;
                                }   
                                if(bundledOptions.systems != null) {
                                    options.systems = bundledOptions.systems;
                                }  
                                if(bundledOptions.urgencies != null) {
                                    options.urgencies = bundledOptions.urgencies;
                                }  
                                if(bundledOptions.impacts != null) {
                                    options.impacts = bundledOptions.impacts;
                                }  
                                if(bundledOptions.suppliers != null) {
                                    options.suppliers = bundledOptions.suppliers;
                                }  
                                if(bundledOptions.countries != null) {
                                    options.countries = bundledOptions.countries;
                                }  
                                if(bundledOptions.currencies != null) {
                                    options.currencies = bundledOptions.currencies;
                                }   */
                            }

                            if (caseTypes != null) {
                                options.caseTypes = caseTypes;
                            }
                            if (productAreas != null) {
                                options.productAreas = productAreas;
                            }
                            if (categories != null) {
                                options.categories = categories;
                            }
                            if (closingReasons != null) {
                                options.closingReasons = closingReasons;
                            }                             
                            return options;
                    }));
/*         return this.postJson(this.buildResourseUrl('/api/caseoptions/bundle'), filter)//TODO: error handling
            .pipe(
                map((jsOptions: any) => {
                    if(jsOptions == null) throwError('No options received.')
                    let options = new CaseOptions();
                    const mapArray = (arr: any) => (arr as Array<any>).map(jsItem => new OptionItem(jsItem.name, jsItem.value));
                    
                    if(jsOptions.regions != null) {                                                
                        options.regions = mapArray(jsOptions.regions);
                    }
                    if(jsOptions.departments != null) {
                        options.departments = mapArray(jsOptions.departments);
                    }
                    if(jsOptions.oUs != null) {
                        options.oUs = mapArray(jsOptions.oUs);
                    }
                    if(jsOptions.isAboutDepartments != null) {
                        options.isAboutDepartments = mapArray(jsOptions.isAboutDepartments);
                    }
                    if(jsOptions.isAboutOUs != null) {
                        options.isAboutOUs = mapArray(jsOptions.isAboutOUs);
                    }
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

                    return options;
                })  
            )*/
    }
}