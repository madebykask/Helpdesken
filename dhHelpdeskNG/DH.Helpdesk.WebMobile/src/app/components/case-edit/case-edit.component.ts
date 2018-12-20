import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource, CaseSectionInputModel, CaseSectionType, CaseLockModel, CaseAccessMode, CasesSearchType, IBaseCaseField, OptionItem } from '../../models';
import { forkJoin, Subject, Subscription, of, throwError } from 'rxjs';
import { switchMap, take, finalize, tap, delay, catchError, map, takeUntil, } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
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
    private searchType: CasesSearchType = CasesSearchType.All;
    private caseId: number;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    private ownsLock = false;
    private destroy$ = new Subject();
    private caseLockIntervalSub: Subscription = null;
    private caseLock: CaseLockModel = null;

    constructor(private route: ActivatedRoute,
                private _caseService: CaseService,
                private _router: Router,
                private _caseLockApiService: CaseLockApiService,
                private _authStateService:AuthenticationStateService,
                private _caseDataHelpder: CaseEditDataHelper,
                private _alertService: AlertsService,
                private _caseSaveService: CaseSaveService,
                private _commService: CommunicationService,
                private _сaseDataReducersFactory: CaseDataReducersFactory, 
                private _workingGroupsService: WorkingGroupsService,
                private _stateSecondariesSerive: StateSecondariesService,
                private _translateService: TranslateService) {
        if (this.route.snapshot.paramMap.has('id')) {
            this.caseId = +this.route.snapshot.paramMap.get('id');
        } else {
            // TODO: throw error if caseid is invalid or go back
        }

        this._commService.listen(Channels.DropdownValueChanged).pipe(
          switchMap((v: DropdownValueChangedEvent) => {
            const reducer = this._сaseDataReducersFactory.createCaseDataReducers(this.dataSource);
            this.runUpdates(reducer, v);
            return of(v);
          }),
          takeUntil(this.destroy$)
        ).subscribe();
    }

    private runUpdates(reducer, v: DropdownValueChangedEvent) { // TODO: move to new class
      const filters = this._caseDataHelpder.getCaseOptionsFilter(this.caseData, (name: string) => this.getFormValue(name));
      const optionsHelper = this._caseService.getOptionsHelper(filters);

      switch (v.name) {
        case CaseFieldsNames.WorkingGroupId: {
          let perfomers$ = optionsHelper.getPerformers();
          perfomers$.pipe(
            take(1),
            switchMap((o: OptionItem[]) => {
                reducer.caseDataReducer(CaseFieldsNames.PerformerUserId, { items: o});
                return of(o);
              }),
            takeUntil(this.destroy$)
          ).subscribe();

          if (v.value != null && this.form.contains(CaseFieldsNames.StateSecondaryId)) {
            this._workingGroupsService.getWorkingGroup(v.value)
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
        }
        case CaseFieldsNames.StateSecondaryId: {
          if (v.value != null) {
            this._stateSecondariesSerive.getStateSecondary(v.value)
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
        }
      }
    }

    ngOnInit() {
      this._commService.publish(Channels.Header, new HeaderEventData(false));
      this.loadCaseData();
    }

    ngOnDestroy() {
        this._commService.publish(Channels.Header, new HeaderEventData(true));

        if (this.caseLockIntervalSub) {
            this.caseLockIntervalSub.unsubscribe();
        }

        // unlock the case if required
        if (this.caseId > 0 && this.ownsLock) {
            this._caseLockApiService.unLockCase(this.caseId, this.caseLock.lockGuid).subscribe();
        }

        // shall we do extra checks? 
        this._alertService.clearMessages();

        this.destroy$.next();
        this.destroy$.complete();
    }

    loadCaseData(): any {
      this.isLoaded = false;
      const sessionId = this._authStateService.getUser().authData.sessionId;

      const caseLock$ = this._caseLockApiService.acquireCaseLock(this.caseId, sessionId);
      const caseSections$ = this._caseService.getCaseSections(); // TODO: error handling
      // todo: apply search type (all, my cases)
      const caseData$ =
          this._caseService.getCaseData(this.caseId)
              .pipe(
                  switchMap(data => { // TODO: Error handle
                    this.ownsLock = false;
                    this.caseData = data;
                    const filter = this._caseDataHelpder.getCaseOptionsFilter(this.caseData,
                      (name: string) => this._caseDataHelpder.getValue(this.caseData, name));
                    return this._caseService.getCaseOptions(filter);
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
      return this._caseDataHelpder.getCaseTitle(this.caseData);
    }

    hasField(name: string): boolean {
      return this._caseDataHelpder.hasField(this.caseData, name);
    }

    hasSection(type: CaseSectionType): boolean {
      return this._caseDataHelpder.hasSection(this.caseData, type);
    }

    getField(name: string): BaseCaseField<any> {
      return this._caseDataHelpder.getField(this.caseData, name);
    }

    public navigate(url: string) {
      if(url == null) return;
      of(true).pipe(
        delay(200),
        switchMap(() => of(this._router.navigate([url]))),
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
      this._caseSaveService.saveCase(this.form, this.caseId)
        .pipe(
          //catchError()
        ).subscribe(() => {
          this.navigate('/casesoverview');
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

        if (this.isRequired(field)) {
            validators.push(Validators.required);
        }

        controls[field.name] = new FormControl({
          value: field.value || '',
          disabled: !this.canSave
        }, validators);
    });

    return new FormGroup(controls);
  }

  private isRequired(field: IBaseCaseField<any>): boolean {
    if (!field.options) {
      return false;
    }
    return field.options.findIndex((value, index) => {
      return value.key == CaseFieldOptions.reqiured;
    }) != -1;
  }

  private initLock() {
    let currentUser =  this._authStateService.getUser();

    if (this.caseId > 0) {

      this.ownsLock = !this.caseLock.isLocked;

      if (this.caseLock.isLocked) {
          // TODO: translate messages
          let notice =
              this.caseLock.isLocked && this.caseLock.userId === currentUser.id ?
                  this._translateService.instant('OBS! Du har redan öppnat detta ärende i en annan session.') :
                  this._translateService.instant('OBS! Detta ärende är öppnat av')  + this.caseLock.userFullName;                  
          this._alertService.showMessage(notice, AlertType.Warning);
      } else if (this.caseLock.timerInterval > 0) {
          // run extend case lock at specified interval
          this.caseLockIntervalSub =
              interval(this.caseLock.timerInterval * 1000).pipe(
                switchMap(x => {
                  return this._caseLockApiService.reExtendedCaseLock(this.caseId, this.caseLock.lockGuid, this.caseLock.extendValue);
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