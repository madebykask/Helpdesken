import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CaseEditInputModel, CaseSectionType, CaseFieldModel, CaseSectionInputModel, ICaseField } from '../../models';
import { CaseOptionsFilterModel, OptionItem } from 'src/app/modules/shared-module/models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms/case-form-group';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';
import { DateTime } from 'luxon';
import { CaseDataStore } from './case-data.store';
import { OptionsHelper } from 'src/app/helpers/options-helper';

@Injectable({ providedIn: 'root' })
export class CaseEditDataHelper {

  constructor(private translateService: TranslateService,
     private dataSourceOptionsHelper: OptionsHelper) {
  }

  getCaseTitle(caseData: CaseEditInputModel): string {
    let title = this.translateService.instant('Ärende');
    if (caseData) {
        if (caseData.caseSolution) {
            title = caseData.caseSolution.name;
        }
    }
    return title;
  }

  hasField(caseData: CaseEditInputModel, name: string): boolean {
    if (caseData === null) {
        throw new Error('No Case Data.');
    }
    // console.log('hasField: ' + name);
    return caseData.fields.filter(f => f.name === name).length > 0;
  }

  hasSection(caseData: CaseEditInputModel, type: CaseSectionType): boolean {
    if (caseData === null) {
        throw new Error('No Case Data.');
    }
    return caseData.fields.filter(f => f.section === type && !f.isHidden).length > 0;
  }

  getField(caseData: CaseEditInputModel, name: string): CaseFieldModel<any> {
    if (caseData === null) {
        throw new Error('No Case Data.');
    }
    const fields = caseData.fields.filter(f => f.name === name);
    return fields.length <= 0 ? null : fields[0];
  }

    // returns value from caseData, not formControl
    getValue(caseData: CaseEditInputModel, name: string) {
        const field = this.getField(caseData, name);
        return field != null ? field.value || null : undefined; // null - value is null, undefined - no such field
    }

    getCaseOptionsFilter(caseData: CaseEditInputModel) {
      return this.createCaseOptionsFilter(caseData, (name: string) => this.getValue(caseData, name));
    }

    getFormCaseOptionsFilter(caseData: CaseEditInputModel, form: CaseFormGroup ) {
      return this.createCaseOptionsFilter(caseData, (name: string) => form.getValue(name));
    }

    getSectionInfoFields(section: CaseSectionInputModel, dataSource: CaseDataStore, caseData: CaseEditInputModel): string {
      const emptyValue = null;

      const getDate = (value: string, isShortData: boolean = false) => {
        return DateUtil.formatDate(value, isShortData ? DateTime.DATE_SHORT : null);
      };

      const initiatorFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.RegionId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.regionsStore$.value);
          }
          case CaseFieldsNames.DepartmentId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.departmentsStore$.value);
          }
          case CaseFieldsNames.OrganizationUnitId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.oUsStore$.value);
          }
        }
        return field.value;
      };

      const regardingFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.IsAbout_RegionId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.regionsStore$.value);
          }
          case CaseFieldsNames.IsAbout_DepartmentId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.isAboutDepartmentsStore$.value);
          }
          case CaseFieldsNames.IsAbout_OrganizationUnitId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.isAboutOUsStore$.value);
          }
        }
        return field.value;
      };

      const caseInfoFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.RegTime:
          case CaseFieldsNames.ChangeTime: {
            return getDate(field.value);
          }
          case CaseFieldsNames.RegistrationSourceCustomer: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.customerRegistrationSourcesStore$.value);
          }
          case CaseFieldsNames.CaseTypeId: {
            return this.dataSourceOptionsHelper.getFromMultiLevelOptions(field.value, dataSource.caseTypesStore$.value);
          }
          case CaseFieldsNames.ProductAreaId: {
            return this.dataSourceOptionsHelper.getFromMultiLevelOptions(field.value, dataSource.productAreasStore$.value);
          }
          case CaseFieldsNames.SystemId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.systemsStore$.value);
          }
          case CaseFieldsNames.UrgencyId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.urgenciesStore$.value);
          }
          case CaseFieldsNames.ImpactId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.impactsStore$.value);
          }
          case CaseFieldsNames.CategoryId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.categoriesStore$.value);
          }
          case CaseFieldsNames.SupplierId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.suppliersStore$.value);
          }
          case CaseFieldsNames.AgreedDate: {
            return getDate(field.value, true);
          }
          case CaseFieldsNames.Caption:
          case CaseFieldsNames.Description: {
            return field.value ? (<string>field.value).substring(0, 30) : '';
          }
          case CaseFieldsNames.Cost: {
            return field.value; // TODO: add Other cost and currency
          }
        }
        return field.value;
      };

      const caseManagementFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.WorkingGroupId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.workingGroupsStore$.value);
          }
          case CaseFieldsNames.CaseResponsibleUserId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.responsibleUsersStore$.value);
          }
          case CaseFieldsNames.PerformerUserId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.performersStore$.value);
          }
          case CaseFieldsNames.PriorityId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.prioritiesStore$.value);
          }
          case CaseFieldsNames.StatusId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.statusesStore$.value);
          }
          case CaseFieldsNames.StateSecondaryId: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.stateSecondariesStore$.value);
          }
          case CaseFieldsNames.Project: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.projectsStore$.value);
          }
          case CaseFieldsNames.Problem: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.problemsStore$.value);
          }
          case CaseFieldsNames.CausingPart: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.causingPartsStore$.value);
          }
          case CaseFieldsNames.Change: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.changesStore$.value);
          }
          case CaseFieldsNames.PlanDate:
          case CaseFieldsNames.WatchDate: {
              return getDate(field.value, true);
          }
          case CaseFieldsNames.SolutionRate: {
            return this.dataSourceOptionsHelper.getFromOptions(field.value, dataSource.solutionsRatesStore$.value);
          }
        }
        return field.value;
      };

      return section.caseSectionFields.map(name => {
        if (this.hasField(caseData, name)) {
           const field = this.getField(caseData, name);
           if (field.value == null) {
             return null;
           }
           switch (section.type) {
            case CaseSectionType.Initiator:
              return initiatorFields(name, field);

            case CaseSectionType.Regarding:
              return regardingFields(name, field);

            case CaseSectionType.ComputerInfo:
              return field.value;

            case CaseSectionType.CaseInfo:
              return caseInfoFields(name, field);

            case CaseSectionType.CaseManagement:
              return caseManagementFields(name, field);

           }
           return emptyValue;
        }
      })
      .filter(value => value)
      .join(' - ');
    }

    private createCaseOptionsFilter(caseData: CaseEditInputModel, getValue: (name: string) => number) {
      const filter = new CaseOptionsFilterModel();
      filter.RegionId = getValue(CaseFieldsNames.RegionId);
      filter.DepartmentId = getValue(CaseFieldsNames.DepartmentId);
      filter.IsAboutRegionId = getValue(CaseFieldsNames.IsAbout_RegionId);
      filter.IsAboutDepartmentId = getValue(CaseFieldsNames.IsAbout_DepartmentId);
      filter.CaseResponsibleUserId = getValue(CaseFieldsNames.CaseResponsibleUserId);
      filter.CaseWorkingGroupId = getValue(CaseFieldsNames.WorkingGroupId);
      filter.CasePerformerUserId = getValue(CaseFieldsNames.PerformerUserId);
      filter.CaseCausingPartId = getValue(CaseFieldsNames.CausingPart);
      filter.CaseTypeId = getValue(CaseFieldsNames.CaseTypeId);
      filter.ProductAreaId = getValue(CaseFieldsNames.ProductAreaId);
      filter.Changes = this.hasField(caseData, CaseFieldsNames.Change);
      filter.Currencies = this.hasField(caseData, CaseFieldsNames.Cost_Currency);
      filter.CausingParts  = this.hasField(caseData, CaseFieldsNames.CausingPart);
      filter.CustomerRegistrationSources = this.hasField(caseData, CaseFieldsNames.RegistrationSourceCustomer);
      filter.Impacts = this.hasField(caseData, CaseFieldsNames.ImpactId);
      filter.Performers = this.hasField(caseData, CaseFieldsNames.PerformerUserId);
      filter.Priorities = this.hasField(caseData, CaseFieldsNames.PriorityId);
      filter.Problems = this.hasField(caseData, CaseFieldsNames.Problem);
      filter.Projects = this.hasField(caseData, CaseFieldsNames.Project);
      filter.ResponsibleUsers = this.hasField(caseData, CaseFieldsNames.CaseResponsibleUserId);
      filter.SolutionsRates = this.hasField(caseData, CaseFieldsNames.SolutionRate);
      filter.StateSecondaries = this.hasField(caseData, CaseFieldsNames.StateSecondaryId);
      filter.Statuses = this.hasField(caseData, CaseFieldsNames.StatusId);
      filter.Suppliers = this.hasField(caseData, CaseFieldsNames.SupplierId); // Supplier_Country_Id
      filter.Systems = this.hasField(caseData, CaseFieldsNames.SystemId);
      filter.Urgencies = this.hasField(caseData, CaseFieldsNames.UrgencyId);
      filter.WorkingGroups = this.hasField(caseData, CaseFieldsNames.WorkingGroupId);
      filter.CaseTypes = this.hasField(caseData, CaseFieldsNames.CaseTypeId);
      filter.ProductAreas = this.hasField(caseData, CaseFieldsNames.ProductAreaId);
      filter.Categories = this.hasField(caseData, CaseFieldsNames.CategoryId);
      filter.ClosingReasons = this.hasField(caseData, CaseFieldsNames.ClosingReason);

      return filter;
  }
}
