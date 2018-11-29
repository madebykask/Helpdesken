import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource, 
  CaseSectionInputModel, CaseSectionType, CaseLockModel, CaseAccessMode } from '../../models';
import { forkJoin, Subject, Subscription, of } from 'rxjs';
import { switchMap, take, finalize, tap, delay, catchError, } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/header-event-data';
import { AlertsService } from 'src/app/helpers/alerts/alerts.service';
import { interval } from 'rxjs';
import { AuthenticationStateService } from 'src/app/services/authentication';
import { CaseLockApiService } from 'src/app/services/api/case/case-lock-api.service';
import { CaseSaveService } from 'src/app/services/case';
import { CaseFieldsNames } from '../../helpers/constants';
import { AlertType } from 'src/app/helpers/alerts/alert-types';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent {
    caseSectionTypes = CaseSectionType;
    caseFieldsNames = CaseFieldsNames;
    accessModeEnum = CaseAccessMode
    dataSource: OptionsDataSource;
    isLoaded = false;
    form: FormGroup;
    tabsMenuSettings = {};

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
                private translateService: TranslateService,
                private commService: CommunicationService,
                private alertService: AlertsService,
                private caseSaveService: CaseSaveService) {
        if (this.route.snapshot.paramMap.has('id')) {
            this.caseId = +this.route.snapshot.paramMap.get('id');
        } else {
            // TODO: throw error if caseid is invalid or go back
        }
    }

    ngOnInit() {
      this.commService.publish(Channels.Header, new HeaderEventData(false));
      this.loadCaseData();
    }

    getCaseTitle() : string {
        let title = this.translateService.instant('Ärende');
        if (this.caseData) {
            if (this.caseData.caseSolution) {
                title = this.caseData.caseSolution.name;
            }
        }
        return title;
    }

    loadCaseData(): any {
        this.isLoaded = false;
        const sessionId = this.authStateService.getUser().authData.sessionId;

        const caseLock$ = this.caseLockApiService.acquireCaseLock(this.caseId, sessionId);
        const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling
        const caseData$ =
            this.caseService.getCaseData(this.caseId)
                .pipe(
                    switchMap(data => { // TODO: Error handle
                      this.ownsLock = false;
                      this.caseData = data;
                      const filter = this.getCaseOptionsFilter(this.caseData);
                      return this.caseService.getCaseOptions(filter);
                    })
                );

                forkJoin(caseSections$, caseData$, caseLock$)
                .pipe(
                    take(1),
                    switchMap(([sectionData, options, caseLock]) => {
                      this.caseSections = sectionData;
                      this.dataSource = new OptionsDataSource(options);
                      this.caseLock = caseLock;
                      this.form = this.createFormGroup(this.caseData);
                      return of([sectionData, options]);
                    }),
                    finalize(() => this.isLoaded = true)
                )
                .subscribe(() => {
                	this.initLock();
                });
    }

    ngOnDestroy() {
        this.commService.publish(Channels.Header, new HeaderEventData(true));
        this.destroy$.next();

        if (this.caseLockIntervalSub) {
            this.caseLockIntervalSub.unsubscribe();
        }

        // unlock the case if required
        if (this.caseId > 0 && this.ownsLock) {
            this.caseLockApiService.unLockCase(this.caseLock.lockGuid).subscribe();
        }

        //todo: close ALERTS!!!
    }

    hasField(name: string): boolean {
        if (this.caseData === null) {
            throw new Error('No Case Data.');
        }
        // console.log('hasField: ' + name);
        return this.caseData.fields.filter(f => f.name === name).length > 0;
    }

    hasSection(type: CaseSectionType): boolean {
        if(this.caseData === null) {
            throw new Error('No Case Data.');
        }
        return this.caseData.fields.filter(f => f.section === type).length > 0;
    }

    getField(name: string): BaseCaseField<any> {
        if (this.caseData === null) {
            throw new Error('No Case Data.');
        }
        const fields = this.caseData.fields.filter(f => f.name === name);
        return fields.length <= 0 ? null : fields[0];
    }

    getValue(name: string) {
        const field = this.getField(name);
        return field != null ? field.value || null : undefined; // null - value is null, undefined - no such field
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
      return !this.form.disabled || this.caseAccessMode == CaseAccessMode.FullAccess;
    }

    saveCase() {
      if(!this.canSave) {
        return;
      }
      this.isLoaded = false;
      this.caseSaveService.saveCase(this.form, this.caseId)
        .pipe(
          //catchError()
        ).subscribe(() => {
          this.navigate('/casesoverview');
        });
    }

    private get caseAccessMode(): CaseAccessMode {
      return this.caseData.editMode;
    }

    private getCaseOptionsFilter(data: CaseEditInputModel) {
        let filter = new CaseOptionsFilterModel();
        filter.RegionId = this.getValue(this.caseFieldsNames.RegionId);
        filter.DepartmentId = this.getValue(this.caseFieldsNames.DepartmentId);
        filter.IsAboutRegionId = this.getValue(this.caseFieldsNames.IsAbout_RegionId);
        filter.IsAboutDepartmentId = this.getValue(this.caseFieldsNames.IsAbout_DepartmentId);
        filter.CaseResponsibleUserId = this.getValue(this.caseFieldsNames.CaseResponsibleUserId);
        filter.CaseWorkingGroupId = this.getValue(this.caseFieldsNames.WorkingGroupId);
        filter.CasePerformerUserId = this.getValue(this.caseFieldsNames.PerformerUserId);
        filter.CaseCausingPartId = this.getValue(this.caseFieldsNames.CausingPart);
        filter.CaseTypeId = this.getValue(this.caseFieldsNames.CaseTypeId);
        filter.ProductAreaId = this.getValue(this.caseFieldsNames.ProductAreaId);
        filter.Changes = this.hasField(this.caseFieldsNames.Change);
        filter.Currencies = this.hasField(this.caseFieldsNames.Cost_Currency);
        filter.CausingParts  = this.hasField(this.caseFieldsNames.CausingPart);
        filter.CustomerRegistrationSources = this.hasField(this.caseFieldsNames.RegistrationSourceCustomer);
        filter.Impacts = this.hasField(this.caseFieldsNames.ImpactId);
        filter.Performers = this.hasField(this.caseFieldsNames.PerformerUserId);
        filter.Priorities = this.hasField(this.caseFieldsNames.PriorityId);
        filter.Problems = this.hasField(this.caseFieldsNames.Problem);
        filter.Projects = this.hasField(this.caseFieldsNames.Project);
        filter.ResponsibleUsers = this.hasField(this.caseFieldsNames.CaseResponsibleUserId);
        filter.SolutionsRates = this.hasField(this.caseFieldsNames.SolutionRate);
        filter.StateSecondaries = this.hasField(this.caseFieldsNames.StateSecondaryId);
        filter.Statuses = this.hasField(this.caseFieldsNames.StatusId);
        filter.Suppliers = this.hasField(this.caseFieldsNames.SupplierId);// Supplier_Country_Id
        filter.Systems = this.hasField(this.caseFieldsNames.SystemId);
        filter.Urgencies = this.hasField(this.caseFieldsNames.UrgencyId);
        filter.WorkingGroups = this.hasField(this.caseFieldsNames.WorkingGroupId);
        filter.CaseTypes = this.hasField(this.caseFieldsNames.CaseTypeId);
        filter.ProductAreas = this.hasField(this.caseFieldsNames.ProductAreaId);
        filter.Categories = this.hasField(this.caseFieldsNames.CategoryId);
        filter.ClosingReasons = this.hasField(this.caseFieldsNames.ClosingReason);

        return filter;
    }

  private createFormGroup(data: CaseEditInputModel): FormGroup {
    let controls: { [key: string]: FormControl; } = {};
    data.fields.forEach(field => {
        controls[field.name] = new FormControl({
          value: field.value || '',
          disabled: this.caseLock.isLocked || this.caseAccessMode != CaseAccessMode.FullAccess
        });
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
              (this.caseLock.isLocked && this.caseLock.userId === currentUser.id) ?
                  'OBS! Du har redan oppnat detta arende i en annan session.' :
                  `OBS! Detta arende ar oppnat av ${this.caseLock.userFullName}'`;
          this.alertService.showMessage(notice, AlertType.Warning);
      } else if (this.caseLock.timerInterval > 0) {
          // run extend case lock at specified interval
          this.caseLockIntervalSub =
              interval(this.caseLock.timerInterval * 1000).pipe(
                switchMap(x => {
                  return this.caseLockApiService.reExtendedCaseLock(this.caseLock.lockGuid, this.caseLock.extendValue);
                },
                // catchError(err => {})// TODO:
                )
              )
              .subscribe(res => {
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