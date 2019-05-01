import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { forkJoin, Subject, of, throwError, interval } from 'rxjs';
import { switchMap, take, finalize, delay, catchError, map, takeUntil, takeWhile } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels,
  FormValueChangedEvent, NotifierChangedEvent } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/data/header-event-data';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { WorkingGroupsService } from 'src/app/services/case-organization/workingGroups-service';
import { StateSecondariesService } from 'src/app/services/case-organization/stateSecondaries-service';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseDataStore } from '../../logic/case-edit/case-data.store';
import { CaseDataReducersFactory, CaseDataReducers } from '../../logic/case-edit/case-data.reducers';
import { CaseEditDataHelper } from '../../logic/case-edit/case-editdata.helper';
import { CaseFieldsNames, CasesSearchType } from 'src/app/modules/shared-module/constants';
import { CaseLockApiService } from '../../services/api/case/case-lock-api.service';
import { CaseSaveService } from '../../services/case';
import { CaseSectionType, CaseAccessMode, CaseEditInputModel, CaseSectionInputModel,
   CaseLockModel, CaseFieldModel, CaseAction, CaseActionDataType, ICaseField, CaseFileModel } from '../../models';
import { OptionItem, MultiLevelOptionItem } from 'src/app/modules/shared-module/models';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';
import { CaseWatchDateApiService } from '../../services/api/case/case-watchDate-api.service';
import { CaseFilesApiService } from '../../services/api/case/case-files-api.service';
import { NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { CaseTypesService } from 'src/app/services/case-organization/caseTypes-service';
import { CaseFormGroup } from 'src/app/modules/shared-module/models/forms';
import { MbscFormOptions } from '@mobiscroll/angular';
import { ProductAreasService } from 'src/app/services/case-organization/productAreas-service';
import { DateTime } from 'luxon';
import { CaseFieldsDefaultErrorMessages } from '../../logic/constants/case-fields.constants';
import { CaseFormGroupBuilder } from 'src/app/modules/shared-module/models/forms/case-form-group-builder';
import { NotifierFormFieldsSetter } from 'src/app/modules/shared-module/models/forms/notifier-form-fields-setter';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent {

    constructor(private route: ActivatedRoute,
                private caseService: CaseService,
                private router: Router,
                private caseLockApiService: CaseLockApiService,
                private authStateService: AuthenticationStateService,
                private caseDataHelpder: CaseEditDataHelper,
                private alertService: AlertsService,
                private caseSaveService: CaseSaveService,
                private commService: CommunicationService,
                private сaseDataReducersFactory: CaseDataReducersFactory,
                private workingGroupsService: WorkingGroupsService,
                private stateSecondariesService: StateSecondariesService,
                private caseTypesService: CaseTypesService,
                private caseWatchDateApiService: CaseWatchDateApiService,
                private translateService: TranslateService,
                private localStorage:  LocalStorageService,
                private caseFileService: CaseFilesApiService,
                private productAreasService: ProductAreasService) {
      // read route params
      if (this.route.snapshot.paramMap.has('id')) {
          this.isNewCase = false;
          this.caseId = +this.route.snapshot.paramMap.get('id');
      } else if (this.route.snapshot.paramMap.has('templateId')) {
          this.isNewCase = true;
          this.templateId = +this.route.snapshot.paramMap.get('templateId');
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

    public get canSave() {
      if (this.isNewCase) {
        return true;
      }

      return this.accessMode === CaseAccessMode.FullAccess;
    }
  @ViewChild('mainForm') mainForm: any; // MbscForm
    caseSectionTypes = CaseSectionType;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode;
    dataSource: CaseDataStore;
    isLoaded = false;
    form: CaseFormGroup;
    caseKey: string;
    caseFiles: CaseFileModel[] = null;

    titleTabsSettings = {
      display: 'top'
    };

    caseTabsSettings = {
      display: 'top',
      layout: 'fixed',
      theme: 'mobiscroll'
    };

    formOptions: MbscFormOptions = {
      onInit: (event, inst) => {
      }
    };

    currentTab = 'case_details';
    caseActions: CaseAction<CaseActionDataType>[] = [];
    notifierTypes = NotifierType;
    isCommunicationSectionVisible = false;

    private searchType: CasesSearchType = CasesSearchType.AllCases;
    private isNewCase = false;
    private caseId = 0;
    private templateId = 0;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = false;
    private destroy$ = new Subject();
    private caseLock: CaseLockModel = null;
    private isClosing = false;

    ngOnInit() {
      this.commService.publish(Channels.Header, new HeaderEventData(false));

     if (this.caseId > 0) {
          // existing case
          this.loadCaseData(this.caseId);
      } else if (this.templateId > 0) {
          this.isNewCase = true;
          this.loadTemplate(this.templateId);
      }

      this.translateMessages();
    }

   loadCaseData(caseId: number): any {
      this.isLoaded = false;
      const sessionId = this.authStateService.getUser().authData.sessionId;

      const caseLock$ = this.caseLockApiService.acquireCaseLock(caseId, sessionId);
      const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling

      // todo: apply search type (all, my cases)
      const caseData$ =
          this.caseService.getCaseData(caseId)
              .pipe(
                  take(1),
                  switchMap(data => {
                    this.ownsLock = false;
                    this.caseData = data;
                    this.caseKey = this.caseData.id > 0 ? this.caseData.id.toString() : this.caseData.caseGuid.toString();
                    const filter = this.caseDataHelpder.getCaseOptionsFilter(this.caseData);

                    return this.caseService.getCaseOptions(filter);
                  }),
                  catchError((e) => throwError(e)),
              );

      forkJoin(caseSections$, caseData$, caseLock$).pipe(
          take(1),
          finalize(() => this.isLoaded = true),
          catchError((e) => throwError(e))
      ).subscribe(([sectionData, options, caseLock]) => {
          this.caseLock = caseLock;
          this.caseSections = sectionData;
          this.dataSource = new CaseDataStore(options);

          this.initLock();
          this.processCaseData();
      });
    }

    getCaseTitle(): string {
      return this.caseDataHelpder.getCaseTitle(this.caseData);
    }

    showField(name: string): boolean {
      return this.caseDataHelpder.hasField(this.caseData, name) && !this.getField(name).isHidden;
    }

    hasSection(type: CaseSectionType): boolean {
      return this.caseDataHelpder.hasSection(this.caseData, type);
    }

    private getField(name: string): CaseFieldModel<any> {
      return this.caseDataHelpder.getField(this.caseData, name);
    }

    public navigate(url: string) {
      if (url == null) { return; }
      of(true).pipe(
        delay(200),
        map(() => this.router.navigate([url])),
        take(1)
      ).subscribe();
    }

    getSectionHeader(type: CaseSectionType): string {
        if (this.caseSections == null) { return ''; }
        return this.caseSections.find(s => s.type == type).header;
    }

    isSectionOpen(type: CaseSectionType) {
        if (this.caseSections == null) { return null; }
        return this.caseSections.find(s => s.type == type).isEditCollapsed ? null : true;
    }

    saveCase() {
      if (!this.canSave) { return; }
      this.form.submit();
      if (this.form.invalid) {
        // let invalidControls = this.form.findInvalidControls(); // debug info
        const errormessage = this.translateService.instant('Fyll i obligatoriska fält.');
        this.alertService.showMessage(errormessage, AlertType.Error, 3);
        return;
      }

      this.isLoaded = false;
      this.caseSaveService.saveCase(this.form, this.caseData).pipe(
          take(1),
          catchError((e) => throwError(e)) // TODO: handle here ? show error ?
      ).subscribe(() => {
          this.goToCases();
      });
    }

    goToCases() {
      const res = this.localStorage.getCaseSearchState();
      let searchType: string;
      if (res) {
        searchType = CasesSearchType[res.SearchType];
      } else {
        searchType = CasesSearchType[CasesSearchType.AllCases];
      }
      this.navigate('/casesoverview/' + searchType);
    }

    cleanTempFiles(caseId: number) {
      this.caseFileService.deleteTemplFiles(caseId).pipe(
        take(1)
      ).subscribe();
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

    private loadTemplate(templateId: number) {
      const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling

      const caseData$ =
        this.caseService.getTemplateData(templateId).pipe(
          take(1),
          switchMap(data => {
            // this.ownsLock = false; //TODO: check?
            this.caseData = data;
            this.caseKey = this.caseData.caseGuid.toString();
            const filter = this.caseDataHelpder.getCaseOptionsFilter(this.caseData);

            return this.caseService.getCaseOptions(filter);
          }),
          catchError((e) => throwError(e)),
      );

      forkJoin(caseSections$, caseData$).pipe(
          take(1),
          finalize(() => this.isLoaded = true),
          catchError((e) => throwError(e))
      ).subscribe(([sectionData, options]) => {
          this.caseSections = sectionData;
          this.dataSource = new CaseDataStore(options);

          this.initLock();
          this.processCaseData();
      });
    }

    private subscribeEvents() {

      // drop down value changed
      this.commService.listen(Channels.FormValueChanged).pipe(
        takeUntil(this.destroy$)
      ).subscribe((v: FormValueChangedEvent) => this.runUpdates(v));

      // Notifier changed
      this.commService.listen<NotifierChangedEvent>(Channels.NotifierChanged).pipe(
        takeUntil(this.destroy$)
      ).subscribe(data => this.processNotifierChanged(data));
    }

    private getCaseDataReducers(): CaseDataReducers {
      return this.сaseDataReducersFactory.createCaseDataReducers(this.dataSource);
    }

    private getFormOptionsHelpers() {
      const filters = this.caseDataHelpder.getFormCaseOptionsFilter(this.caseData, this.form);
      return this.caseService.getOptionsHelper(filters);
    }

    private runUpdates(v: FormValueChangedEvent) { // TODO: move to new class
      const reducer = this.getCaseDataReducers();
      const filters = this.caseDataHelpder.getFormCaseOptionsFilter(this.caseData, this.form);
      const optionsHelper = this.caseService.getOptionsHelper(filters);

      // NOTE: remember to update case data reducer when adding new fields
      switch (v.name) {
        case CaseFieldsNames.RegionId: {
          optionsHelper.getDepartments().pipe(
            take(1),
          ).subscribe((deps: OptionItem[]) => {
            reducer.caseDataReducer(CaseFieldsNames.DepartmentId, { items: deps });
            // clear org units
            reducer.caseDataReducer(CaseFieldsNames.OrganizationUnitId, { items: []});
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
            // clear org units
            reducer.caseDataReducer(CaseFieldsNames.IsAbout_OrganizationUnitId, { items: []});
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
                if (ct && ct.performerUserId != null && !this.getField(CaseFieldsNames.PerformerUserId).setByTemplate) {
                  if (!this.dataSource.performersStore$.value.some((e) => e.value === ct.performerUserId)) {
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
                  this.form.controls[CaseFieldsNames.WorkingGroupId].setValue(ct.workingGroupId);
                }
                if (ct && ct.performerUserId != null) {
                  this.form.controls[CaseFieldsNames.PerformerUserId].setValue(ct.performerUserId);
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

          if (!!v.value && this.form.contains(CaseFieldsNames.StateSecondaryId)) {
            this.workingGroupsService.getWorkingGroup(v.value).pipe(
              take(1)
            ).subscribe(wg => {
              if (wg && wg.stateSecondaryId != null) {
                this.form.controls[CaseFieldsNames.StateSecondaryId].setValue(wg.stateSecondaryId);
              }
            });
          }
          break;
        }
        case CaseFieldsNames.StateSecondaryId: {
          if (v.value) {
            this.stateSecondariesService.getStateSecondary(v.value)
            .pipe(
              take(1)
            ).subscribe(ss => {
              if (ss && ss.workingGroupId != null && this.form.contains(CaseFieldsNames.WorkingGroupId)) {
                this.form.controls[CaseFieldsNames.WorkingGroupId].setValue(ss.workingGroupId);
              }
              const departmentCtrl = this.form.controls[CaseFieldsNames.DepartmentId];
              if (ss.recalculateWatchDate && departmentCtrl.value) {
                  this.caseWatchDateApiService.getWatchDate(departmentCtrl.value).pipe(
                    take(1)
                  ).subscribe(date => this.form.controls[CaseFieldsNames.WatchDate].setValue(date));
              }
            });
          }
          break;
        }
        case CaseFieldsNames.ProductAreaId: {
          if (v.value) {
            this.productAreasService.getProductArea(v.value).pipe(
              take(1)
            ).subscribe(ct => {
              if (ct && ct.workingGroupId != null) {
                this.form.controls[CaseFieldsNames.WorkingGroupId].setValue(ct.workingGroupId);
              }
              if (ct && ct.priorityId != null) {
                this.form.controls[CaseFieldsNames.PriorityId].setValue(ct.priorityId);
              }
            });
          }
          break;
        }
        case CaseFieldsNames.ClosingReason: {
          if (v.value) {
            if (!this.form.controls[CaseFieldsNames.FinishingDate].value) {
              this.form.controls[CaseFieldsNames.FinishingDate].setValue(DateTime.local().toString());
            }
          } else {
            this.form.controls[CaseFieldsNames.FinishingDate].setValue('');
          }
          break;
        }
        case CaseFieldsNames.PersonEmail: {
          const externalEmailsToControl = this.form.controls[CaseFieldsNames.Log_ExternalEmailsTo];
          if (externalEmailsToControl) {
            externalEmailsToControl.setValue(v.value, {self: true, emitEvent: false });
          }
          break;
        }
        case CaseFieldsNames.Log_SendMailToNotifier: {
          const externalEmailsCcControl = this.form.controls[CaseFieldsNames.Log_ExternalEmailsCC];
          if (v.value === true) {
            externalEmailsCcControl.enable({onlySelf: true, emitEvent: true});
          } else {
            externalEmailsCcControl.disable({onlySelf: true, emitEvent: true});
          }
          break;
        }
      }
    }

    private processCaseData() {
      // create form
      const fb = new CaseFormGroupBuilder(this.translateService);
      this.form = fb.createFormGroup(this.caseData.fields, this.canSave);

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
      const filesField = this.getField(CaseFieldsNames.Filename);
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
      this.caseService.getCaseActions(this.caseId).pipe(
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
                  takeWhile(x => this.ownsLock)
              ).subscribe(_ => {
                this.caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue).pipe(
                  take(1)
                ).subscribe(res => {
                    if (!res) {
                      this.ownsLock = false;
                      this.caseLock.isLocked = true;
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

    private processNotifierChanged(eventData: NotifierChangedEvent) {
      const data = eventData.notifier;
      const isRegarding = eventData.type === NotifierType.Regarding;
      const formFieldsSetter = this.form.getNotifierFieldsSetter(isRegarding);

      if (data) {
        formFieldsSetter.setReportedBy(data.userId);
        formFieldsSetter.setPersonName(data.name);
        formFieldsSetter.setPersonEmail(data.email);
        formFieldsSetter.setPersonCellPhone(data.cellphone);
        formFieldsSetter.setPlace(data.place);
        formFieldsSetter.setUserCode(data.userCode);
        formFieldsSetter.setCostCenter(data.costCentre);

        this.updateOrganisationFields(formFieldsSetter, data);
      } else {
        this.resetNotifierFields(formFieldsSetter);
      }
    }

    private updateOrganisationFields(notifierFieldsSetter: NotifierFormFieldsSetter, data: IOrganisationData) {
      const regionId = data.regionId ? +data.regionId : null;
      const departmentId = data.departmentId ? +data.departmentId : null;
      const ouId = data.ouId ? +data.ouId : null;
      const formRegionId = +this.form.getValue(CaseFieldsNames.RegionId);

      if (regionId !== formRegionId) {
        this.changeRegion(notifierFieldsSetter, regionId, departmentId, ouId);
      } else {
          // just set new department if exists
          if (!isNaN(departmentId) && departmentId) {
            this.changeDepartment(notifierFieldsSetter, departmentId, ouId);
          }
      }
    }

    private changeRegion(notifierFieldsSetter: NotifierFormFieldsSetter, regionId?: number, departmentId?: number, ouId?: number) {
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
    }

    private changeDepartment(notifierFieldsSetter: NotifierFormFieldsSetter, departmentId: number, ouId?: number) {

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
    }

    private resetNotifierFields(formFieldsSetter: NotifierFormFieldsSetter) {
      formFieldsSetter.setReportedBy('');
      formFieldsSetter.setPersonName('');
      formFieldsSetter.setPersonEmail('');
      formFieldsSetter.setPersonCellPhone('');
      formFieldsSetter.setPlace('');
      formFieldsSetter.setUserCode('');
      formFieldsSetter.setCostCenter('');
      formFieldsSetter.setRegion('');
      this.changeRegion(formFieldsSetter, null, null, null);
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
          this.caseLockApiService.unLockCase(this.caseId, this.caseLock.lockGuid).subscribe();
        }
      }

      // shall we do extra checks?
      this.alertService.clearMessages();
      this.destroy$.next();
      this.destroy$.complete();

      this.commService.publish(Channels.Header, new HeaderEventData(true));

  }
}

interface IOrganisationData {
  regionId?: number;
  departmentId?: number;
  ouId?: number;
}
