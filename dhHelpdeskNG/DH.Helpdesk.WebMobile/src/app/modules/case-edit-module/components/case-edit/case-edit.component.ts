import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { forkJoin, Subject, Subscription, of, throwError, interval } from 'rxjs';
import { switchMap, take, finalize, delay, catchError, map, takeUntil, takeWhile, } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels, DropdownValueChangedEvent } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/header-event-data';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { WorkingGroupsService } from 'src/app/services/case-organization/workingGroups-service';
import { StateSecondariesService } from 'src/app/services/case-organization/stateSecondaries-service';
import { LocalStorageService } from 'src/app/services/local-storage';
import { CaseDataStore } from '../../logic/case-edit/case-data.store';
import { CaseDataReducersFactory } from '../../logic/case-edit/case-data.reducers';
import { CaseEditDataHelper } from '../../logic/case-edit/case-editdata.helper';
import { CaseFieldsNames, CasesSearchType } from 'src/app/modules/shared-module/constants';
import { CaseLockApiService } from '../../services/api/case/case-lock-api.service';
import { CaseSaveService } from '../../services/case';
import { CaseSectionType, CaseAccessMode, CaseEditInputModel, CaseSectionInputModel, CaseLockModel, BaseCaseField, CaseAction, CaseActionDataType } from '../../models';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';
import { CaseWatchDateApiService } from '../../services/api/case/case-watchDate-api.service';
import { CaseFilesApiService } from '../../services/api/case/case-files-api.service';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent {
    caseSectionTypes = CaseSectionType;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode;
    dataSource: CaseDataStore;
    isLoaded = false;
    form: FormGroup;
    caseKey:string;
    
    titleTabsSettings = {
      display:"top"
    }

    caseTabsSettings = {
      display:"top",
      layout: "fixed",
      theme:"mobiscroll"
    };

    currentTab = 'case_details';
    caseActions: CaseAction<CaseActionDataType>[] = [];

    private searchType: CasesSearchType = CasesSearchType.AllCases;
    private isNewCase: boolean = false;
    private caseId: number = 0;
    private templateId: number = 0;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = false;
    private destroy$ = new Subject();
    private caseLock: CaseLockModel = null;
    private isClosing = false;

    constructor(private route: ActivatedRoute,
                private caseService: CaseService,
                private router: Router,
                private formBuilder:FormBuilder,
                private caseLockApiService: CaseLockApiService,
                private authStateService:AuthenticationStateService,
                private caseDataHelpder: CaseEditDataHelper,
                private alertService: AlertsService,
                private caseSaveService: CaseSaveService,
                private commService: CommunicationService,
                private сaseDataReducersFactory: CaseDataReducersFactory,
                private workingGroupsService: WorkingGroupsService,
                private stateSecondariesService: StateSecondariesService,
                private caseWatchDateApiService: CaseWatchDateApiService,
                private translateService : TranslateService,
                private localStorage:  LocalStorageService,
                private caseFileService: CaseFilesApiService) {
      // read route params
      if (this.route.snapshot.paramMap.has('id')) {
          this.isNewCase = false;
          this.caseId = +this.route.snapshot.paramMap.get('id');
      } else if (this.route.snapshot.paramMap.has('templateId')) {
          this.isNewCase = true;
          this.templateId = this.route.snapshot.params['templateId'];
      } else {
        throw 'Invalid parameters';
      }

      this.commService.listen(Channels.DropdownValueChanged).pipe(
        takeUntil(this.destroy$)
      ).subscribe((v: DropdownValueChangedEvent) => {
          const reducer = this.сaseDataReducersFactory.createCaseDataReducers(this.dataSource);
          this.runUpdates(reducer, v);
      });
    }

    get isLocked() {
      return this.caseLock && this.caseLock.isLocked;
    }       

    get accessMode() {
      let accessMode = CaseAccessMode.NoAccess;
      if (this.caseData) {
          if (this.caseData.editMode === CaseAccessMode.NoAccess) {
            accessMode = CaseAccessMode.NoAccess;
          }
          else {
            if (this.caseLock && this.caseLock.isLocked) {
              accessMode = CaseAccessMode.ReadOnly;
            }
            else {
              accessMode = this.caseData.editMode;
            }
          }
      }
      return accessMode;
    } 

    private runUpdates(reducer, v: DropdownValueChangedEvent) { // TODO: move to new class
      const filters = this.caseDataHelpder.getCaseOptionsFilter(this.caseData, (name: string) => this.getFormValue(name));
      const optionsHelper = this.caseService.getOptionsHelper(filters);

      switch (v.name) {
        case CaseFieldsNames.WorkingGroupId: {
          optionsHelper.getPerformers(false).pipe(
            take(1)
          ).subscribe((o:OptionItem[]) => {
              reducer.caseDataReducer(CaseFieldsNames.PerformerUserId, { items: o});
          });

          if (!!v.value && this.form.contains(CaseFieldsNames.StateSecondaryId)) {
            this.workingGroupsService.getWorkingGroup(v.value)
            .pipe(
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
          if (v.value != null) {
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
      }
    }

    ngOnInit() {
      this.commService.publish(Channels.Header, new HeaderEventData(false));
      
     if (this.caseId > 0) {
          // existing case 
          this.loadCaseData(this.caseId);
      } else if (this.templateId > 0) {
          this.isNewCase = true;
          this.loadTemplate(this.templateId);
      }
    }

    private loadTemplate(templateId: number) {
      const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling
      const caseData$ = 
        this.caseService.getTemplateData(templateId).pipe(
          take(1),
          switchMap(data => {
            //this.ownsLock = false; //TODO: check?
            this.caseData = data;
            this.caseKey = this.caseData.caseGuid.toString();
            const filter = 
                this.caseDataHelpder.getCaseOptionsFilter(this.caseData, 
                    (name: string) => this.caseDataHelpder.getValue(this.caseData, name));

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
                    const filter = 
                        this.caseDataHelpder.getCaseOptionsFilter(this.caseData, 
                            (name: string) => this.caseDataHelpder.getValue(this.caseData, name));

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

    getCaseTitle() : string {
      return this.caseDataHelpder.getCaseTitle(this.caseData);
    }

    hasField(name: string): boolean {
      return this.caseDataHelpder.hasField(this.caseData, name);
    }

    hasSection(type: CaseSectionType): boolean {
      return this.caseDataHelpder.hasSection(this.caseData, type);
    }

    getField(name: string): BaseCaseField<any> {
      return this.caseDataHelpder.getField(this.caseData, name);
    }

    public navigate(url: string) {
      if(url == null) return;
      of(true).pipe(
        delay(200),
        map(() => this.router.navigate([url])),
        take(1)
      ).subscribe();
    }

    getSectionHeader(type: CaseSectionType): string {
        if (this.caseSections == null) return '';
        return this.caseSections.find(s => s.type == type).header;
    }

    isSectionOpen(type: CaseSectionType) {
        if (this.caseSections == null) return null
        return this.caseSections.find(s => s.type == type).isEditCollapsed ? null : true;
    }

    public get canSave() {
      if (this.isNewCase) return true; //TODO: check logic for new case
      return this.caseLock && 
             !this.caseLock.isLocked && 
             this.caseAccessMode == CaseAccessMode.FullAccess;
    }

    saveCase() {
      if (!this.canSave) return;
      
      this.isLoaded = false;
      this.caseSaveService.saveCase(this.form, this.caseData).pipe(
          take(1),
          catchError((e) => throwError(e)) //TODO: handle here ? show error ?
      ).subscribe(() => {
          this.goToCases();
      });
    }

    // returns value from formControl, not caseData
    getFormValue(name: string) {
      const noField = undefined;
      if (!this.form.contains(name)) {
        return noField;
      }
      const control = this.form.controls[name];
      return control != null ? control.value || null : noField; // null - value is null, undefined - no such field
    }

    goToCases() {
      let res = this.localStorage.getCaseSearchState();
      let searchType: string;
      if (res) {
        searchType = CasesSearchType[res.SearchType];
      }
      else {
        searchType = CasesSearchType[CasesSearchType.AllCases];
      }
      this.navigate('/casesoverview/' + searchType);
    }

    private processCaseData() {
      this.form = this.createFormGroup(this.caseData);
      
      //run only for existing case 
      if (this.caseId > 0) {
          this.loadCaseActions();
          this.cleanTempFiles(this.caseId);
      }
    } 

    cleanTempFiles(caseId:number) {
      this.caseFileService.deleteTemplFiles(caseId).pipe(
        take(1)
      ).subscribe();
    }
    
    private loadCaseActions() {
      this.caseService.getCaseActions(this.caseId).pipe(
        take(1),
        catchError((e) => throwError(e))
      ).subscribe(caseActions => {
        this.caseActions = caseActions;
      });
    }

    private get caseAccessMode(): CaseAccessMode {
      return this.caseData.editMode;
    }

    private createFormGroup(data: CaseEditInputModel): FormGroup {
      let controls: { [key: string]: FormControl; } = {};
      data.fields.forEach(field => {
          let validators = [];

          if (field.isRequired) {
              validators.push(Validators.required);
          }

          controls[field.name] = new FormControl({
            value: field.value === null ? '' : field.value,
            disabled: !this.canSave
          }, validators);
      });
      return new FormGroup(controls);
    }

    private initLock() {
      if (this.caseId > 0) {
        //set flag if we own the lock
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
  
    private showLockWarning() {
      let currentUser =  this.authStateService.getUser();
      let notice =
        this.caseLock.isLocked && this.caseLock.userId === currentUser.id ?
            this.translateService.instant('OBS! Du har redan öppnat detta ärende i en annan session.') :
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
