import { Injectable } from '@angular/core';
import { defaultIfEmpty, take, switchMap, catchError, map } from 'rxjs/operators';
import { throwError, forkJoin, empty, Observable, of } from 'rxjs';
import { CaseApiService } from '../api/case/case-api.service';
import { BundleCaseOptionsService } from 'src/app/modules/case-edit-module/services/case-organization/bundle-case-options.service';
import { CaseOptionsFilterModel, BundleOptionsFilter, CaseOptions } from 'src/app/modules/shared-module/models';
import { CaseEditInputModel, CaseSectionInputModel, CaseSolution, MailToTicketInfo, CaseAccessMode, BaseCaseField, KeyValue, CaseLogActionData, CaseHistoryActionData, CaseAction } from '../../models';
import { CaseOrganizationService } from '../case-organization/case-organization.service';
import { CaseLogApiService } from '../api/case/case-log-api.service';
import { CaseLogModel, LogFile, CaseHistoryModel, CaseHistoryChangeModel } from '../../models/case/case-actions-api.model';
import { CaseActionsDataService } from './case-actions-data-service.service';
import { CaseHistoryApiService } from '../api/case/case-history-api.service';

@Injectable({ providedIn: 'root' })
export class CaseService {

    protected constructor(private caseOrganizationService: CaseOrganizationService,
         private batchCaseOptionsService: BundleCaseOptionsService,
         private caseActionsDataService: CaseActionsDataService,
         private caseLogApiService: CaseLogApiService,
         private caseHistoryApiService: CaseHistoryApiService,
         private caseApiService: CaseApiService ) {
    }

    getCaseData(caseId: number): Observable<CaseEditInputModel> {
      return this.caseApiService.getCaseData(caseId)
        .pipe(
          map((caseData: any) => {
            let model = this.fromJSONCaseEditInputModel(caseData);
            return model;
        }))
    }

    getCaseActions(caseId: number): Observable<CaseAction<any>[]> {
      let caseLogs$ = this.getCaseLogsData(caseId);
      let caseHistory$ = this.getCaseHistoryData(caseId);

      return forkJoin(caseLogs$, caseHistory$).pipe(      
        map(([caseLogsData, caseHistoryData]) => {
          return this.caseActionsDataService.process(caseLogsData, caseHistoryData);
      }));
    }

    private getCaseLogsData(caseId: number): Observable<CaseLogModel[]> {
      return this.caseLogApiService.getCaseLogs(caseId)
            .pipe(
                take(1),
                map(data => {
                  let items = data.map(x => this.fromJsonCaseLogModel(x));
                  return items;
                })
            );
    }

    private getCaseHistoryData(caseId) : Observable<CaseHistoryModel> {
        return this.caseHistoryApiService.getHistoryEvents(caseId)
              .pipe(
                  take(1),
                  map(jsonData => this.fromJsonCaseHistoryModel(jsonData))
              );
    }

    private fromJsonCaseHistoryModel(json): CaseHistoryModel {
      var model = Object.assign(new CaseHistoryModel(),  {
          emailLogs: json.emailLog || [],
          changes: json.changes && json.changes.length 
            ? json.changes.map(x => this.fromJsonCaseHistoryChangeModel(x))
            : []
      });
      return model;
    }

    private fromJsonCaseHistoryChangeModel(json) {
        return Object.assign(new CaseHistoryChangeModel(), json, {
                //todo:review
                createdAt: new Date(json.createdAt),
                previousValue: json.previousValue,
                currentValue:  json.currentValue
              });
    }

    private fromJsonCaseLogModel(data: any): CaseLogModel
    {
        let model = Object.assign(new CaseLogModel(), data, {
            createdAt: new Date(data.createdAt),
            files: data.files && data.files.length 
              ? data.files.map(f => Object.assign(new LogFile(), f))
              : []
        });
        return model;
    }

    getOptionsHelper(filter: CaseOptionsFilterModel) {
      const empty$ = () => empty().pipe(defaultIfEmpty(null));
      const fieldExists = (field: any) => field !== undefined;

      return {
        getRegions: () => this.caseOrganizationService.getRegions(),
        getDepartments: () => fieldExists(filter.RegionId) ? this.caseOrganizationService.getDepartments(filter.RegionId) : empty$(),
        getOUs: () => fieldExists(filter.DepartmentId) && filter.DepartmentId != null ? this.caseOrganizationService.getOUs(filter.DepartmentId): empty$(),
        getIsAboutDepartments: () => fieldExists(filter.IsAboutRegionId) ? this.caseOrganizationService.getDepartments(filter.IsAboutRegionId) : empty$(),
        getIsAboutOUs: () => fieldExists(filter.IsAboutDepartmentId) && filter.IsAboutDepartmentId != null ? this.caseOrganizationService.getOUs(filter.IsAboutDepartmentId) : empty$(),
        getCaseTypes: () => fieldExists(filter.CaseTypes) ? this.caseOrganizationService.getCaseTypes() : empty$(),
        getProductAreas: () => fieldExists(filter.ProductAreas) ? this.caseOrganizationService.getProductAreas(filter.CaseTypeId, filter.ProductAreaId) : empty$(),
        getCategories: () => fieldExists(filter.Categories) ? this.caseOrganizationService.getCategories() : empty$(),
        getWorkingGroups: () => fieldExists(filter.WorkingGroups) ? this.caseOrganizationService.getWorkingGroups() : empty$(),
        getClosingReasons: () => fieldExists(filter.ClosingReasons) ? this.caseOrganizationService.getClosingReasons() : empty$(),
        getPerformers: (includePerfomer: boolean) => fieldExists(filter.Performers) 
            ? this.caseOrganizationService.getPerformers(includePerfomer ? filter.CasePerformerUserId : null, filter.CaseWorkingGroupId) 
            : empty$(),
        getStateSecondaries: () => fieldExists(filter.StateSecondaries) ? this.caseOrganizationService.getStateSecondaries() : empty$()
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

        let bundledOptions$ = this.batchCaseOptionsService.getOptions(filter as BundleOptionsFilter);

        return forkJoin(bundledOptions$, regions$, departments$, oUs$, isAboutDepartments$, isAboutOUs$, caseTypes$, 
          productAreas$, categories$, closingReasons$, perfomers$, workingGroups$, stateSecondaries$)
                    .pipe(
                        take(1),
                        map(([bundledOptions, regions, departments, oUs, isAboutDepartments, isAboutOUs, caseTypes,
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

                            return options;
                      }),
                      catchError((e) => throwError(e))
                    );
    }

    getCaseSections() {
        return this.caseApiService.getCaseSections()
          .pipe(
            map((jsCaseSections: any) => {
              if (!jsCaseSections) throwError("No data from server.");

              let sections = (jsCaseSections as Array<any>).map((jsSection: any) => {
                  return new CaseSectionInputModel(
                            jsSection.id, 
                            jsSection.sectionHeader,
                            jsSection.sectionType, 
                            jsSection.isNewCollapsed,
                            jsSection.isEditCollapsed);
              });
              return sections;
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
        let mailToTickets: MailToTicketInfo = json.mailToTickets ? <MailToTicketInfo>json.mailToTickets : null;
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

export class ResponseDataHelper {
  fromJSONKeyValue(json: any): KeyValue {
      if (typeof json === 'string') { json = JSON.parse(json); }
      return Object.assign(new KeyValue(), json, {})
  }  
}