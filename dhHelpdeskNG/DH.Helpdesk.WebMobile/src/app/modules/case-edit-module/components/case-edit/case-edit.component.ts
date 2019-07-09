import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { forkJoin, Subject, of, throwError, interval } from 'rxjs';
import { switchMap, take, finalize, delay, catchError, map, takeUntil, takeWhile } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels, CaseFieldValueChangedEvent, } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/data/header-event-data';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseDataStore } from '../../logic/case-edit/case-data.store';
import { CaseEditDataHelper } from '../../logic/case-edit/case-editdata.helper';
import { CaseFieldsNames, CasesSearchType } from 'src/app/modules/shared-module/constants';
import { CaseLockApiService } from '../../services/api/case/case-lock-api.service';
import { CaseSaveService } from '../../services/case';
import { CaseSectionType, CaseAccessMode, CaseEditInputModel, CaseSectionInputModel,
   CaseLockModel, CaseAction, CaseActionDataType, CaseFileModel } from '../../models';
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
                private translateService: TranslateService,
                private localStorage:  LocalStorageService,
                private caseFileService: CaseFilesApiService,
                private caseWorkflowsService: CaseWorkflowsApiService,
                private caseEditLogic: CaseEditLogic,
                private caseTemplateService: CaseTemplateService) {
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

    caseSectionTypes = CaseSectionType;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode;
    dataSource: CaseDataStore;
    isLoaded = false;
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

    currentTab = 'case_details';
    caseActions: CaseAction<CaseActionDataType>[] = [];
    notifierTypes = NotifierType;
    isCommunicationSectionVisible = false;

    private isNewCase = false;
    private caseId = 0;
    private templateId = 0;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = true;
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
          this.loadWorkflows(caseId);
      });
    }

    getCaseTitle(): string {
      return this.caseDataHelpder.getCaseTitle(this.caseData);
    }

    showField(name: string): boolean {
      return this.caseDataHelpder.hasField(this.caseData, name) && !this.caseEditLogic.getField(this.caseData, name).isHidden;
    }

    hasSection(type: CaseSectionType): boolean {
      return this.caseDataHelpder.hasSection(this.caseData, type);
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

    saveCase(reload: boolean = false) {
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
        reload ? this.ngOnInit() : this.goToCases();
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

    ///////////////////////////////////////////////////////// Private section

    private loadWorkflows(caseId: number) {
      this.caseWorkflowsService.getWorkflows(caseId)
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

    private loadTemplate(templateId: number) {
      const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling

      const caseData$ =
        this.caseService.getTemplateData(templateId).pipe(
          take(1),
          switchMap(data => {
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
                  takeWhile(() => this.ownsLock)
              ).subscribe(_ => {
                this.caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue).pipe(
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

