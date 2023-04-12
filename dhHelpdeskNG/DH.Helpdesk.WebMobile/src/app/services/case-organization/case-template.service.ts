import { Injectable } from '@angular/core';
import { take, map } from 'rxjs/operators';
import { CaseTemplateModel, CustomerCaseTemplateModel } from '../../models/caseTemplate/case-template.model';
import { Observable } from 'rxjs';
import { CaseTemplateApiService } from '../api/caseTemplate/case-template-api.service';
import { CaseTemplateFullModel } from 'src/app/models/caseTemplate/case-template-full.model';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { FinalActionEnum } from 'src/app/modules/shared-module/constants/finalAction.enum';

@Injectable({ providedIn: 'root' })
export class CaseTemplateService {

  constructor(private caseTemplateApiService: CaseTemplateApiService ) {
  }

  loadTemplates(): Observable<CustomerCaseTemplateModel[]> {
    //tood: move to a separate service
    return this.caseTemplateApiService.getCaseTemplates()
    .pipe(
      take(1),
      map(data => {
        return data.map(x => Object.assign(new CustomerCaseTemplateModel(), {
           ...x,
            items: x.items.map(i => Object.assign(new CaseTemplateModel(), { ...i }))
          }));
      })
    );
  }

  loadTemplate(id: number): Observable<CaseTemplateFullModel> {
    return this.caseTemplateApiService.getCaseTemplate(id);
  }

  applyWorkflow(workflowData: CaseTemplateFullModel, form: CaseFormGroup) {
    if (workflowData == null) {
      return;
    }
    const mapping = { // using mapping to keep required order
      region_Id: CaseFieldsNames.RegionId,
      department_Id: CaseFieldsNames.DepartmentId,
      oU_Id: CaseFieldsNames.OrganizationUnitId,
      costCentre: CaseFieldsNames.CostCentre,
      personsName: CaseFieldsNames.PersonName,
      personsPhone: CaseFieldsNames.PersonPhone,
      personsCellPhone: CaseFieldsNames.PersonCellPhone,
      category_Id: CaseFieldsNames.ReportedBy,
      reportedBy: CaseFieldsNames.ReportedBy,
      personsEmail: CaseFieldsNames.PersonEmail,
      place: CaseFieldsNames.Place,
      userCode: CaseFieldsNames.UserCode,
      updateNotifierInformation: CaseFieldsNames.UpdateNotifierInformation,
      //noMailToNotifier: CaseFieldsNames.,
      isAbout_PersonsName: CaseFieldsNames.IsAbout_PersonName,
      isAbout_PersonsEmail: CaseFieldsNames.IsAbout_PersonEmail,
      isAbout_PersonsPhone: CaseFieldsNames.IsAbout_PersonPhone,
      isAbout_PersonsCellPhone: CaseFieldsNames.IsAbout_PersonCellPhone,
      isAbout_Region_Id: CaseFieldsNames.IsAbout_RegionId,
      isAbout_Department_Id: CaseFieldsNames.IsAbout_DepartmentId,
      isAbout_OU_Id: CaseFieldsNames.IsAbout_OrganizationUnitId,
      isAbout_CostCentre: CaseFieldsNames.IsAbout_CostCentre,
      isAbout_Place: CaseFieldsNames.IsAbout_Place,
      isAbout_UserCode: CaseFieldsNames.IsAbout_UserCode,
      priority_Id: CaseFieldsNames.PriorityId,
      performerUser_Id: CaseFieldsNames.PerformerUserId,
      workingGroup_Id: CaseFieldsNames.WorkingGroupId,
      caseType_Id: CaseFieldsNames.CaseTypeId,
      productArea_Id: CaseFieldsNames.ProductAreaId,
      stateSecondary_Id: CaseFieldsNames.StateSecondaryId,
      inventoryNumber: CaseFieldsNames.InventoryNumber,
      inventoryType: CaseFieldsNames.ComputerTypeId,
      inventoryLocation: CaseFieldsNames.InventoryLocation,
      invoiceNumber: CaseFieldsNames.InvoiceNumber,
      referenceNumber: CaseFieldsNames.ReferenceNumber,
      system_Id: CaseFieldsNames.SystemId,
      urgency_Id: CaseFieldsNames.UrgencyId,
      impact_Id: CaseFieldsNames.ImpactId,
      watchDate: CaseFieldsNames.WatchDate,
      caption: CaseFieldsNames.Caption,
      description: CaseFieldsNames.Description,
      miscellaneous: CaseFieldsNames.Miscellaneous,
      project_Id: CaseFieldsNames.Project,
      text_External: CaseFieldsNames.Log_ExternalText,
      text_Internal: CaseFieldsNames.Log_InternalText,
      finishingCause_Id: CaseFieldsNames.ClosingReason,
      registrationSource: CaseFieldsNames.RegistrationSourceCustomer,
      causingPartId: CaseFieldsNames.CausingPart,
      sms: CaseFieldsNames.Sms,
      available: CaseFieldsNames.Available,
      cost: CaseFieldsNames.Cost,
      otherCost: CaseFieldsNames.Cost_OtherCost,
      currency: CaseFieldsNames.Cost_Currency,
      problem_Id: CaseFieldsNames.Problem,
      planDate: CaseFieldsNames.Place,
      agreedDate: CaseFieldsNames.AgreedDate,
      verifiedDescription: CaseFieldsNames.VerifiedDescription,
      verified: CaseFieldsNames.Verified,
      solutionRate: CaseFieldsNames.SolutionRate
    };

    // using mapping to keep required order
    Object.keys(mapping).forEach(key => {
      if (workflowData[key] != null && workflowData[key] != '') {
        this.setValueIfVisible(mapping[key], workflowData[key].replace(/\r\n|\r|\n/g, "<br />"), form);
      }
    });

    return <FinalActionEnum>workflowData.finalAction == null ? FinalActionEnum.NoAction : <FinalActionEnum>workflowData.finalAction;
  }

  private setValueIfVisible(fieldName: string, value: number | string | boolean, form: CaseFormGroup) {
    const control = form.get(fieldName);
    if (control == null) { return; }

    if (!control.fieldInfo.isHidden) {
      form.setValueWithNotification(fieldName, value);
    }
  }

}
