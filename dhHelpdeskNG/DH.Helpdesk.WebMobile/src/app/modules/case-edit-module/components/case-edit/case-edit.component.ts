import { Component, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { forkJoin, Subject, of, throwError, interval, EMPTY } from 'rxjs';
import { switchMap, take, finalize, delay, catchError, map, takeUntil, takeWhile, defaultIfEmpty } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels, CaseFieldValueChangedEvent, } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/data/header-event-data';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { CaseDataStore } from '../../logic/case-edit/case-data.store';
import { CaseEditDataHelper } from '../../logic/case-edit/case-editdata.helper';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { CaseLockApiService } from '../../services/api/case/case-lock-api.service';
import { CaseSaveService } from '../../services/case';
import { CaseSectionType, CaseAccessMode, CaseEditInputModel, CaseSectionInputModel,
   CaseLockModel, CaseAction, CaseActionDataType, CaseFileModel, ICaseField } from '../../models';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';
import { CaseFilesApiService } from '../../services/api/case/case-files-api.service';
import { NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';
import { MbscFormOptions } from '@mobiscroll/angular';
import { CaseFieldsDefaultErrorMessages } from '../../logic/constants/case-fields.constants';
import { CaseFormGroupBuilder } from 'src/app/modules/shared-module/models/forms/case-form-group-builder';
import { CaseWorkflowsApiService } from '../../services/api/workflow-api.service';
import { CaseEditLogic } from '../../logic/case-edit/case-edit.logic';
import { CaseTemplateFullModel } from 'src/app/models/caseTemplate/case-template-full.model';
import { CaseTemplateService } from 'src/app/services/case-organization/case-template.service';
import { FinalActionEnum } from 'src/app/modules/shared-module/constants/finalAction.enum';
import { LocalStorageService } from 'src/app/services/local-storage';
import { OUsService } from 'src/app/services/case-organization/ous-service';
import { TabNames } from '../../constants/tab-names';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';
import { DateTime } from 'luxon';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent {

  @ViewChild('extendedCase', { static: false }) extendedCase: ElementRef; //ElementRefNgElement & WithProperties<{loadForm: any}>;

    constructor(private route: ActivatedRoute,
                private caseService: CaseService,
                private router: Router,
                private caseLockApiService: CaseLockApiService,
                private authStateService: AuthenticationStateService,
                private caseDataHelpder: CaseEditDataHelper,
                private alertService: AlertsService,
                private caseSaveService: CaseSaveService,
                private commService: CommunicationService,
                private translateService: TranslateService,
                private caseFileService: CaseFilesApiService,
                private caseWorkflowsService: CaseWorkflowsApiService,
                private caseEditLogic: CaseEditLogic,
                private caseTemplateService: CaseTemplateService,
                private localStorageService: LocalStorageService,
                private oUsService: OUsService) {
      // read route params
      if (this.route.snapshot.paramMap.has('id')) {
          this.isNewCase = false;
          this.caseId = +this.route.snapshot.paramMap.get('id');
      } else if (this.route.snapshot.paramMap.has('templateId')) {
          this.isNewCase = true;
          this.templateId = +this.route.snapshot.paramMap.get('templateId');
          this.templateCid = +this.route.snapshot.paramMap.get('templateCid');
      } else {
        throw new Error('Invalid parameters');
      }

      // subscribe global events
      this.subscribeEvents();
    }

    get isLocked() {
      return this.caseLock && this.caseLock.isLocked;
    }

    get accessMode() {
      let accessMode = CaseAccessMode.NoAccess;
      if (this.caseData) {
          if (this.caseData.editMode === CaseAccessMode.NoAccess) {
            accessMode = CaseAccessMode.NoAccess;
          } else {
            if (this.isLocked) {
              accessMode = CaseAccessMode.ReadOnly;
            } else {
              accessMode = this.caseData.editMode;
            }
          }
      }
      return accessMode;
    }

    get customerId() {
      return this.dataSource.currentCaseCustomerId$.value;
    }

    public get canSave() {
      if (this.isNewCase) {
        return true;
      }
      return this.accessMode === CaseAccessMode.FullAccess;
    }

    isExtendedCaseInvalid = false;
    caseSectionTypes = CaseSectionType;
    tabNames = TabNames;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode;
    dataSource: CaseDataStore;
    isLoaded = false;
    isEcLoaded = false;
    form: CaseFormGroup;
    caseKey: string;
    caseFiles: CaseFileModel[] = [];

    titleTabsSettings = {
      display: 'top'
    };

    caseTabsSettings = {
      display: 'top',
      layout: 'fixed',
      theme: 'mobiscroll'
    };

    formOptions: MbscFormOptions = {
      onInit: () => {
      }
    };

    currentTab = TabNames.Case;
    caseActions: CaseAction<CaseActionDataType>[] = [];
    notifierTypes = NotifierType;
    isCommunicationSectionVisible = false;

    private isNewCase = false;
    private caseId = 0;
    private templateId = 0;
    private templateCid = 0;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = true;
    private destroy$ = new Subject();
    private caseLock: CaseLockModel = null;
    private isClosing = false;
    private extendedCaseValidation$ = new Subject<boolean>();
    private extendedCaseValidationObserver = this.extendedCaseValidation$.asObservable();
    private extendedCaseSave$ = new Subject<boolean>();
    private extendedCaseSaveObserver = this.extendedCaseSave$.asObservable();

    ngOnInit() {
      this.commService.publish(Channels.Header, new HeaderEventData(false));

     if (this.caseId > 0) {
          // existing case
          this.loadCaseData(this.caseId);
      } else if (this.templateId > 0) {
          this.isNewCase = true;
          this.loadTemplate(this.templateId, this.templateCid);
      }

      this.translateMessages();
    }

   loadCaseData(caseId: number): any {
      this.isLoaded = false;
      const sessionId = this.authStateService.getUser().authData.sessionId;

      const caseData$ =
          this.caseService.getCaseData(caseId)
              .pipe(
                  take(1),
                  switchMap(data => {
                    this.caseData = data;
                    this.caseKey = this.caseData.id > 0 ? this.caseData.id.toString() : this.caseData.caseGuid.toString();
                    const filter = this.caseDataHelpder.getCaseOptionsFilter(this.caseData);
                    const options$ = this.caseService.getCaseOptions(filter, this.caseData.customerId);
                    const caseSections$ = this.caseService.getCaseSections(this.caseData.customerId);
                    const caseLock$ = this.caseLockApiService.acquireCaseLock(caseId, sessionId, this.caseData.customerId);

                    return forkJoin([options$, caseSections$, caseLock$]);
                  }),
                  catchError((e) => throwError(e)),
              );

      caseData$.pipe(
          take(1),
          finalize(() => this.isLoaded = true),
          catchError((e) => throwError(e))
      ).subscribe((caseData) => {
          const lock = caseData[2];
          this.caseLock = lock;
          this.caseSections = caseData[1];
          const options = caseData[0];
          this.dataSource = new CaseDataStore(options, this.caseData.customerId);

          this.initLock();
          this.processCaseData();
          this.loadWorkflows(caseId);
          this.loadExtendedCase(this.caseData);
      });
    }

    getCaseTitle(): string {
      return this.caseDataHelpder.getCaseTitle(this.caseData);
    }

    showField(name: string): boolean {
      return this.caseDataHelpder.hasField(this.caseData, name) && !this.caseDataHelpder.getField(this.caseData, name).isHidden;
    }

    hasSection(type: CaseSectionType): boolean {
      return this.caseDataHelpder.hasSection(this.caseData, type);
    }

    public navigate(url: string) {
      if (url == null) { return; }
      of(true).pipe(
        delay(200),
        take(1)
      ).subscribe(() => this.router.navigate([url]));
    }

    getSectionHeader(type: CaseSectionType): string {
        if (this.caseSections == null) { return ''; }
        return this.caseSections.find(s => s.type == type).header;
    }

    getSectionInfo(type?: CaseSectionType): string {
      const defaultValue = '&nbsp';
      if (this.caseSections == null) {
        return defaultValue;
      }
      const section = this.caseSections.find(s => s.type == type);
      if (!section.caseSectionFields || !section.caseSectionFields.length) {
        return defaultValue;
      }

      return this.getSectionInfoFields(section);
    }

    private getSectionInfoFields(section: CaseSectionInputModel): string {
      const emptyValue = null;
      const getFromList = (value: any, list: OptionItem[]) => {
        const item = list.find(o => o.value == value);
        return item ? item.text : emptyValue;
      };

      const getDate = (value: string, isShortData: boolean = false) => {
        return DateUtil.formatDate(value, isShortData ? DateTime.DATE_SHORT : null);
      };

      const initiatorFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.RegionId: {
            return getFromList(field.value, this.dataSource.regionsStore$.value);
          }
          case CaseFieldsNames.DepartmentId: {
            return getFromList(field.value, this.dataSource.departmentsStore$.value);
          }
          case CaseFieldsNames.OrganizationUnitId: {
            return getFromList(field.value, this.dataSource.oUsStore$.value);
          }
        }
        return field.value;
      };

      const regardingFields = (name: string, field: ICaseField<any>) => {
        switch (name) {
          case CaseFieldsNames.RegionId: {
            return getFromList(field.value, this.dataSource.regionsStore$.value);
          }
          case CaseFieldsNames.DepartmentId: {
            return getFromList(field.value, this.dataSource.isAboutDepartmentsStore$.value);
          }
          case CaseFieldsNames.OrganizationUnitId: {
            return getFromList(field.value, this.dataSource.isAboutOUsStore$.value);
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
            return getFromList(field.value, this.dataSource.customerRegistrationSourcesStore$.value);
          }
          case CaseFieldsNames.CaseTypeId: {
            return getFromList(field.value, this.dataSource.caseTypesStore$.value);
          }
          case CaseFieldsNames.ProductAreaId: {
            return getFromList(field.value, this.dataSource.productAreasStore$.value);
          }
          case CaseFieldsNames.SystemId: {
            return getFromList(field.value, this.dataSource.systemsStore$.value);
          }
          case CaseFieldsNames.UrgencyId: {
            return getFromList(field.value, this.dataSource.urgenciesStore$.value);
          }
          case CaseFieldsNames.ImpactId: {
            return getFromList(field.value, this.dataSource.impactsStore$.value);
          }
          case CaseFieldsNames.CategoryId: {
            return getFromList(field.value, this.dataSource.categoriesStore$.value);
          }
          case CaseFieldsNames.SupplierId: {
            return getFromList(field.value, this.dataSource.suppliersStore$.value);
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
            return getFromList(field.value, this.dataSource.workingGroupsStore$.value);
          }
          case CaseFieldsNames.CaseResponsibleUserId: {
            return getFromList(field.value, this.dataSource.responsibleUsersStore$.value);
          }
          case CaseFieldsNames.PerformerUserId: {
            return getFromList(field.value, this.dataSource.performersStore$.value);
          }
          case CaseFieldsNames.PriorityId: {
            return getFromList(field.value, this.dataSource.prioritiesStore$.value);
          }
          case CaseFieldsNames.StatusId: {
            return getFromList(field.value, this.dataSource.statusesStore$.value);
          }
          case CaseFieldsNames.StateSecondaryId: {
            return getFromList(field.value, this.dataSource.stateSecondariesStore$.value);
          }
          case CaseFieldsNames.Project: {
            return getFromList(field.value, this.dataSource.projectsStore$.value);
          }
          case CaseFieldsNames.Problem: {
            return getFromList(field.value, this.dataSource.problemsStore$.value);
          }
          case CaseFieldsNames.CausingPart: {
            return getFromList(field.value, this.dataSource.causingPartsStore$.value);
          }
          case CaseFieldsNames.Change: {
            return getFromList(field.value, this.dataSource.changesStore$.value);
          }
          case CaseFieldsNames.PlanDate:
          case CaseFieldsNames.WatchDate: {
              return getDate(field.value, true);
          }
          case CaseFieldsNames.SolutionRate: {
            return getFromList(field.value, this.dataSource.solutionsRatesStore$.value);
          }
        }
        return field.value;
      };

      return section.caseSectionFields.map(name => {
        if (this.caseDataHelpder.hasField(this.caseData, name)) {
           const field = this.caseDataHelpder.getField(this.caseData, name);
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

    isSectionOpen(type: CaseSectionType) {
        if (this.caseSections == null) { return null; }
        return this.caseSections.find(s => s.type == type).isEditCollapsed ? null : true;
    }

    saveCase(reload: boolean = false) {
      if (!this.canSave) { return; }
      if (this.caseData.extendedCaseData != null && !this.isEcLoaded) {
        return;
      }
      this.extendedCaseValidationObserver.pipe(
        take(1),
        switchMap((isEcValid: boolean) => {
          this.isExtendedCaseInvalid = !isEcValid;
          this.form.submit();
          if (this.form.valid && isEcValid) {
            return this.saveExtendedCase(false).pipe(
              take(1),
              switchMap((isEcSaved: boolean) => {
                if (isEcSaved) {
                  this.isLoaded = false;
                  this.isEcLoaded = false;
                  this.currentTab = TabNames.Case;
                  return this.caseSaveService.saveCase(this.form, this.caseData).pipe(
                      take(1),
                      map(() => true),
                      catchError((e) => throwError(e))
                  );
                } else {
                  return EMPTY.pipe(defaultIfEmpty(false));
                }
              }),
              catchError((e) => {
                this.alertService.showMessage('Extended Case save was not succeed!', AlertType.Error, 3);
                return throwError(e);
              })
            );
          }
          const errormessage = this.translateService.instant('Fyll i obligatoriska fält.');
          this.alertService.showMessage(errormessage, AlertType.Error, 3);
          return EMPTY.pipe(defaultIfEmpty(false));
        }),
        catchError((e) => throwError(e))
      ).subscribe((isSaved: boolean) => {
        if (isSaved) {
          reload ? this.ngOnInit() : this.goToCases();
        }
      });
      this.syncExtendedCaseValues();
      this.validateExtendedCase(false);
    }

    cleanTempFiles(caseId: number) {
      this.caseFileService.deleteTemplFiles(caseId, this.dataSource.currentCaseCustomerId$.value).pipe(
        take(1)
      ).subscribe();
    }

    onClickWorkflow(id: number) {
      this.caseTemplateService.loadTemplate(id)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((data: CaseTemplateFullModel) => {
        const finalAction = this.caseTemplateService.applyWorkflow(data, this.form);
        if (finalAction == FinalActionEnum.Save ||
          finalAction == FinalActionEnum.SaveAndClose) {
            if (this.canSave) {
              this.saveCase(finalAction == FinalActionEnum.Save);
            }
          }
      });
    }

    onExtendedCaseSaved(event: any) {
      this.extendedCaseSave$.next(event.detail.isSuccess);
    }

    onExtendedCaseValidation(event: any) {
      this.extendedCaseValidation$.next(event.detail.data == null);
    }

    onExtendedCaseLoadComplete(event: any) {
      if (event.detail.isSuccess) {
        this.isEcLoaded = true;
      }
    }

    tabClick(tab: TabNames) {
      if (tab == TabNames.Case && this.currentTab == TabNames.ExtendedCase) {
        this.syncExtendedCaseValues();
      }
      this.currentTab = tab;
    }

    ///////////////////////////////////////////////////////// Private section

    private loadExtendedCase(caseData: CaseEditInputModel) {
      if (caseData.extendedCaseData != null) {
        const userData = this.localStorageService.getCurrentUser();
        const ouId = this.caseDataHelpder.getField(caseData, CaseFieldsNames.OrganizationUnitId).value;
        const empty$ = () => EMPTY.pipe(defaultIfEmpty(null));
        const getOU = (_ouId) => _ouId != null ? this.oUsService.getOU(_ouId, caseData.customerId) : empty$();
        getOU(ouId).pipe(
          take(1)
        ).subscribe(ou => {
          const ouParentId = ou != null ? ou.parentId : null;
          this.extendedCase.nativeElement.loadForm = {
            formParameters: {
              formId: caseData.extendedCaseData.extendedCaseFormId,
              languageId: userData.currentData.selectedLanguageId,
              extendedCaseGuid: caseData.extendedCaseData.extendedCaseGuid,
              caseId: caseData.id,
              applicationType: 'helpdesk',
              isCaseLocked: this.isLocked,
              // currentUser: userData.currentData.EmployeeNumber, not used in helpdesk
              userGuid: userData.currentData.userGuid,
              userRole: userData.currentData.userRole,
              caseStatus: this.caseDataHelpder.getField(caseData, CaseFieldsNames.StateSecondaryId).value || '',
              customerId: caseData.customerId
            },
            caseValues: {
              administrator_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PerformerUserId).value },
              reportedby: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.ReportedBy).value },
              persons_name: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonName).value },
              persons_phone: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonPhone).value },
              usercode: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.UserCode).value },
              region_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.RegionId).value },
              department_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.DepartmentId).value },
              ou_id_1: { Value: ouParentId },
              ou_id_2: { Value: ouId },
              productarea_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.ProductAreaId).value },
              status_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.StatusId).value },
              subStatus_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.StateSecondaryId).value },
              plandate: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PlanDate).value },
              watchdate: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.WatchDate).value },
              priority_id: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PriorityId).value },
              log_textinternal: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Log_InternalText).value },
              case_relation_type: { Value: this.getCaseRelationType(caseData) },
              persons_email: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonEmail).value },
              persons_cellphone: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonCellPhone).value },
              place: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Place).value },
              costcentre: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.CostCentre).value },
              caption: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Caption).value }
            }
          };
        });
      }
    }

    private saveExtendedCase(isOnNext: boolean) {
      if (this.caseData.extendedCaseData == null) {
        return EMPTY.pipe(defaultIfEmpty(true));
      }

      this.extendedCase.nativeElement.saveForm = isOnNext;
      return this.extendedCaseSaveObserver;
    }

    private syncExtendedCaseValues() {
      if (!this.isEcLoaded || this.caseData.extendedCaseData == null) {
        return;
      }
      const values = this.extendedCase.nativeElement.getCaseValues;
      if (!values) {
        return;
      }

      if (values.administrator_id != null) {
        this.form.setSafe(CaseFieldsNames.PerformerUserId, values.administrator_id.Value);
      }
      if (values.reportedby != null) {
        this.form.setSafe(CaseFieldsNames.ReportedBy, values.reportedby.Value);
      }
      if (values.persons_name != null) {
        this.form.setSafe(CaseFieldsNames.PersonName, values.persons_name.Value);
      }
      if (values.persons_phone != null) {
        this.form.setSafe(CaseFieldsNames.PersonPhone, values.persons_phone.Value);
      }
      if (values.usercode != null) {
        this.form.setSafe(CaseFieldsNames.UserCode, values.usercode.Value);
      }
      if (values.log_textinternal != null) {
        this.form.setSafe(CaseFieldsNames.Log_InternalText, values.log_textinternal.Value);
      }
      if (values.region_id != null) {
        this.form.setSafe(CaseFieldsNames.RegionId, values.region_id.Value);
      }
      if (values.department_id != null) {
        this.form.setSafe(CaseFieldsNames.DepartmentId, values.department_id.Value);
      }

      let ouValue = values.ou_id_1 != null ? values.ou_id_1.Value : null;
      if (values.ou_id_2 != null) {
        ouValue = values.ou_id_2.Value;
      }
      if (ouValue != null) {
        this.form.setSafe(CaseFieldsNames.OrganizationUnitId, ouValue);
      }

      if (values.priority_id != null) {
        this.form.setSafe(CaseFieldsNames.PriorityId, values.priority_id.Value);
      }

      if (values.priority_id != null) {
        this.form.setSafe(CaseFieldsNames.PriorityId, values.priority_id.Value);
      }
      if (values.productarea_id != null) {
        this.form.setSafe(CaseFieldsNames.ProductAreaId, values.productarea_id.Value);
      }
      if (values.status_id != null) {
        this.form.setSafe(CaseFieldsNames.StatusId, values.status_id.Value);
      }
      if (values.subStatus_id != null) {
        this.form.setSafe(CaseFieldsNames.StateSecondaryId, values.subStatus_id.Value);
      }
      if (values.plandate != null) {
        this.form.setSafe(CaseFieldsNames.PlanDate, values.plandate.Value);
      }
      if (values.watchdate != null) {
        this.form.setSafe(CaseFieldsNames.WatchDate, values.watchdate.Value);
      }
      if (values.persons_email != null) {
        this.form.setSafe(CaseFieldsNames.PersonEmail, values.persons_email.Value);
      }
      if (values.persons_cellphone != null) {
        this.form.setSafe(CaseFieldsNames.PersonCellPhone, values.persons_cellphone.Value);
      }
      if (values.costcentre != null) {
        this.form.setSafe(CaseFieldsNames.CostCentre, values.costcentre.Value);
      }
      if (values.caption != null) {
        this.form.setSafe(CaseFieldsNames.Caption, values.caption.Value);
      }
    }

    private validateExtendedCase(isOnNext: boolean) {
      if (this.caseData.extendedCaseData == null) {
        this.extendedCaseValidation$.next(true);
      } else {
        this.extendedCase.nativeElement.validateForm = isOnNext;
      }
    }

    private getCaseRelationType(caseData: CaseEditInputModel) {
      if (caseData.childCasesIds != null && caseData.childCasesIds.length > 0) {
        return 'Parent';
      }
      if (caseData.parentCaseId != null && caseData.parentCaseId > 0) {
        return 'Child';
      }
      return '';
    }

    private goToCases() {
      this.navigate('/casesoverview/');
    }

    private loadWorkflows(caseId: number) {
      this.caseWorkflowsService.getWorkflows(caseId, this.customerId)
      .subscribe(workflows => {
        this.dataSource.workflowsStore$.next(workflows);
      });
    }

    private translateMessages() {
      Object.keys(CaseFieldsDefaultErrorMessages)
        .forEach(key => {
          if (CaseFieldsDefaultErrorMessages[key] !==  null) {
            const translation = this.translateService.instant(CaseFieldsDefaultErrorMessages[key]);
            CaseFieldsDefaultErrorMessages[key] = translation;
          }
        });
    }

    private loadTemplate(templateId: number, customerId: number) {
      const caseData$ =
        this.caseService.getTemplateData(templateId, customerId).pipe(
          take(1),
          switchMap(data => {
            this.caseData = data;
            this.caseKey = this.caseData.caseGuid.toString();
            const filter = this.caseDataHelpder.getCaseOptionsFilter(this.caseData);
            const caseSections$ = this.caseService.getCaseSections(this.caseData.customerId);
            const options$ = this.caseService.getCaseOptions(filter, this.caseData.customerId);

            return forkJoin([options$, caseSections$]);
          }),
          catchError((e) => throwError(e)),
      );

      caseData$.pipe(
          take(1),
          finalize(() => this.isLoaded = true),
          catchError((e) => throwError(e))
      ).subscribe((caseData) => {
          this.caseSections = caseData[1];
          const options = caseData[0];
          this.dataSource = new CaseDataStore(options, this.caseData.customerId);

          this.initLock();
          this.processCaseData();
          this.loadExtendedCase(this.caseData);
      });
    }

    private subscribeEvents() {
      // drop down value changed
      this.commService.listen(Channels.CaseFieldValueChanged).pipe(
        takeUntil(this.destroy$)
      ).subscribe((v: CaseFieldValueChangedEvent) => this.caseEditLogic.runUpdates(v, this.dataSource, this.caseData, this.form));
    }

    private processCaseData() {
      // create form
      const fb = new CaseFormGroupBuilder(this.translateService);
      this.form = fb.createFormGroup(this.caseData.fields, this.canSave);
      this.form.setCommService(this.commService);

      // run only for existing case
      if (this.caseId > 0) {
          this.loadCaseActions();
          this.cleanTempFiles(this.caseId);
      }

      // set up case visibility settings
      if (this.caseData) {
          this.isCommunicationSectionVisible =
            this.showField(CaseFieldsNames.Log_ExternalText) ||
            this.showField(CaseFieldsNames.Log_InternalText) ||
            this.showField(CaseFieldsNames.Log_FileName);
      }

      // get existing files
      const filesField = this.caseEditLogic.getField(this.caseData, CaseFieldsNames.Filename);
      if (filesField && filesField.value) {
        const files = filesField.value as Array<any>;
        if (files && files.length) {
          this.caseFiles = files.map(f => new CaseFileModel(f.id, f.fileName));
        } else {
          this.caseFiles = [];
        }
      }
    }

    private loadCaseActions() {
      this.caseService.getCaseActions(this.caseId, this.customerId).pipe(
        take(1),
        catchError((e) => throwError(e))
      ).subscribe(caseActions => {
        this.caseActions = caseActions;
      });
    }

    private initLock() {
      if (this.caseId > 0) {
        // set flag if we own the lock
        this.ownsLock = !this.caseLock.isLocked;

        if (this.caseLock.isLocked) {
          this.showLockWarning();
        } else if (this.caseLock.timerInterval > 0) {
            // run extend case lock at specified interval
            interval(this.caseLock.timerInterval * 1000).pipe(
                  takeWhile(() => this.ownsLock)
              ).subscribe(_ => {
                this.caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue, this.customerId).pipe(
                  take(1)
                ).subscribe(res => {
                    if (!res) {
                      this.ownsLock = false;
                      this.caseLock.isLocked = true;

                      // TOOO: raise event

                      if (!this.isClosing) {
                        this.showLockWarning();
                      }
                    }
                });
              });
        }
      } else {
        this.caseLock = new CaseLockModel();
      }
    }

    private showLockWarning() {
      const currentUser =  this.authStateService.getUser();
      const notice =
        this.caseLock.isLocked && this.caseLock.userId === currentUser.id ?
            this.translateService.instant('OBS! Du har redan öppnat detta ärende i en annan session') :
            this.translateService.instant('OBS! Detta ärende är öppnat av') + ' ' + this.caseLock.userFullName;
      this.alertService.showMessage(notice, AlertType.Warning);
    }

    ngOnDestroy() {
      this.isClosing = true;

      // unlock the case if required
      if (this.caseId > 0) {
        if (this.ownsLock) {
          this.ownsLock = false;
          this.caseLockApiService.unLockCase(this.caseId, this.caseLock.lockGuid, this.customerId).subscribe();
        }
      }

      // shall we do extra checks?
      this.alertService.clearMessages();
      this.destroy$.next();
      this.destroy$.complete();
      this.extendedCaseValidation$.complete();
      this.extendedCaseSave$.complete();

      this.commService.publish(Channels.Header, new HeaderEventData(true));

  }
}

