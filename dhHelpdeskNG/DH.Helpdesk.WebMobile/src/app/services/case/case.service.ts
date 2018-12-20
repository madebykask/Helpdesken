import { Injectable } from '@angular/core';
import { defaultIfEmpty, take, switchMap, catchError } from 'rxjs/operators';
import { CaseEditInputModel, CaseOptionsFilterModel, BundleOptionsFilter, CaseSectionInputModel, BaseCaseField, KeyValue, MailToTicketInfo, CaseAccessMode, CaseSolution } from '../../models';
import { throwError, forkJoin, empty, Observable, of } from 'rxjs';
import { CaseOptions } from '../../models/case/case-options.model';
import { CaseOrganizationService } from '../case-organization';
import { BundleCaseOptionsService } from '../case-organization/bundle-case-options.service';
import { CaseApiService } from '../api/case/case-api.service';

@Injectable({ providedIn: 'root' })
export class CaseService {

    protected constructor(private _caseOrganizationService: CaseOrganizationService,
         private _batchCaseOptionsService: BundleCaseOptionsService,
         private _caseApiService: CaseApiService ) {
    }

    getCaseData(caseId: number): Observable<CaseEditInputModel> {
      return this._caseApiService.getCaseData(caseId)
        .pipe(
          switchMap((caseData: any) => {
            let model = this.fromJSONCaseEditInputModel(caseData);
            return of(model);
        }))
    }

    getOptionsHelper(filter: CaseOptionsFilterModel) {
      const empty$ = () => empty().pipe(defaultIfEmpty(null));
      const fieldExists = (field: any) => field !== undefined;

      return {
        getRegions: () => this._caseOrganizationService.getRegions(),
        getDepartments: () => fieldExists(filter.RegionId) ? this._caseOrganizationService.getDepartments(filter.RegionId) : empty$(),
        getOUs: () => fieldExists(filter.DepartmentId) && filter.DepartmentId != null ? this._caseOrganizationService.getOUs(filter.DepartmentId): empty$(),
        getIsAboutDepartments: () => fieldExists(filter.IsAboutRegionId) ? this._caseOrganizationService.getDepartments(filter.IsAboutRegionId) : empty$(),
        getIsAboutOUs: () => fieldExists(filter.IsAboutDepartmentId) && filter.IsAboutDepartmentId != null ? this._caseOrganizationService.getOUs(filter.IsAboutDepartmentId) : empty$(),
        getCaseTypes: () => fieldExists(filter.CaseTypes) ? this._caseOrganizationService.getCaseTypes() : empty$(),
        getProductAreas: () => fieldExists(filter.ProductAreas) ? this._caseOrganizationService.getProductAreas(filter.CaseTypeId, filter.ProductAreaId) : empty$(),
        getCategories: () => fieldExists(filter.Categories) ? this._caseOrganizationService.getCategories() : empty$(),
        getWorkingGroups: () => fieldExists(filter.WorkingGroups) ? this._caseOrganizationService.getWorkingGroups() : empty$(),
        getClosingReasons: () => fieldExists(filter.ClosingReasons) ? this._caseOrganizationService.getClosingReasons() : empty$(),
        getPerformers: (includePerfomer: boolean) =>
         fieldExists(filter.Performers) ? this._caseOrganizationService.getPerformers(includePerfomer ? filter.CasePerformerUserId : null, filter.CaseWorkingGroupId) : empty$(),
        getStateSecondaries: () => fieldExists(filter.StateSecondaries) ? this._caseOrganizationService.getStateSecondaries() : empty$()
      };
    }

    getCaseOptions(filter: CaseOptionsFilterModel) {
        var optionsHelper = this.getOptionsHelper(filter);
        let regions$ = optionsHelper.getRegions();
        let departments$ = optionsHelper.getDepartments();
        let oUs$ = optionsHelper.getOUs();
        let isAboutDepartments$ = optionsHelper.getIsAboutDepartments();
        let isAboutOUs$ = optionsHelper.getIsAboutOUs();
        let caseTypes$ = optionsHelper.getCaseTypes();
        let productAreas$ = optionsHelper.getProductAreas();
        let categories$ = optionsHelper.getCategories();
        let workingGroups$ = optionsHelper.getWorkingGroups();
        let closingReasons$ = optionsHelper.getClosingReasons();
        let perfomers$ = optionsHelper.getPerformers(true);
        let stateSecondaries$ = optionsHelper.getStateSecondaries();

        let bundledOptions$ = this._batchCaseOptionsService.getOptions(filter as BundleOptionsFilter);

        return forkJoin(bundledOptions$, regions$, departments$, oUs$, isAboutDepartments$, isAboutOUs$, caseTypes$, 
          productAreas$, categories$, closingReasons$, perfomers$, workingGroups$, stateSecondaries$)
                    .pipe(
                        take(1),
                        switchMap(([bundledOptions, regions, departments, oUs, isAboutDepartments, isAboutOUs, caseTypes,
                           productAreas, categories, closingReasons, perfomers, workingGroups, stateSecondaries]) => {
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

                            if (perfomers != null) {
                              options.performers = perfomers;
                            }

                            if (workingGroups != null) {
                              options.workingGroups = workingGroups;
                            }

                            if (stateSecondaries != null) {
                              options.stateSecondaries = stateSecondaries;
                          }

                            return of(options);
                      }),
                      catchError((e) => throwError(e))
                    );
    }

    getCaseSections() {
        return this._caseApiService.getCaseSections()
          .pipe(
            switchMap((jsCaseSections: any) => {
              if (!jsCaseSections) throwError("No data from server.");

              let sections = (jsCaseSections as Array<any>).map((jsSection: any) => {
                  return new CaseSectionInputModel(jsSection.id, jsSection.sectionHeader,
                       jsSection.sectionType, jsSection.isNewCollapsed,
                       jsSection.isEditCollapsed);
              });
              return of(sections);
            }),
            catchError((e) => throwError(e))
          );
    }

    // TODO: review - not all cases covered
    private fromJSONCaseEditInputModel(json: any): CaseEditInputModel {
        if (typeof json === 'string') {
             json = JSON.parse(json);
        }

        let fields = json.fields as any[] || new Array();
        let caseSolution = json.caseSolution ? <CaseSolution>json.caseSolution : null;
        let mailToTickets:MailToTicketInfo = json.mailToTickets ? <MailToTicketInfo>json.mailToTickets : null;
        let editMode = <CaseAccessMode>json.editMode;

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