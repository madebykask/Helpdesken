import { Injectable } from '@angular/core';
import { defaultIfEmpty, take, catchError, map } from 'rxjs/operators';
import { throwError, forkJoin, EMPTY, Observable } from 'rxjs';
import { CaseApiService } from '../api/case/case-api.service';
import { BundleCaseOptionsService } from 'src/app/modules/case-edit-module/services/case-organization/bundle-case-options.service';
import { CaseOptionsFilterModel, BundleOptionsFilter, CaseOptions } from 'src/app/modules/shared-module/models';
import { CaseEditInputModel, CaseSectionInputModel, CaseAction } from '../../models';
import { CaseOrganizationService } from '../case-organization/case-organization.service';
import { CaseLogApiService } from '../api/case/case-log-api.service';
import { CaseLogModel, CaseHistoryModel } from '../../models/case/case-actions-api.model';
import { CaseActionsDataService } from './case-actions-data.service';
import { CaseHistoryApiService } from '../api/case/case-history-api.service';
import { CaseTemplateApiService } from 'src/app/services/api/caseTemplate/case-template-api.service';
import { CaseModelBuilder } from '../../models/case/case-model-builder';

@Injectable({ providedIn: 'root' })
export class CaseService {
  private caseModelBuilder = new CaseModelBuilder();

  protected constructor(private caseOrganizationService: CaseOrganizationService,
    private batchCaseOptionsService: BundleCaseOptionsService,
    private caseActionsDataService: CaseActionsDataService,
    private caseLogApiService: CaseLogApiService,
    private caseHistoryApiService: CaseHistoryApiService,
    private caseApiService: CaseApiService,
    private caseTemplateApiService: CaseTemplateApiService) {
  }

  getTemplateData(templateId: number): Observable<CaseEditInputModel> {
    return this.caseApiService.getNewCase(templateId)
      .pipe(
        map((caseData: any) => {
          const model = this.caseModelBuilder.createCaseEditInputModel(caseData);
          return model;
        }));
  }

  getCaseData(caseId: number): Observable<CaseEditInputModel> {
    return this.caseApiService.getCaseData(caseId)
      .pipe(
        map((caseData: any) => {
          const model = this.caseModelBuilder.createCaseEditInputModel(caseData);
          return model;
        }));
  }

  getCaseActions(caseId: number): Observable<CaseAction<any>[]> {
    const caseLogs$ = this.getCaseLogsData(caseId);
    const caseHistory$ = this.getCaseHistoryData(caseId);

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
          const items = data.map(x => this.caseModelBuilder.createCaseLogModel(x));
          return items;
        })
      );
  }

  private getCaseHistoryData(caseId): Observable<CaseHistoryModel> {
    return this.caseHistoryApiService.getHistoryEvents(caseId)
      .pipe(
        take(1),
        map(jsonData => this.caseModelBuilder.createCaseHistoryModel(jsonData))
      );
  }

  getOptionsHelper(filter: CaseOptionsFilterModel): any {
    const empty$ = () => EMPTY.pipe(defaultIfEmpty(null));
    const fieldExists = (field: any) => field !== undefined;

    return {
      getRegions: () => this.caseOrganizationService.getRegions(),
      getDepartments: () => fieldExists(filter.RegionId) ? this.caseOrganizationService.getDepartments(filter.RegionId) : empty$(),
      getOUs: () => fieldExists(filter.DepartmentId) && filter.DepartmentId != null ?
        this.caseOrganizationService.getOUs(filter.DepartmentId) : empty$(),
      getIsAboutDepartments: () => fieldExists(filter.IsAboutRegionId) ?
        this.caseOrganizationService.getDepartments(filter.IsAboutRegionId) : empty$(),
      getIsAboutOUs: () => fieldExists(filter.IsAboutDepartmentId) && filter.IsAboutDepartmentId != null ?
        this.caseOrganizationService.getOUs(filter.IsAboutDepartmentId) : empty$(),
      getCaseTypes: () => fieldExists(filter.CaseTypes) ? this.caseOrganizationService.getCaseTypes() : empty$(),
      getProductAreas: (idToInclude?: number) => fieldExists(filter.ProductAreas) ?
        this.caseOrganizationService.getProductAreas(filter.CaseTypeId, idToInclude) : empty$(),
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
    const optionsHelper = this.getOptionsHelper(filter);

    const regions$ = optionsHelper.getRegions();
    const departments$ = optionsHelper.getDepartments();
    const oUs$ = optionsHelper.getOUs();
    const isAboutDepartments$ = optionsHelper.getIsAboutDepartments();
    const isAboutOUs$ = optionsHelper.getIsAboutOUs();
    const caseTypes$ = optionsHelper.getCaseTypes();
    const productAreas$ = optionsHelper.getProductAreas(filter.ProductAreaId);
    const categories$ = optionsHelper.getCategories();
    const workingGroups$ = optionsHelper.getWorkingGroups();
    const closingReasons$ = optionsHelper.getClosingReasons();
    const perfomers$ = optionsHelper.getPerformers(true);
    const stateSecondaries$ = optionsHelper.getStateSecondaries();

    const bundledOptions$ = this.batchCaseOptionsService.getOptions(filter as BundleOptionsFilter);

    const params = [bundledOptions$, regions$, departments$, oUs$, isAboutDepartments$, isAboutOUs$, caseTypes$,
      productAreas$, categories$, closingReasons$, perfomers$, workingGroups$, stateSecondaries$];

    return forkJoin(params).pipe(
      take(1),
      map(([bundledOptions, regions, departments, oUs, isAboutDepartments, isAboutOUs, caseTypes,
        productAreas, categories, closingReasons, perfomers, workingGroups, stateSecondaries]) => {
        const options = new CaseOptions();

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
        take(1),
        map((jsCaseSections: any) => {
          if (!jsCaseSections) { throwError('No data from server.'); }

          const sections = (jsCaseSections as Array<any>).map((jsSection: any) => {
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
}
