import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { LocalStorageService } from '../../local-storage';
import { HttpClient } from '@angular/common/http';
import { take, map } from 'rxjs/operators';
import { CaseTemplateFullModel } from 'src/app/models/caseTemplate/case-template-full.model';
import { CaseTemplateModel } from 'src/app/models/caseTemplate/case-template.model';

@Injectable({ providedIn: 'root' })
export class CaseTemplateApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCaseTemplates(): Observable<Array<any>> {
    return this.getJson<Array<any>>(this.buildResourseUrl('/api/templates/', { mobileOnly: true }, true, true))
    .pipe(
      take(1),
      map(data => {
        return data.map(x => Object.assign(new CaseTemplateModel(), { ...x, id: x.caseSolutionId }));
      })
    );
  }

  getCaseTemplate(id: number) {
    return this.getJson(this.buildResourseUrl(`/api/templates/${id}`, { mobileOnly: true }, true, true))
    .pipe(
      take(1),
      map((jsItem: any) => {
        return Object.assign(new CaseTemplateFullModel(), jsItem);
/*         let model = new CaseTemplateFullModel();
        model.id = jsItem.id;
        model.impact_Id = jsItem.impact_Id;
        model.inventoryLocation = jsItem.inventoryLocation;
        model.inventoryNumber = jsItem.inventoryNumber;
        model.inventoryType = jsItem.inventoryType;
        model.isAbout_CostCentre = jsItem.isAbout_CostCentre;
        model.isAbout_Department_Id = jsItem.isAbout_Department_Id;
        model.isAbout_OU_Id = jsItem.isAbout_OU_Id;
        model.isAbout_PersonsCellPhone = jsItem.isAbout_PersonsCellPhone;
        model.isAbout_PersonsEmail = jsItem.isAbout_PersonsEmail;
        model.isAbout_PersonsName = jsItem.isAbout_PersonsName;
        model.isAbout_PersonsPhone = jsItem.isAbout_PersonsPhone;
        model.isAbout_Place = jsItem.isAbout_Place;
        model.isAbout_Region_Id = jsItem.isAbout_Region_Id;
        model.isAbout_ReportedBy = jsItem.isAbout_ReportedBy;
        model.isAbout_UserCode = jsItem.isAbout_UserCode;
        model.isAbout_UserSearchCategory_Id = jsItem.isAbout_UserSearchCategory_Id;
        model.miscellaneous = jsItem.miscellaneous;
        model.name = jsItem.name;
        model.noMailToNotifier = jsItem.noMailToNotifier;
        model.oU_Id = jsItem.oU_Id;
        model.orderNum = jsItem.orderNum;
        model.otherCost = jsItem.otherCost;
        model.overWritePopUp = jsItem.overWritePopUp;
        model.performerUser_Id = jsItem.performerUser_Id;
        model.personsCellPhone = jsItem.personsCellPhone;
        model.personsEmail = jsItem.personsEmail;
        model.personsName = jsItem.personsName;
        model.personsPhone = jsItem.personsPhone;
        model.place = jsItem.place;
        model.planDate = jsItem.planDate;
        model.priority_Id = jsItem.priority_Id;
        model.problem_Id = jsItem.problem_Id;
        model.productArea_Id = jsItem.productArea_Id;
        model.project_Id = jsItem.project_Id;
        model.referenceNumber = jsItem.referenceNumber;
        model.region_Id = jsItem.region_Id;
        model.registrationSource = jsItem.registrationSource;
        model.reportedBy = jsItem.reportedBy;
        model.sms = jsItem.sms;
        model.solutionRate = jsItem.solutionRate;
        model.stateSecondary_Id = jsItem.stateSecondary_Id;
        model.isActive = jsItem.isActive;
        model.status_Id = jsItem.status_Id;
        model.supplier_Id = jsItem.supplier_Id;
        model.system_Id = jsItem.system_Id;
        model.text_External = jsItem.text_External;
        model.text_Internal = jsItem.text_Internal;
        model.updateNotifierInformation = jsItem.updateNotifierInformation;
        model.urgency_Id = jsItem.urgency_Id;
        model.userCode = jsItem.userCode;
        model.verified = jsItem.verified;
        model.verifiedDescription = jsItem.verifiedDescription;
        model.watchDate = jsItem.watchDate;
        model.workingGroup_Id = jsItem.workingGroup_Id;
        return model;*/
      }));
  }
}

