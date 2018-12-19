import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { CaseEditInputModel, CaseSectionType, BaseCaseField, CaseOptionsFilterModel } from "src/app/models";
import { FormGroup } from "@angular/forms";
import { CaseFieldsNames } from "src/app/helpers/constants";

@Injectable({ providedIn: 'root' })
export class CaseEditDataHelper {
  
  constructor(private translateService: TranslateService){
    
  }
  
  getCaseTitle(caseData: CaseEditInputModel) : string {
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
    if(caseData === null) {
        throw new Error('No Case Data.');
    }
    return caseData.fields.filter(f => f.section === type).length > 0;
  }

  getField(caseData: CaseEditInputModel, name: string): BaseCaseField<any> {
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

    getCaseOptionsFilter(caseData: CaseEditInputModel, getValue: (name: string) => number) {
      let filter = new CaseOptionsFilterModel();
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
      filter.Suppliers = this.hasField(caseData, CaseFieldsNames.SupplierId);// Supplier_Country_Id
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