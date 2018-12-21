import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource, CaseSectionInputModel, CaseSectionType, CaseLockModel, CaseAccessMode, CasesSearchType, IBaseCaseField, OptionItem } from '../../models';
import { forkJoin, Subject, Subscription, of, throwError } from 'rxjs';
import { switchMap, take, finalize, tap, delay, catchError, map, takeUntil, } from 'rxjs/operators';
import { TranslateService, TranslateLoader } from '@ngx-translate/core';
import { CommunicationService, Channels, DropdownValueChangedEvent } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/header-event-data';
import { AlertsService } from 'src/app/helpers/alerts/alerts.service';
import { interval } from 'rxjs';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { CaseLockApiService } from 'src/app/services/api/case/case-lock-api.service';
import { CaseSaveService } from 'src/app/services/case';
import { CaseFieldsNames, CaseFieldOptions } from '../../helpers/constants';
import { AlertType } from 'src/app/helpers/alerts/alert-types';
import { CaseDataStore } from 'src/app/logic/case-edit/case-data.store';
import { CaseDataReducersFactory, } from 'src/app/logic/case-edit/case-data.reducers';
import { CaseEditDataHelper } from 'src/app/logic/case-edit/case-editdata.helper';
import { WorkingGroupsService } from 'src/app/services/case-organization/workingGroups-service';
import { WorkingGroupInputModel } from 'src/app/models/workinggroups/workingGroup-input.model';
import { StateSecondariesService } from 'src/app/services/case-organization/stateSecondaries-service';
import { StateSecondaryInputModel } from 'src/app/models/stateSecondaries/stateSecondaryInputModel';
import { LocalStorageService } from 'src/app/services/local-storage';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent {
    caseSectionTypes = CaseSectionType;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode
    dataSource: CaseDataStore;
    isLoaded = false;
    form: FormGroup;
    caseKey:string = '';
    tabsMenuSettings = {};
    private searchType: CasesSearchType = CasesSearchType.AllCases;
    private caseId: number;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = false;
    private destroy$ = new Subject();
    private caseLockIntervalSub: Subscription = null;
    private caseLock: CaseLockModel = null;

    constructor(private route: ActivatedRoute,
                private caseService: CaseService,
                private router: Router,
                private caseLockApiService: CaseLockApiService,
                private authStateService:AuthenticationStateService,
                private caseDataHelpder: CaseEditDataHelper,
                private alertService: AlertsService,
                private caseSaveService: CaseSaveService,
                private commService: CommunicationService,
                private сaseDataReducersFactory: CaseDataReducersFactory,
                private workingGroupsService: WorkingGroupsService,
                private stateSecondariesService: StateSecondariesService,
                private translateService : TranslateService,
                private localStorage:  LocalStorageService) {
        if (this.route.snapshot.paramMap.has('id')) {
            this.caseId = +this.route.snapshot.paramMap.get('id');
        } else {
            // TODO: throw error if caseid is invalid or go back
        }

        this.commService.listen(Channels.DropdownValueChanged).pipe(
          switchMap((v: DropdownValueChangedEvent) => {
            const reducer = this.сaseDataReducersFactory.createCaseDataReducers(this.dataSource);
            this.runUpdates(reducer, v);
            return of(v);
          }),
          takeUntil(this.destroy$)
        ).subscribe();
    }

    private runUpdates(reducer, v: DropdownValueChangedEvent) { // TODO: move to new class
      const filters = this.caseDataHelpder.getCaseOptionsFilter(this.caseData, (name: string) => this.getFormValue(name));
      const optionsHelper = this.caseService.getOptionsHelper(filters);

      switch (v.name) {
        case CaseFieldsNames.WorkingGroupId: {
          let perfomers$ = optionsHelper.getPerformers(false);
          perfomers$.pipe(
            take(1),
            switchMap((o: OptionItem[]) => {
                reducer.caseDataReducer(CaseFieldsNames.PerformerUserId, { items: o});
                return of(o);
              }),
            takeUntil(this.destroy$)
          ).subscribe();

          if (v.value != null && this.form.contains(CaseFieldsNames.StateSecondaryId)) {
            this.workingGroupsService.getWorkingGroup(v.value)
            .pipe(
              switchMap((wg: WorkingGroupInputModel) => {
                if (wg.stateSecondaryId != null) {
                  this.form.controls[CaseFieldsNames.StateSecondaryId].setValue(wg.stateSecondaryId);
                }
                return of(wg);
              }),
              takeUntil(this.destroy$)
            ).subscribe();
          }
          break;
        }
        case CaseFieldsNames.StateSecondaryId: {
          if (v.value != null) {
            this.stateSecondariesService.getStateSecondary(v.value)
            .pipe(
              switchMap((wg: StateSecondaryInputModel) => {
                if (wg.workingGroupId != null && this.form.contains(CaseFieldsNames.WorkingGroupId)) {
                  this.form.controls[CaseFieldsNames.WorkingGroupId].setValue(wg.workingGroupId);
                }
                return of(wg);
              }),
              takeUntil(this.destroy$)
            ).subscribe();
          }
          break;
        }
      }
    }

    ngOnInit() {
      this.commService.publish(Channels.Header, new HeaderEventData(false));
      this.loadCaseData();
    }

    ngOnDestroy() {
        this.commService.publish(Channels.Header, new HeaderEventData(true));

        if (this.caseLockIntervalSub) {
            this.caseLockIntervalSub.unsubscribe();
        }

        // unlock the case if required
        if (this.caseId > 0 && this.ownsLock) {
            this.caseLockApiService.unLockCase(this.caseId, this.caseLock.lockGuid).subscribe();
        }

        // shall we do extra checks?
        this.alertService.clearMessages();

        this.destroy$.next();
        this.destroy$.complete();
    }

    loadCaseData(): any {
      this.isLoaded = false;
      const sessionId = this.authStateService.getUser().authData.sessionId;

      const caseLock$ = this.caseLockApiService.acquireCaseLock(this.caseId, sessionId);
      const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling
      // todo: apply search type (all, my cases)
      const caseData$ =
          this.caseService.getCaseData(this.caseId)
              .pipe(
                  switchMap(data => { // TODO: Error handle
                    this.ownsLock = false;
                    this.caseData = data;
                    const filter = this.caseDataHelpder.getCaseOptionsFilter(this.caseData,
                      (name: string) => this.caseDataHelpder.getValue(this.caseData, name));
                    return this.caseService.getCaseOptions(filter);
                  }),
                  catchError((e) => throwError(e)),
              );

              forkJoin(caseSections$, caseData$, caseLock$)
              .pipe(
                  take(1),
                  map(([sectionData, options, caseLock]) => {
                    this.caseSections = sectionData;
                    this.dataSource = new CaseDataStore(options);
                    this.caseLock = caseLock;
                    this.processCaseData();
                  }),
                  finalize(() => this.isLoaded = true),
                  catchError((e) => throwError(e)),
                  takeUntil(this.destroy$)
              )
              .subscribe(() => {
                this.initLock();
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
        switchMap(() => of(this.router.navigate([url]))),
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
      return !this.caseLock.isLocked && this.caseAccessMode == CaseAccessMode.FullAccess;
    }

    saveCase() {
      if (!this.canSave) {
        return;
      }
      this.isLoaded = false;
      this.caseSaveService.saveCase(this.form, this.caseId)
        .pipe(
          //catchError()
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
      else{
        searchType = CasesSearchType[CasesSearchType.AllCases];
      }
      this.navigate('/casesoverview/' + searchType);
    }

    private processCaseData(){
      this.caseKey = this.caseData.id > 0 ? this.caseData.id.toString() : this.caseData.caseGuid.toString();
      this.form = this.createFormGroup(this.caseData);
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
          value: field.value || '',
          disabled: !this.canSave
        }, validators);
    });

    return new FormGroup(controls);
  }

  private initLock() {
    let currentUser =  this.authStateService.getUser();

    if (this.caseId > 0) {

      this.ownsLock = !this.caseLock.isLocked;

      if (this.caseLock.isLocked) {
          // TODO: translate messages
          let notice =
              this.caseLock.isLocked && this.caseLock.userId === currentUser.id ?
                  this.translateService.instant('OBS! Du har redan öppnat detta ärende i en annan session.') :
                  this.translateService.instant('OBS! Detta ärende är öppnat av') + ' ' + this.caseLock.userFullName;
          this.alertService.showMessage(notice, AlertType.Warning);
      } else if (this.caseLock.timerInterval > 0) {
          // run extend case lock at specified interval
          this.caseLockIntervalSub =
              interval(this.caseLock.timerInterval * 1000).pipe(
                switchMap(x => {
                  return this.caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue);
                },
                // catchError(err => {})// TODO:
                )
              ).subscribe(res => {
                  if (res === false) {
                      this.ownsLock = false;
                      this.caseLockIntervalSub.unsubscribe();
                      this.caseLockIntervalSub = null;
                  }
              });
      }
    }
  }
}
