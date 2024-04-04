import { Component, ViewChild, ElementRef, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { forkJoin, Subject, of, throwError, interval, EMPTY } from 'rxjs';
import { switchMap, take, finalize, delay, catchError, map, takeWhile, defaultIfEmpty } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels, CaseFieldValueChangedEvent } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/data/header-event-data';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { CaseDataStore } from '../../logic/case-edit/case-data.store';
import { CaseEditDataHelper } from '../../logic/case-edit/case-editdata.helper';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { CaseLockApiService } from '../../services/api/case/case-lock-api.service';
import { CaseSaveService } from '../../services/case';
import {
  CaseSectionType, CaseAccessMode, CaseEditInputModel, CaseSectionInputModel,
  CaseLockModel, CaseAction, CaseActionDataType, CaseFileModel, ExCaseFileModel
} from '../../models';
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
import { untilDestroyed } from 'ngx-take-until-destroy';
import { isNumeric } from 'rxjs/internal/util/isNumeric';
import { PerfomersService } from 'src/app/services/case-organization/perfomers-service';
import { ExtendedCaseValidateFormData } from 'src/app/models/extendedCase/extendedCaseValidateForm.model';


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
    private oUsService: OUsService,
    private caseFileApiService: CaseFilesApiService,
    private performersService: PerfomersService) {
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
  caseData: CaseEditInputModel;

  isNewCase = false;
  private caseId = 0;
  private templateId = 0;
  private templateCid = 0;
  private caseSections: CaseSectionInputModel[];
  private ownsLock = true;
  private caseLock: CaseLockModel = null;
  private isClosing = false;
  private extendedCaseValidation$ = new Subject<boolean>();
  private extendedCaseValidationObserver = this.extendedCaseValidation$.asObservable();
  private extendedCaseSave$ = new Subject<boolean>();
  private extendedCaseSaveObserver = this.extendedCaseSave$.asObservable();
  private sectionHeadersInfo: { key: CaseSectionType, value: string }[] = [];
  private caseTabName = 'case-tab';

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
          untilDestroyed(this),
          catchError((e) => throwError(e)),
        );

    caseData$.pipe(
      map((caseData) => {
        const lock = caseData[2];
        this.caseLock = lock;
        this.caseSections = caseData[1];
        const options = caseData[0];
        this.dataSource = new CaseDataStore(options, this.caseData.customerId);

        this.initLock();
        this.processCaseData();
        this.processSectionHeader();
        return caseData;
      }),
      take(1),
      finalize(() => this.isLoaded = true),
      catchError((e) => throwError(e))
    ).subscribe((caseData) => {
      this.loadWorkflows(caseId, this.caseData.customerId);
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
      take(1),
      untilDestroyed(this),
    ).subscribe(() => this.router.navigate([url]));
  }

  getSectionHeader(type: CaseSectionType): string {
    if (this.caseSections == null) { return ''; }
    return this.caseSections.find(s => s.type == type).header;
  }

  getSectionHeaderInfoText(type: CaseSectionType): string {
    if (!this.hasSectionHeaderInfo(type)) { return ''; }
    return this.sectionHeadersInfo.find(s => s.key == type).value;
  }

  hasSectionHeaderInfo(type: CaseSectionType): boolean {
    if (this.sectionHeadersInfo == null) {
      return false;
    }
    const section = this.sectionHeadersInfo.find(s => s.key == type);
    return section.value && section.value.length !== 0;
  }

  isSectionOpen(type: CaseSectionType) {
    if (this.caseSections == null) { return null; }
    return this.caseSections.find(s => s.type == type).isEditCollapsed ? null : true;
  }

  saveCase(reload: boolean = false) {

    let finishingType = 0;    

    if (!isNaN(this.form.getValue(CaseFieldsNames.ClosingReason))
        && this.form.getValue(CaseFieldsNames.ClosingReason) !== null
        && this.form.getValue(CaseFieldsNames.ClosingReason) !== undefined) {
      finishingType = this.form.getValue(CaseFieldsNames.ClosingReason);
    }

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
          return this.saveExtendedCase(false, finishingType).pipe(
            take(1),
            switchMap((isEcSaved: boolean) => {
              if (isEcSaved) {
                this.isLoaded = false;
                this.isEcLoaded = false;
                this.currentTab = TabNames.Case;
                return this.caseSaveService.saveCase(this.form, this.caseData).pipe(
                  take(1),
                  map((caseId: number) => {
                    this.caseId = caseId;
                    return true;
                  }),
                  catchError((e) => {
                    const errormessage = this.translateService.instant(e.error.message);
                    this.alertService.showMessage(errormessage, AlertType.Error, 3);
                    return EMPTY.pipe(defaultIfEmpty(false));
                  })
                );
              } else {
                return EMPTY.pipe(defaultIfEmpty(false));
              }
            }),
            untilDestroyed(this), 
            catchError((e) => {
              const errormessage = this.translateService.instant(e.error.message);
              this.alertService.showMessage(errormessage, AlertType.Error, 3);
              return EMPTY.pipe(defaultIfEmpty(false));
            })
          );
        }
        const errormessage = this.translateService.instant('Fyll i obligatoriska fält.');
        this.alertService.showMessage(errormessage, AlertType.Error, 3);
        return EMPTY.pipe(defaultIfEmpty(false));
      }),
      untilDestroyed(this),
      catchError((e) => throwError(e))
    ).subscribe((isSaved: boolean) => {
      console.log('isSaved', isSaved);
      if (isSaved) {
        if (reload) {
          if (this.caseData.id == 0) { // New case route to saved case
            this.router.navigate(['/case', this.caseId]);
          } else {
            this.ngOnInit();
          }
        } else {
          this.goToCases();
        }
      }
      else{
        this.loadCaseData(this.caseId);
      }
    });
    this.syncExtendedCaseValues();
    this.validateExtendedCase(false, finishingType);

  }

  cleanTempFiles(caseId: number) {
    this.caseFileService.deleteTemplFiles(caseId, this.dataSource.currentCaseCustomerId$.value).pipe(
      take(1),
      untilDestroyed(this)
    ).subscribe();
  }

  onClickWorkflow(id: number) {

    this.caseTemplateService.loadTemplate(id)
      .pipe(
        untilDestroyed(this)
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
      const getWhiteList = this.caseFileApiService.getFileUploadWhiteList();
      forkJoin([getOU(ouId), getWhiteList]).pipe(
        take(1),
        untilDestroyed(this)
      ).subscribe(data => {
        const ou = data[0];
        const whiteList = data[1] || [];
        const ouParentId = ou != null ? ou.parentId : null;
        this.extendedCase.nativeElement.loadForm = {
          formParameters: {
            formId: caseData.extendedCaseData.extendedCaseFormId,
            languageId: userData.currentData.selectedLanguageId,
            extendedCaseGuid: caseData.extendedCaseData.extendedCaseGuid,
            caseId: caseData.id,
            caseNumber: caseData.caseNumber.toString(),
            caseGuid: caseData.caseGuid,
            applicationType: 'helpdesk',
            isCaseLocked: this.isLocked,
            isMobile: true,
            // currentUser: userData.currentData.EmployeeNumber, not used in helpdesk
            userGuid: userData.currentData.userGuid,
            userRole: userData.currentData.userRole,
            caseStatus: this.caseDataHelpder.getField(caseData, CaseFieldsNames.StateSecondaryId).value || '',
            customerId: caseData.customerId,
            whiteFilesList: whiteList,
            maxFileSize: 36700160
          },
          caseValues: { // EC uses strict comparision of values. so if value here is number, but in datasource is string - no item is selected
            // keep all ids as a string
            administrator_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.PerformerUserId).value },
            reportedby: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.ReportedBy).value },
            persons_name: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonName).value },
            persons_phone: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonPhone).value },
            usercode: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.UserCode).value },
            region_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.RegionId).value },
            department_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.DepartmentId).value },
            ou_id_1: { Value: isNumeric(ouParentId) ? ouParentId.toString() : ouParentId },
            ou_id_2: { Value: isNumeric(ouId) ? ouId.toString() : ouId },
            productarea_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.ProductAreaId).value },
            status_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.StatusId).value },
            subStatus_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.StateSecondaryId).value },
            plandate: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PlanDate).value },
            watchdate: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.WatchDate).value },
            priority_id: { Value: this.caseDataHelpder.getIdField(caseData, CaseFieldsNames.PriorityId).value },
            log_textinternal: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Log_InternalText).value },
            case_relation_type: { Value: this.getCaseRelationType(caseData) },
            persons_email: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonEmail).value },
            persons_cellphone: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.PersonCellPhone).value },
            place: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Place).value },
            costcentre: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.CostCentre).value },
            caption: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.Caption).value },
            inventorytype: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.ComputerTypeId).value },
            inventorylocation: { Value: this.caseDataHelpder.getField(caseData, CaseFieldsNames.InventoryLocation).value },
            case_files: { Value: JSON.stringify((this.caseFiles || []).map(f => new ExCaseFileModel(f.fileId, f.fileName))) }
          }
        };
        if (caseData.caseSolution &&
          caseData.caseSolution.defaultTab != null &&
          caseData.caseSolution.defaultTab !== this.caseTabName) {
          this.tabClick(this.tabNames.ExtendedCase);
        }
      });
    }
  }

  private saveExtendedCase(isOnNext: boolean, finishingType = 0) {
    if (this.caseData.extendedCaseData == null) {
      return EMPTY.pipe(defaultIfEmpty(true));
    }
    let validateFormdata: ExtendedCaseValidateFormData = {state: isOnNext, finishingType: finishingType};
    this.extendedCase.nativeElement.saveForm = validateFormdata;
    return this.extendedCaseSaveObserver;
  }

  private syncExtendedCaseValues() {

    if (this.caseData.extendedCaseData == null) {
      return;
    }
    else {
      if (!this.isEcLoaded) {
        return;
      }

      const values = this.extendedCase.nativeElement.getCaseValues;

      if (!values) {
        return;
      }

      if (values.administrator_id != null) {
        this.form.setSafe(CaseFieldsNames.PerformerUserId, values.administrator_id.Value);
        if(values.administrator_id !== undefined) {
          this.performersService.handlePerformerEmail(values.administrator_id.Value);
        }
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
      if (values.inventorytype != null) {
        this.form.setSafe(CaseFieldsNames.ComputerTypeId, values.inventorytype.Value);
      }
      if (values.inventorylocation != null) {
        this.form.setSafe(CaseFieldsNames.InventoryLocation, values.inventorylocation.Value);
      }
    }
  }
  // var finishingType = parseInt(document.getElementById("CaseLog_FinishingType").value);

  // if (isNaN(finishingType)) {
  //     finishingType = 0;
  // }

  private validateExtendedCase(isOnNext: boolean, finishingType = 0) {
    if (this.caseData.extendedCaseData == null) {
      this.extendedCaseValidation$.next(true);
    } else {
      let validateFormdata: ExtendedCaseValidateFormData = {state: isOnNext, finishingType: finishingType};
      this.extendedCase.nativeElement.validateForm = validateFormdata;
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

  goToCases() {
    this.navigate('/casesoverview/');
  }

  private loadWorkflows(caseId: number, customerId: number) {
    this.caseWorkflowsService.getWorkflows(caseId, customerId)
      .subscribe(workflows => {
        this.dataSource.workflowsStore$.next(workflows);
      });
  }

  private translateMessages() {
    Object.keys(CaseFieldsDefaultErrorMessages)
      .forEach(key => {
        if (CaseFieldsDefaultErrorMessages[key] !== null) {
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
        untilDestroyed(this),
        catchError((e) => throwError(e)),
      );

    caseData$.pipe(
      map((caseData) => {
        this.caseSections = caseData[1];
        const options = caseData[0];
        this.dataSource = new CaseDataStore(options, this.caseData.customerId);

        this.initLock();
        this.processCaseData();
        this.processSectionHeader();
        return caseData;
      }),
      take(1),
      finalize(() => this.isLoaded = true),
      untilDestroyed(this),
      catchError((e) => throwError(e))
    ).subscribe((caseData) => {
      this.loadWorkflows(null, this.caseData.customerId);
      this.loadExtendedCase(this.caseData);
    });
  }

  private subscribeEvents() {
    // drop down value changed
    this.commService.listen(Channels.CaseFieldValueChanged).pipe(
      untilDestroyed(this)
    ).subscribe((v: CaseFieldValueChangedEvent) => { this.caseEditLogic.runUpdates(v, this.dataSource, this.caseData, this.form) });
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

    if (this.caseData.caseSolution &&
      this.caseData.caseSolution.defaultTab != null &&
      this.caseData.caseSolution.defaultTab !== '' &&
      this.caseData.caseSolution.defaultTab !== this.caseTabName) {
      this.tabClick(this.tabNames.ExtendedCase);
    }
  }

  private getSectionInfo(type?: CaseSectionType): string {
    const defaultValue = '';
    if (this.caseSections == null) {
      return defaultValue;
    }
    const section = this.caseSections.find(s => s.type == type);
    if (!section.caseSectionFields || !section.caseSectionFields.length) {
      return defaultValue;
    }

    return this.caseDataHelpder.getSectionInfoFields(section, this.dataSource, this.caseData);
  }

  private processSectionHeader() {
    const headersToProcess = new Array<CaseSectionType>(CaseSectionType.Initiator,
      CaseSectionType.Regarding,
      CaseSectionType.CaseInfo,
      CaseSectionType.ComputerInfo,
      CaseSectionType.CaseManagement);
    this.sectionHeadersInfo = [];
    headersToProcess.forEach(t => {
      this.sectionHeadersInfo.push({ key: t, value: this.getSectionInfo(t) });
    });
  }

  private loadCaseActions() {
    this.caseService.getCaseActions(this.caseId, this.customerId).pipe(
      take(1),
      untilDestroyed(this),
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
          takeWhile(() => this.ownsLock),
          untilDestroyed(this)
        ).subscribe(_ => {
          this.caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue, this.customerId).pipe(
            take(1),
            untilDestroyed(this)
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
    const currentUser = this.authStateService.getUser();
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
        this.caseLockApiService.unLockCase(this.caseId, this.caseLock.lockGuid, this.customerId).pipe(
          take(1)
        ).subscribe();
      }
    }

    // shall we do extra checks?
    this.alertService.clearMessages();
    this.extendedCaseValidation$.complete();
    this.extendedCaseSave$.complete();

    this.commService.publish(Channels.Header, new HeaderEventData(true));
  }
}

