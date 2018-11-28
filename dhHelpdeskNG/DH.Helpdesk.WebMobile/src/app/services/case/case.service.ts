import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { map, defaultIfEmpty, take, tap, switchMap } from 'rxjs/operators';
import { CaseEditInputModel, CaseOptionsFilterModel, BundleOptionsFilter, CaseSectionInputModel, BaseCaseField, KeyValue, MailToTicketInfo, CaseEditMode, CaseSolution, CaseLockModel } from '../../models';
import { throwError, forkJoin, empty, Observable, of } from 'rxjs';
import { CaseOptions } from '../../models/case/case-options.model';
import { CaseOrganizationService } from '../case-organization';
import { BundleCaseOptionsService } from '../case-organization/bundle-case-options.service';

@Injectable({ providedIn: 'root' })
export class CaseService extends HttpApiServiceBase {

    protected constructor(http: HttpClient, localStorageService: LocalStorageService,
         private _caseOrganizationService: CaseOrganizationService,
         private _batchCaseOptionsService: BundleCaseOptionsService ) {
        super(http, localStorageService);
    }

    getCaseData(caseId: number): Observable<CaseEditInputModel> {
      const userData = this.localStorageService.getCurrentUser();
      let params = null;
      if (userData !== null) {
        params = { sessionId: userData.authData.sessionId };
      }
      let url = this.buildResourseUrl('/api/case/' + caseId, params, true, true);
      return this.getJson(url)// TODO: error handling
          .pipe(
              take(1),
              switchMap((caseData: any) => {
                  let model = this.fromJSONCaseEditInputModel(caseData);
                  return of(model);
              }) 
          );
    }

    getCaseOptions(filter: CaseOptionsFilterModel) {
        const empty$ = () => empty().pipe(defaultIfEmpty(null));
        const fieldExists = (field: any) => field !== undefined;

        let regions$ = this._caseOrganizationService.getRegions();
        let departments$ = fieldExists(filter.RegionId) ? this._caseOrganizationService.getDepartments(filter.RegionId) : empty$();
        let oUs$ = fieldExists(filter.DepartmentId) && filter.DepartmentId != null ? this._caseOrganizationService.getOUs(filter.DepartmentId): empty$();
        let isAboutDepartments$ =  fieldExists(filter.IsAboutRegionId) ? this._caseOrganizationService.getDepartments(filter.IsAboutRegionId) : empty$();
        let isAboutOUs$ = fieldExists(filter.IsAboutDepartmentId) && filter.IsAboutDepartmentId != null ? this._caseOrganizationService.getOUs(filter.IsAboutDepartmentId) : empty$();
        let caseTypes$ = fieldExists(filter.CaseTypes) ? this._caseOrganizationService.getCaseTypes() : empty$();
        let productAreas$ = fieldExists(filter.ProductAreas) ? this._caseOrganizationService.getProductAreas(filter.CaseTypeId, filter.ProductAreaId) : empty$();
        let categories$ = fieldExists(filter.Categories) ? this._caseOrganizationService.getCategories() : empty$();
        let closingReasons$ = fieldExists(filter.ClosingReasons) ? this._caseOrganizationService.getClosingReasons() : empty$();
 
        let bundledOptions$ = this._batchCaseOptionsService.getOptions(filter as BundleOptionsFilter);

        return forkJoin(bundledOptions$, regions$, departments$, oUs$, isAboutDepartments$, isAboutOUs$, caseTypes$, productAreas$, categories$, closingReasons$)
                    .pipe(
                        take(1),
                        switchMap(([bundledOptions, regions, departments, oUs, isAboutDepartments, isAboutOUs, caseTypes, productAreas, categories, closingReasons]) => {
                            let options = new CaseOptions();
                            
                            if (regions != null) {
                                options.regions = regions;
                            }

                            if (departments != null) {
                                options.departments = departments;
                            }

                            if (oUs != null) {
                                options.oUs = oUs;
                            }

                            if (isAboutDepartments != null) {
                                options.isAboutDepartments = isAboutDepartments;
                            }

                            if (isAboutOUs != null) {
                                options.isAboutOUs = isAboutOUs;
                            }

                            Object.assign(options, bundledOptions);

                            if (bundledOptions != null) {
                                Object.assign(options, bundledOptions);
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

                            return of(options);
                    }));
    }

    getCaseSections() {        
        return this.getJson(this.buildResourseUrl('/api/casesections/get', null, true, true)) // TODO: error handling
            .pipe(
                take(1),
                map((jsCaseSections: any) => {
                    if (!jsCaseSections) throwError("No data from server.");

                    let sections = (jsCaseSections as Array<any>).map((jsSection: any) => {
                        return new CaseSectionInputModel(jsSection.id, jsSection.sectionHeader,
                             jsSection.sectionType, jsSection.isNewCollapsed,
                             jsSection.isEditCollapsed);
                    });
                    return sections;
                }) 
            )
    }

    buildCaseFileUrl(caseId: number, fileId: number): string {
        let url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, null, true, false)
        return url;
    }

    // TODO: review - not all cases covered
    private fromJSONCaseEditInputModel(json: any): CaseEditInputModel {
        if (typeof json === 'string') {
             json = JSON.parse(json);
        } 

        let fields = json.fields as any[] || new Array();
        let caseSolution = json.caseSolution ? <CaseSolution>json.caseSolution : null;
        let mailToTickets:MailToTicketInfo = json.mailToTickets ? <MailToTicketInfo>json.mailToTickets : null;      
        let editMode = <CaseEditMode>json.editMode;

        return Object.assign(new CaseEditInputModel(), json, {
            editMode: editMode,
            caseSolution: caseSolution,
            mailToTickets: mailToTickets,           
            fields: fields.map(v => {
                let field = null;
                switch (v.JsonType) {
                    case "string":
                        field = this.fromJSONBaseCaseField<string>(v);
                        break;
                    case "date":
                        field = this.fromJSONBaseCaseField<string>(v);// TODO: As date
                        break;
                    case "number":
                        field = this.fromJSONBaseCaseField<number>(v);
                        break;
                    case "array":
                        field = this.fromJSONBaseCaseField<Array<any>>(v);
                        break;
                    default:
                        field = this.fromJSONBaseCaseField<any>(v)
                        break;
                }
                return field;
            })
        });
    }

    private fromJSONBaseCaseField<T>(json: any): BaseCaseField<T> {
        if (typeof json === 'string') {
            json = JSON.parse(json); 
        }
        var options = json.options as any[] || new Array();
        return Object.assign(new BaseCaseField<T>(), json, {
            value: json.value,
            options: options.map(v => this.fromJSONKeyValue(v))
        });
    }

    private fromJSONKeyValue(json: any): KeyValue {
        if (typeof json === 'string') { json = JSON.parse(json); }
        return Object.assign(new KeyValue(), json, {})
    }
}