import { Injectable } from '@angular/core';
import { CaseApiService } from '../api/case/case-api.service';
import { throwError, Observable } from 'rxjs';
import { isNumeric } from 'rxjs/internal/util/isNumeric';
import { CaseEditOutputModel, CaseEditInputModel } from '../../models';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';

@Injectable({ providedIn: 'root' })
export class CaseSaveService {

  protected constructor(private caseApiService: CaseApiService ) {
  }

  public saveCase(form: CaseFormGroup, caseInputData: CaseEditInputModel): Observable<any> {
    const model = new CaseEditOutputModel();
    model.caseId = caseInputData.id;
    model.caseGuid = caseInputData.caseGuid;
    model.extendedCaseGuid = caseInputData.extendedCaseData != null ? caseInputData.extendedCaseData.extendedCaseGuid : null;
    model.caseSolutionId = caseInputData.caseSolution != null ? caseInputData.caseSolution.caseSolutionId : null;
    model.reportedBy = this.getStringValue(form, CaseFieldsNames.ReportedBy);
    model.personName = this.getStringValue(form, CaseFieldsNames.PersonName);
    model.personEmail = this.getStringValue(form, CaseFieldsNames.PersonEmail);
    model.personPhone = this.getStringValue(form, CaseFieldsNames.PersonPhone);
    model.personCellPhone = this.getStringValue(form, CaseFieldsNames.PersonCellPhone);
    model.regionId = this.getNumericValue(form, CaseFieldsNames.RegionId);
    model.departmentId = this.getNumericValue(form, CaseFieldsNames.DepartmentId);
    model.organizationUnitId = this.getNumericValue(form, CaseFieldsNames.OrganizationUnitId);
    model.costCentre = this.getStringValue(form, CaseFieldsNames.CostCentre);
    model.place = this.getStringValue(form, CaseFieldsNames.Place);
    model.userCode = this.getStringValue(form, CaseFieldsNames.UserCode);

    model.isAbout_ReportedBy = this.getStringValue(form, CaseFieldsNames.IsAbout_ReportedBy);
    model.isAbout_PersonName = this.getStringValue(form, CaseFieldsNames.IsAbout_PersonName);
    model.isAbout_PersonEmail = this.getStringValue(form, CaseFieldsNames.IsAbout_PersonEmail);
    model.isAbout_PersonPhone = this.getStringValue(form, CaseFieldsNames.IsAbout_PersonPhone);
    model.isAbout_PersonCellPhone = this.getStringValue(form, CaseFieldsNames.IsAbout_PersonCellPhone);
    model.isAbout_RegionId = this.getNumericValue(form, CaseFieldsNames.IsAbout_RegionId);
    model.isAbout_DepartmentId = this.getNumericValue(form, CaseFieldsNames.IsAbout_DepartmentId);
    model.isAbout_OrganizationUnitId = this.getNumericValue(form, CaseFieldsNames.IsAbout_OrganizationUnitId);
    model.isAbout_CostCentre = this.getStringValue(form, CaseFieldsNames.IsAbout_CostCentre);
    model.isAbout_Place = this.getStringValue(form, CaseFieldsNames.IsAbout_Place);
    model.isAbout_UserCode = this.getStringValue(form, CaseFieldsNames.IsAbout_UserCode);

    model.inventoryNumber = this.getStringValue(form, CaseFieldsNames.InventoryNumber);
    model.computerTypeId = this.getStringValue(form, CaseFieldsNames.ComputerTypeId);
    model.inventoryLocation = this.getStringValue(form, CaseFieldsNames.InventoryLocation);

    model.сaseNumber = this.getStringValue(form, CaseFieldsNames.CaseNumber);
    model.regTime = this.getDateValue(form, CaseFieldsNames.RegTime);
    model.changeTime = this.getDateValue(form, CaseFieldsNames.ChangeTime);
    model.userId = this.getNumericValue(form, CaseFieldsNames.UserId);
    model.registrationSourceCustomerId = this.getNumericValue(form, CaseFieldsNames.RegistrationSourceCustomer);
    model.caseTypeId = this.getNumericValue(form, CaseFieldsNames.CaseTypeId);
    model.productAreaId = this.getNumericValue(form, CaseFieldsNames.ProductAreaId);
    model.systemId = this.getNumericValue(form, CaseFieldsNames.SystemId);
    model.urgencyId = this.getNumericValue(form, CaseFieldsNames.UrgencyId);
    model.impactId = this.getNumericValue(form, CaseFieldsNames.ImpactId);
    model.categoryId = this.getNumericValue(form, CaseFieldsNames.CategoryId);
    model.supplierId = this.getNumericValue(form, CaseFieldsNames.SupplierId);
    model.supplierCountryId = this.getNumericValue(form, CaseFieldsNames.SupplierCountryId);
    model.invoiceNumber = this.getStringValue(form, CaseFieldsNames.InvoiceNumber);
    model.referenceNumber = this.getStringValue(form, CaseFieldsNames.ReferenceNumber);
    model.miscellaneous = this.getStringValue(form, CaseFieldsNames.Miscellaneous);
    model.description = this.getStringValue(form, CaseFieldsNames.Description);
    model.caption = this.getStringValue(form, CaseFieldsNames.Caption);
    model.contactBeforeAction = this.getBooleanValue(form, CaseFieldsNames.ContactBeforeAction);
    model.sms = this.getBooleanValue(form, CaseFieldsNames.Sms);
    model.agreedDate = this.getStringValue(form, CaseFieldsNames.AgreedDate);
    model.available = this.getStringValue(form, CaseFieldsNames.Available);
    model.cost = this.getNumericValue(form, CaseFieldsNames.Cost);
    model.otherCost = this.getNumericValue(form, CaseFieldsNames.Cost_OtherCost);
    model.costCurrency = this.getStringValue(form, CaseFieldsNames.Cost_Currency);

    model.workingGroupId = this.getNumericValue(form, CaseFieldsNames.WorkingGroupId);
    model.responsibleUserId = this.getNumericValue(form, CaseFieldsNames.CaseResponsibleUserId);
    model.performerId = this.getNumericValue(form, CaseFieldsNames.PerformerUserId);
    model.priorityId = this.getNumericValue(form, CaseFieldsNames.PriorityId);
    model.statusId = this.getNumericValue(form, CaseFieldsNames.StatusId);
    model.stateSecondaryId = this.getNumericValue(form, CaseFieldsNames.StateSecondaryId);
    model.projectId = this.getNumericValue(form, CaseFieldsNames.Project);
    model.problemId = this.getNumericValue(form, CaseFieldsNames.Problem);
    model.causingPartId = this.getNumericValue(form, CaseFieldsNames.CausingPart);
    model.changeId = this.getNumericValue(form, CaseFieldsNames.Change);
    model.planDate = this.getDateValue(form, CaseFieldsNames.PlanDate);
    model.watchDate = this.getDateValue(form, CaseFieldsNames.WatchDate);
    model.verified = this.getBooleanValue(form, CaseFieldsNames.Verified);
    model.verifiedDescription = this.getStringValue(form, CaseFieldsNames.VerifiedDescription);
    model.solutionRate = this.getStringValue(form, CaseFieldsNames.SolutionRate);

    model.logExternalText = this.getStringValue(form, CaseFieldsNames.Log_ExternalText);
    model.logInternalText = this.getStringValue(form, CaseFieldsNames.Log_InternalText);

    model.logSendMailToNotifier =  this.getBooleanValue(form, CaseFieldsNames.Log_SendMailToNotifier);
    model.logExternalEmailsCc = this.getStringValue(form, CaseFieldsNames.Log_ExternalEmailsCC);

    model.logSendMailToPerformer =  this.getBooleanValue(form, CaseFieldsNames.Log_SendMailToPerformer);
    model.logInternalEmailTo = this.getStringValue(form, CaseFieldsNames.Log_InternalEmailsTo);
    model.logInternalEmailCc = this.getStringValue(form, CaseFieldsNames.Log_InternalEmailsCC);

    model.finishingDescription = this.getStringValue(form, CaseFieldsNames.FinishingDescription);
    model.closingReason = this.getNumericValue(form, CaseFieldsNames.ClosingReason);
    model.finishingDate = this.getDateValue(form, CaseFieldsNames.FinishingDate);

    return this.caseApiService.saveCaseData(model, caseInputData.customerId);
  }
  public checkBusinessRulesOnSave(form: CaseFormGroup,cid: number): Observable<any>{
    const model = new CaseEditOutputModel();
    model.closingReason = this.getNumericValue(form, CaseFieldsNames.ClosingReason);
    model.finishingDate = this.getDateValue(form, CaseFieldsNames.FinishingDate);
    model.statusId = this.getNumericValue(form, CaseFieldsNames.StatusId);
    model.stateSecondaryId = this.getNumericValue(form, CaseFieldsNames.StateSecondaryId);
    let isValid = this.caseApiService.checkBusinessRulesOnSave(model, cid);
    return isValid;
    }
  private getNumericValue(form: CaseFormGroup, fieldName: string): number {
    if (this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if (value == null) { return null; }
      if (isNumeric(value)) {
        return Number(value);
      }
      throwError(`Not supported value. Expecting number, but recieved ${typeof(value)}.`);
    }
    return undefined;
  }

  private getDateValue(form: CaseFormGroup, fieldName: string): string {
    if (this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if (!value) { return null; }
      return value;
    }
    return undefined;
  }

  private getStringValue(form: CaseFormGroup, fieldName: string): string {
    if (this.hasValue(form, fieldName)) {
      return form.controls[fieldName].value;
    }
    return undefined;
  }

  private getBooleanValue(form: CaseFormGroup, fieldName: string): boolean {
    if (this.hasValue(form, fieldName)) {
      const value = form.controls[fieldName].value;
      if (typeof value === 'boolean') {
        return Boolean(value);
      } else {
        return null;
      }
    }
    return undefined;
  }

  private hasValue(form: CaseFormGroup, fieldName: string): boolean {
    return form.controls[fieldName] != null;
  }
}
