import { Injectable } from '@angular/core';
import { defaultIfEmpty, take, catchError, map } from 'rxjs/operators';
import { throwError, forkJoin, EMPTY, Observable, from, of } from 'rxjs';
import { CaseApiService } from '../api/case/case-api.service';
import { BundleCaseOptionsService } from 'src/app/modules/case-edit-module/services/case-organization/bundle-case-options.service';
import { CaseOptionsFilterModel, BundleOptionsFilter, CaseOptions, OptionItem,
        BundledCaseOptions, MultiLevelOptionItem } from 'src/app/modules/shared-module/models';
import { CaseEditInputModel, CaseSectionInputModel, CaseAction } from '../../models';
import { CaseOrganizationService } from '../case-organization/case-organization.service';
import { CaseLogApiService } from '../api/case/case-log-api.service';
import { CaseLogModel, CaseHistoryModel } from '../../models/case/case-actions-api.model';
import { CaseActionsDataService } from './case-actions-data.service';
import { CaseHistoryApiService } from '../api/case/case-history-api.service';
import { CaseModelBuilder } from '../../models/case/case-model-builder';

@Injectable({ providedIn: 'root' })
export class CaseService {
  private caseModelBuilder = new CaseModelBuilder();

  protected constructor(private caseOrganizationService: CaseOrganizationService,
    private batchCaseOptionsService: BundleCaseOptionsService,
    private caseActionsDataService: CaseActionsDataService,
    private caseLogApiService: CaseLogApiService,
    private caseHistoryApiService: CaseHistoryApiService,
    private caseApiService: CaseApiService) {
  }

  getTemplateData(templateId: number, customerId: number): Observable<CaseEditInputModel> {
    return this.caseApiService.getNewCase(templateId, customerId)
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

  getCaseActions(caseId: number, customerId: number): Observable<CaseAction<any>[]> {
    const caseLogs$ = this.getCaseLogsData(caseId, customerId);
    const caseHistory$ = this.getCaseHistoryData(caseId, customerId);

    return forkJoin([caseLogs$, caseHistory$]).pipe(
      map(([caseLogsData, caseHistoryData]) => {
        return this.caseActionsDataService.process(caseLogsData, caseHistoryData);
      }));
  }

  getOptionsHelper(filter: CaseOptionsFilterModel, customerId: number): any {
    const empty$ = () => EMPTY.pipe(defaultIfEmpty(null));
    const fieldExists = (field: any) => field !== undefined;

    return {
      getRegions: () => this.caseOrganizationService.getRegions(customerId),
      getDepartments: () => fieldExists(filter.RegionId) ? this.caseOrganizationService.getDepartments(filter.RegionId, customerId) : empty$(),
      getOUs: () => fieldExists(filter.DepartmentId) && filter.DepartmentId != null ?
        this.caseOrganizationService.getOUs(filter.DepartmentId, customerId) : empty$(),
      getIsAboutDepartments: () => fieldExists(filter.IsAboutRegionId) ?
        this.caseOrganizationService.getDepartments(filter.IsAboutRegionId, customerId) : empty$(),
      getIsAboutOUs: () => fieldExists(filter.IsAboutDepartmentId) && filter.IsAboutDepartmentId != null ?
        this.caseOrganizationService.getOUs(filter.IsAboutDepartmentId, customerId) : empty$(),
      getCaseTypes: () => fieldExists(filter.CaseTypes) ? this.caseOrganizationService.getCaseTypes(customerId) : empty$(),
      getProductAreas: (idToInclude?: number) => fieldExists(filter.ProductAreas) ?
        this.caseOrganizationService.getProductAreas(filter.CaseTypeId, idToInclude, customerId) : empty$(),
      getCategories: () => fieldExists(filter.Categories) ? this.caseOrganizationService.getCategories(customerId) : empty$(),
      getWorkingGroups: () => fieldExists(filter.WorkingGroups) ? this.caseOrganizationService.getWorkingGroups(customerId) : empty$(),
      getClosingReasons: () => fieldExists(filter.ClosingReasons) ? this.caseOrganizationService.getClosingReasons(customerId) : empty$(),
      getPerformers: (includePerfomer: boolean) => fieldExists(filter.Performers)
        ? this.caseOrganizationService.getPerformers(includePerfomer ? filter.CasePerformerUserId : null, filter.CaseWorkingGroupId, customerId)
        : empty$(),
      getStateSecondaries: () => fieldExists(filter.StateSecondaries) ? this.caseOrganizationService.getStateSecondaries(customerId) : empty$()
    };
  }

  getCaseOptions(filter: CaseOptionsFilterModel, customerId: number) {
    const optionsHelper = this.getOptionsHelper(filter, customerId);

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

    const bundledOptions$ = this.batchCaseOptionsService.getOptions(filter as BundleOptionsFilter, customerId);

    const params = [bundledOptions$, regions$, departments$, oUs$, isAboutDepartments$, isAboutOUs$, caseTypes$,
      productAreas$, categories$, closingReasons$, perfomers$, workingGroups$, stateSecondaries$];

    return forkJoin(params).pipe(
      take(1),
      map(([bundledOptions, regions, departments, oUs, isAboutDepartments, isAboutOUs, caseTypes,
        productAreas, categories, closingReasons, perfomers, workingGroups, stateSecondaries] :
        [BundledCaseOptions, OptionItem[], OptionItem[], OptionItem[], OptionItem[], OptionItem[],
         MultiLevelOptionItem[], MultiLevelOptionItem[], MultiLevelOptionItem[], MultiLevelOptionItem[],
        OptionItem[], OptionItem[], OptionItem[]]) => {

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

  getCaseSections(customerId: number) {
    return this.caseApiService.getCaseSections(customerId)
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
              jsSection.isEditCollapsed,
              jsSection.caseSectionFields);
          });
          return sections;
        }),
        catchError((e) => throwError(e))
      );
  }

  private getCaseLogsData(caseId: number, customerId: number): Observable<CaseLogModel[]> {
    return this.caseLogApiService.getCaseLogs(caseId, customerId)
      .pipe(
        take(1),
        map(data => {
          const items = data.map(x => this.caseModelBuilder.createCaseLogModel(x));
          return items;
        })
      );
  }

  private getCaseHistoryData(caseId: number, customerId: number): Observable<CaseHistoryModel> {
    return this.caseHistoryApiService.getHistoryEvents(caseId, customerId)
      .pipe(
        take(1),
        map(jsonData => this.caseModelBuilder.createCaseHistoryModel(jsonData))
      );
  }
}
