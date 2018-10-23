import { Component, OnInit, OnDestroy } from '@angular/core';
import {  FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource,
      CaseSectionInputModel, CaseSectionType } from '../../models';
import { Subscription, Observable, forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent implements OnInit, OnDestroy {
    private caseId: number;
    private caseData: CaseEditInputModel;
    private subscriptions = new Array<Subscription>();        
    private caseSections: CaseSectionInputModel[];
    caseSectionTypes = CaseSectionType;
    dataSource: OptionsDataSource;
    isLoaded = false;
    form: FormGroup;   

    tabsMenuSettings = {
    };

    constructor(private route: ActivatedRoute,
         private caseService: CaseService,
         private _router: Router) {
        if (this.route.snapshot.paramMap.has('id')) {
            this.caseId = Number(this.route.snapshot.paramMap.get('id'));
        } else {
            // TODO: throw error if caseid is invalid or go back
        }
    }

    ngOnInit() {
        this.loadCaseData();
    }

    getCaseTitle() {
        if (this.caseData) {
            if (this.caseData.caseSolution) {
                return this.caseData.caseSolution.name;
            } else {
                return `Case ${this.caseData.caseNumber}`;
            }
        } else {
            return '';
        }        
    }

    loadCaseData(): any {
        this.isLoaded = false;
        const caseSections$ = this.caseService.getCaseSections(); // TODO: error handling
        const caseData$ = this.caseService.getCaseData(this.caseId)
            .pipe(
                switchMap(data => { // TODO: Error handle
                    this.caseData = data;
                    const filter = this.getCaseOptionsFilter(this.caseData);
                    const op1 = this.caseService.getCaseOptions(filter);                    
                    const op2 = Observable.create(observer => { 
                        let group: any = {};
                        data.fields.forEach(field => {
                            group[field.name] = new FormControl({value: field.value || '', disabled: true});                        
                        });                    
                        observer.next(new FormGroup(group));
                        observer.complete();                                        
                    }) as Observable<FormGroup>;
                    return forkJoin(op1, op2);
                })
            )
        this.subscriptions.push(forkJoin(caseSections$, caseData$)
            .subscribe(([sectionData, [options, formgroup]]) => {
                this.caseSections = sectionData;
                this.dataSource = new OptionsDataSource(options);
                this.form = formgroup;
                this.isLoaded = true;        
            }));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(s => {
            if(!s.closed) s.unsubscribe();
        })
    }

    hasField(name: string): boolean {
        if(this.caseData === null) {          
            throw new Error("No Case Data.");
        }
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
        this._router.navigate([url]);
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
        filter.CaseTypeId = this.getValue("CaseType_Id");
        filter.ProductAreaId = this.getValue("ProductArea_Id");
        filter.Changes = this.hasField('Change');
        filter.Currencies = this.hasField('Cost_Currency');
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