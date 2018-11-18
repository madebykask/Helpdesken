import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource, CaseSectionInputModel, CaseSectionType } from '../../models';
import { forkJoin, Observable, Subject, Subscription } from 'rxjs';
import { switchMap, take, takeUntil, map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationService, Channels } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/header-event-data';
import { AlertsService } from 'src/app/helpers/alerts/alerts.service';
import { interval } from 'rxjs';
import 'rxjs/add/operator/map'
import { AuthenticationService, AuthenticationStateService } from 'src/app/services/authentication';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent implements OnInit, OnDestroy {
    private caseId: number;
    private caseData: CaseEditInputModel;
    private caseSections: CaseSectionInputModel[];
    caseSectionTypes = CaseSectionType;
    dataSource: OptionsDataSource;
    isLoaded = false;
    form: FormGroup;   
    private ownsLock = false;
    private destroy$ = new Subject();    

    tabsMenuSettings = {};
    private caseLockIntervalSub: Subscription = null;

    //ctor
    constructor(private route: ActivatedRoute, 
                private caseService: CaseService,                 
                private router: Router, 
                private authenticationService: AuthenticationService,
                private authStateService:AuthenticationStateService,
                private translateService: TranslateService, 
                private commService: CommunicationService,
                private alertService: AlertsService) {
        if (this.route.snapshot.paramMap.has('id')) {
            this.caseId = +this.route.snapshot.paramMap.get('id');
        } else {
            // TODO: throw error if caseid is invalid or go back
        }
    }

    ngOnInit() {
        this.loadCaseData();
        this.commService.publish(Channels.Header, new HeaderEventData(false));
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
        
        const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling
        const caseData$ = 
            this.caseService.getCaseData(this.caseId)
                .pipe( 
                    take(1),                    
                    switchMap(data => { // TODO: Error handle                    
                        this.processCaseDataResponse(data);
                        const filter = this.getCaseOptionsFilter(this.caseData);
                        return this.caseService.getCaseOptions(filter);
                    })
                );        
            
                forkJoin(caseSections$, caseData$)
                .pipe(
                    take(1)
                )
                .subscribe(([sectionData, options]) => {
                    this.caseSections = sectionData;
                    this.dataSource = new OptionsDataSource(options);                    
                    this.isLoaded = true;        
                });       
    }

    private processCaseDataResponse(data){
        this.ownsLock = false;
        this.caseData = data;
        let controls: any = {};
        data.fields.forEach(field => {
            controls[field.name] = new FormControl({value: field.value || '', disabled: true});                        
        });           

        this.form = new FormGroup(controls);
        
        let caseLock  = this.caseData.caseLock;

        let currentUser =  this.authStateService.getUser();

        if (this.caseId > 0) {
            this.ownsLock = !caseLock.isLocked;

            if (caseLock.isLocked) {
                //todo: translate messages
                let notice =                    
                    (caseLock.isLocked && caseLock.userId === currentUser.id) ?
                        "OBS! Du har redan oppnat detta arende i en annan session." :
                        `OBS! Detta arende ar oppnat av ${caseLock.userFullName}"`;                
                this.alertService.warning(notice);
            } else if (caseLock.timerInterval > 0) { 
                //run extend case lock at specified interval
                this.caseLockIntervalSub = 
                    interval(caseLock.timerInterval * 1000).subscribe(x => {                        
                        console.log('>>> timer interval called: ' + (x || 0));
                        this.caseService.ReExtendedCaseLock(caseLock.lockGuid, caseLock.extendValue)
                            .subscribe(res => {
                                if (res === false) {
                                    this.ownsLock = false;
                                    this.caseLockIntervalSub.unsubscribe();
                                    this.caseLockIntervalSub = null;
                                }
                            }, error => {
                                //todo: handle error 
                            });
                    });
            }
        }
    }

    ngOnDestroy() {
        this.commService.publish(Channels.Header, new HeaderEventData(true));
        this.destroy$.next();
        
        if (this.caseLockIntervalSub) {
            this.caseLockIntervalSub.unsubscribe();
        }

        let caseLock = this.caseData.caseLock;
        //unlock the case if required
        if (this.caseId > 0 && this.ownsLock) {
            //console.log('>>> Unlocking case');
            this.caseService.UnLockCase(caseLock.lockGuid);
        }
        //console.log('>>> case edit: destroy called!');
    }

    hasField(name: string): boolean {
        if (this.caseData === null) {          
            throw new Error("No Case Data.");
        }
        //console.log('hasField: ' + name);
        return this.caseData.fields.filter(f => f.name === name).length > 0;
    }

    hasSection(type: CaseSectionType): boolean {
        if(this.caseData === null) {          
            throw new Error("No Case Data.");
        }
        return this.caseData.fields.filter(f => f.section === type).length > 0;
    }
    /* getValue<T>(name: string): T {
        if(this.caseData === null) {
            throw new Error("No Case Data.");
        }
        return this.caseData.Fields.filter(f => f.Name === name)[0].Value as T;
    }*/

    getField(name: string): BaseCaseField<any> {
        if (this.caseData === null) {
            throw new Error("No Case Data.");
        }
        const fields = this.caseData.fields.filter(f => f.name === name);
        return fields.length <= 0 ? null : fields[0];
    }

    getValue(name: string) {
        const field = this.getField(name);
        return field != null ? field.value || null : undefined;//null - value is null, undefined - no such field
    }

    goTo(url: string) {
        if (url == null) return;
        this.router.navigate([url]);
      }

    getSectionHeader(type: CaseSectionType): string {
        if (this.caseSections == null) return "";
        return this.caseSections.find(s => s.type == type).header;
    }

    isSectionOpen(type: CaseSectionType) {
        if (this.caseSections == null) return null
        return this.caseSections.find(s => s.type == type).isEditCollapsed ? null : true;
    }

    saveCase() {
        return false;
    }

    private getCaseOptionsFilter(data: CaseEditInputModel) {
        let filter = new CaseOptionsFilterModel();
        //TODO: review and put all field names to constants        
        filter.RegionId = this.getValue("Region_Id");
        filter.DepartmentId = this.getValue("Department_Id");
        filter.IsAboutRegionId = this.getValue("IsAbout_Region_Id");
        filter.IsAboutDepartmentId = this.getValue("IsAbout_Department_Id");
        filter.CaseResponsibleUserId = this.getValue("CaseResponsibleUser_Id");
        filter.CaseWorkingGroupId = this.getValue("WorkingGroup_Id");
        filter.CasePerformerUserId = this.getValue("Performer_User_Id");
        filter.CaseCausingPartId = this.getValue("CausingPart");
        filter.CaseTypeId = this.getValue("CaseType_Id");
        filter.ProductAreaId = this.getValue("ProductArea_Id");
        filter.Changes = this.hasField('Change');
        filter.Currencies = this.hasField('Cost_Currency');
        filter.CausingParts  = this.hasField('CausingPart');
        filter.CustomerRegistrationSources = this.hasField('RegistrationSourceCustomer');
        filter.Impacts = this.hasField('Impact_Id');
        filter.Performers = this.hasField('Performer_User_Id');
        filter.Priorities = this.hasField('Priority_Id');
        filter.Problems = this.hasField('Problem');
        filter.Projects = this.hasField('Project');
        filter.ResponsibleUsers = this.hasField('CaseResponsibleUser_Id');
        filter.SolutionsRates = this.hasField('SolutionRate');
        filter.StateSecondaries = this.hasField('StateSecondary_Id');
        filter.Statuses = this.hasField('Status_Id');
        filter.Suppliers = this.hasField('Supplier_Id');//Supplier_Country_Id
        filter.Systems = this.hasField('System_Id');
        filter.Urgencies = this.hasField('Urgency_Id');
        filter.WorkingGroups = this.hasField('WorkingGroup_Id');
        filter.CaseTypes = this.hasField('CaseType_Id');
        filter.ProductAreas = this.hasField('ProductArea_Id');
        filter.Categories = this.hasField('Category_Id');
        filter.ClosingReasons = this.hasField('ClosingReason');

        return filter;
    }
}