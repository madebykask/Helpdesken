import { Injectable } from '@angular/core';
import { CaseFieldValueChangedEvent, CommunicationService, Channels } from 'src/app/services/communication';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { NotifierFormFieldsSetter } from 'src/app/modules/shared-module/models/forms/notifier-form-fields-setter';
import { NotifierModel, NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { take } from 'rxjs/operators';
import { OptionItem, MultiLevelOptionItem } from 'src/app/modules/shared-module/models';
import { CaseDataReducers, CaseDataReducersFactory } from './case-data.reducers';
import { WorkingGroupsService } from 'src/app/services/case-organization/workingGroups-service';
import { StateSecondariesService } from 'src/app/services/case-organization/stateSecondaries-service';
import { CaseTypesService } from 'src/app/services/case-organization/caseTypes-service';
import { CaseWatchDateApiService } from '../../services/api/case/case-watchDate-api.service';
import { ProductAreasService } from 'src/app/services/case-organization/productAreas-service';
import { NotifierService } from '../../services/notifier.service';
import { CaseEditDataHelper } from './case-editdata.helper';
import { CaseService } from '../../services/case';
import { CaseFieldModel, CaseEditInputModel } from '../../models';
import { DateTime } from 'luxon';
import { CaseDataStore } from './case-data.store';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';

@Injectable({ providedIn: 'root' })
export class CaseEditLogic {

  protected constructor(private сaseDataReducersFactory: CaseDataReducersFactory,
    private workingGroupsService: WorkingGroupsService,
    private stateSecondariesService: StateSecondariesService,
    private caseTypesService: CaseTypesService,
    private caseWatchDateApiService: CaseWatchDateApiService,
    private productAreasService: ProductAreasService,
    private notifierService: NotifierService,
    private caseDataHelpder: CaseEditDataHelper,
    private caseService: CaseService) {

  }

  public runUpdates(v: CaseFieldValueChangedEvent, dataSource: CaseDataStore, caseData: CaseEditInputModel, form: CaseFormGroup) { // TODO: move to new class
    const reducer = this.getCaseDataReducers(dataSource);
    const filters = this.caseDataHelpder.getFormCaseOptionsFilter(caseData, form);
    const optionsHelper = this.caseService.getOptionsHelper(filters);

    // NOTE: remember to update case data reducer when adding new fields
    switch (v.name) {
      case CaseFieldsNames.RegionId: {
        optionsHelper.getDepartments().pipe(
          take(1),
        ).subscribe((deps: OptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.DepartmentId, { items: deps });
        });
        break;
      }
      case CaseFieldsNames.DepartmentId: {
        optionsHelper.getOUs().pipe(
          take(1),
        ).subscribe((ous: OptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.OrganizationUnitId, { items: ous });
        });
        break;
      }
      case CaseFieldsNames.IsAbout_RegionId: {
        optionsHelper.getIsAboutDepartments().pipe(
          take(1),
        ).subscribe((deps: OptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.IsAbout_DepartmentId, { items: deps });
        });
        break;
      }
      case CaseFieldsNames.IsAbout_DepartmentId: {
        optionsHelper.getIsAboutOUs().pipe(
          take(1),
        ).subscribe((ous: OptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.IsAbout_OrganizationUnitId, { items: ous });
        });
        break;
      }
      case CaseFieldsNames.CaseTypeId: {
        if (v.value) {
          this.caseTypesService.getCaseType(v.value).pipe(
              take(1)
            ).subscribe(ct => {
              if (ct && ct.performerUserId != null && !this.getField(caseData, CaseFieldsNames.PerformerUserId).setByTemplate) {
                if (!dataSource.performersStore$.value.some((e) => e.value === ct.performerUserId)) {
                  // get new list of performers with casetype perfomer included
                  filters.CasePerformerUserId = ct.performerUserId;
                  optionsHelper.getPerformers(true).pipe(
                    take(1)
                  ).subscribe((o: OptionItem[]) => {
                    reducer.caseDataReducer(CaseFieldsNames.PerformerUserId, { items: o });
                  });
                }
              }
              if (ct && ct.workingGroupId != null) {
                form.setSafe(CaseFieldsNames.WorkingGroupId, ct.workingGroupId);
              }
              if (ct && ct.performerUserId != null) {
                form.setSafe(CaseFieldsNames.PerformerUserId, ct.performerUserId);
              }
            });
        }
        optionsHelper.getProductAreas(null).pipe(
          take(1)
        ).subscribe((o: MultiLevelOptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.ProductAreaId, { items: o });
        });
        break;
      }
      case CaseFieldsNames.WorkingGroupId: {
        optionsHelper.getPerformers(false).pipe(
          take(1)
        ).subscribe((o: OptionItem[]) => {
          reducer.caseDataReducer(CaseFieldsNames.PerformerUserId, { items: o });
        });

        if (!!v.value && form.contains(CaseFieldsNames.StateSecondaryId)) {
          this.workingGroupsService.getWorkingGroup(v.value).pipe(
            take(1)
          ).subscribe(wg => {
            if (wg && wg.stateSecondaryId != null) {
              form.setSafe(CaseFieldsNames.StateSecondaryId, wg.stateSecondaryId);
            }
          });
        }
        break;
      }
      case CaseFieldsNames.StateSecondaryId: {
        let sendExternalEmailsControl = form.get(CaseFieldsNames.Log_SendMailToNotifier);
        let externalLogTextControl = form.get(CaseFieldsNames.Log_ExternalText);
        if (v.value) {
          this.stateSecondariesService.getStateSecondary(v.value)
          .pipe(
            take(1)
          ).subscribe(ss => {
            if (ss && ss.workingGroupId != null && form.contains(CaseFieldsNames.WorkingGroupId)) {
              form.setSafe(CaseFieldsNames.WorkingGroupId, ss.workingGroupId);
            }
            const departmentCtrl = form.get(CaseFieldsNames.DepartmentId);
            if (ss.recalculateWatchDate && departmentCtrl.value) {
                this.caseWatchDateApiService.getWatchDate(departmentCtrl.value).pipe(
                  take(1)
                ).subscribe(date => form.setSafe(CaseFieldsNames.WatchDate, date));
            }
            // update extneral log field state
            sendExternalEmailsControl = form.get(CaseFieldsNames.Log_SendMailToNotifier);
            externalLogTextControl = form.get(CaseFieldsNames.Log_ExternalText);
            if (ss.noMailToNotifier === true) {
              sendExternalEmailsControl.disable({ onlySelf: true, emitEvent: true });
            } else if (externalLogTextControl && externalLogTextControl.disabled === false) {
              //enable only in case extneral log note text is enabled as well
              sendExternalEmailsControl.enable({ onlySelf: true, emitEvent: true });
            }
          });
        } else if (externalLogTextControl && externalLogTextControl.disabled === false) {
          //enable only in case extneral log note text is enabled as well
          sendExternalEmailsControl.enable({ onlySelf: true, emitEvent: true });
        }
        break;
      }
      case CaseFieldsNames.ProductAreaId: {
        if (v.value) {
          this.productAreasService.getProductArea(v.value).pipe(
            take(1)
          ).subscribe(ct => {
            if (ct && ct.workingGroupId != null) {
              form.setSafe(CaseFieldsNames.WorkingGroupId, ct.workingGroupId);
            }
            if (ct && ct.priorityId != null) {
              form.setSafe(CaseFieldsNames.PriorityId, ct.priorityId);
            }
          });
        }
        break;
      }
      case CaseFieldsNames.ClosingReason: {
        if (v.value) {
          const finishingDateControl = form.get(CaseFieldsNames.FinishingDate);
          if (finishingDateControl && !finishingDateControl.value) {
            form.setSafe(CaseFieldsNames.FinishingDate, DateTime.local().toString());
          }
        } else {
          form.setSafe(CaseFieldsNames.FinishingDate, '');
        }
        break;
      }
      case CaseFieldsNames.ReportedBy: {
        const userId = v.value !== null ? +v.value : 0;
        const notifierType = v.text && v.text.length ? <NotifierType>+v.text : NotifierType.Initiator;

        if (!isNaN(userId) && userId > 0) {
          this.notifierService.getNotifier(v.value).pipe(
            take(1)
          ).subscribe(x => {
             this.processNotifierChanged(x, notifierType === NotifierType.Regarding, form);
          });
        } else {
          this.processNotifierChanged(null, notifierType === NotifierType.Regarding, form);
        }
        break;
      }
    }
  }

  public getField(caseData: CaseEditInputModel, name: string): CaseFieldModel<any> {
    return this.caseDataHelpder.getField(caseData, name);
  }

  private processNotifierChanged(data: NotifierModel, isRegarding: boolean, form: CaseFormGroup) {
    const formFieldsSetter = form.getNotifierFieldsSetter(isRegarding);

    if (data) {
      formFieldsSetter.setReportedBy(data.userId);
      formFieldsSetter.setPersonName(data.name);
      formFieldsSetter.setPersonEmail(data.email);
      formFieldsSetter.setPersonPhone(data.phone);
      formFieldsSetter.setPersonCellPhone(data.cellphone);
      formFieldsSetter.setPlace(data.place);
      formFieldsSetter.setUserCode(data.userCode);
      formFieldsSetter.setCostCenter(data.costCentre);

      this.updateOrganisationFields(formFieldsSetter, data, form);
    } else {
      this.resetNotifierFields(formFieldsSetter);
    }
  }

  private updateOrganisationFields(notifierFieldsSetter: NotifierFormFieldsSetter, data: IOrganisationData, form: CaseFormGroup) {
    const regionId = data.regionId ? +data.regionId : null;
    const departmentId = data.departmentId ? +data.departmentId : null;
    const ouId = data.ouId ? +data.ouId : null;
    const formRegionId = +form.getValue(CaseFieldsNames.RegionId);

    if (regionId !== formRegionId) {
      notifierFieldsSetter.setRegion(regionId || '');
      notifierFieldsSetter.setDepartment(departmentId || '');
      notifierFieldsSetter.setOU(ouId || '');
    } else {
        // just set new department if exists
        if (!isNaN(departmentId) && departmentId) {
          notifierFieldsSetter.setDepartment(departmentId || '');
          notifierFieldsSetter.setOU(ouId || '');
        }
    }
  }

/*   private changeRegion(notifierFieldsSetter: NotifierFormFieldsSetter, regionId?: number, departmentId?: number, ouId?: number) {
    // change first to update form
    notifierFieldsSetter.setRegion(regionId || '');

     const reducer = this.getCaseDataReducers();
    const optionsHelper = this.getFormOptionsHelpers(); 

    // change to new region and load departments
     optionsHelper.getDepartments().pipe(
      take(1)
    ).subscribe(deps => {
      reducer.caseDataReducer(CaseFieldsNames.DepartmentId, { items: deps });
      this.changeDepartment(notifierFieldsSetter, departmentId, ouId);
    }); 
    this.changeDepartment(notifierFieldsSetter, departmentId, ouId);
  } */

/*   private changeDepartment(notifierFieldsSetter: NotifierFormFieldsSetter, departmentId: number, ouId?: number) {

    notifierFieldsSetter.setDepartment(departmentId || '');

     const reducer = this.getCaseDataReducers();
    const optionsHelper = this.getFormOptionsHelpers();

    // load OUs
    optionsHelper.getOUs(departmentId).pipe(
      take(1)
    ).subscribe(ous => {
        reducer.caseDataReducer(CaseFieldsNames.OrganizationUnitId, { items: ous });
        notifierFieldsSetter.setOU(ouId || '');
    }); 
    notifierFieldsSetter.setOU(ouId || '');
  } */

  private resetNotifierFields(formFieldsSetter: NotifierFormFieldsSetter) {
    formFieldsSetter.setReportedBy('');
    formFieldsSetter.setPersonName('');
    formFieldsSetter.setPersonEmail('');
    formFieldsSetter.setPersonCellPhone('');
    formFieldsSetter.setPlace('');
    formFieldsSetter.setUserCode('');
    formFieldsSetter.setCostCenter('');
    formFieldsSetter.setRegion('');
    //this.changeRegion(formFieldsSetter, null, null, null);
  }

  private getCaseDataReducers(dataSource: CaseDataStore): CaseDataReducers {
    return this.сaseDataReducersFactory.createCaseDataReducers(dataSource);
  }

/*   private getFormOptionsHelpers() {
    const filters = this.caseDataHelpder.getFormCaseOptionsFilter(this.caseData, this.form);
    return this.caseService.getOptionsHelper(filters);
  } */
}

interface IOrganisationData {
  regionId?: number;
  departmentId?: number;
  ouId?: number;
}
