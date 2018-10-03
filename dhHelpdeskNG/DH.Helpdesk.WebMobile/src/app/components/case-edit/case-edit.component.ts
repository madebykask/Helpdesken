import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField, CaseOptionsFilterModel, OptionsDataSource, CaseOptions } from '../../models';
import { Subscription, of, Observable, zip, forkJoin } from 'rxjs';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent implements OnInit, OnDestroy {
    private caseId: number;
    private caseData: CaseEditInputModel;
    private caseDataSubscription: Subscription;
    private dataSource: OptionsDataSource;

    isLoaded: boolean = false;
    form: FormGroup;    

    bottomMenuSettings = {
        type: 'bottom',
        display: 'inline'
    };

    tabsMenuSettings = {
        type: 'tab',
        display: 'inline'
    }

    constructor(private route: ActivatedRoute,
         private caseService: CaseService,
         private router: Router) {
        if(this.route.snapshot.paramMap.has('id')) {
            this.caseId = Number(this.route.snapshot.paramMap.get('id'));
        } else {
            //TODO: throw error if caseid is invalid or go back
        }
    }

    ngOnInit() {
        this.loadCaseData();
    }

    loadCaseData(): any {
        this.isLoaded = false;
        this.caseDataSubscription = this.caseService.getCaseData(this.caseId)
            .subscribe(data => {//TODO: Error handle
                this.caseData = data;
                let filter = this.getCaseOptionsFilter(this.caseData);
                let op1 = this.caseService.getCaseOptions(filter);                    
                let op2 = Observable.create(observer => { 
                    let group: any = {};
                    data.fields.forEach(field => {
                        group[field.name] = new FormControl({value: field.value || '', disabled: true});                        
                    });                    
                    observer.next(new FormGroup(group));
                    observer.complete();                    
                }, );
                forkJoin(op1, op2)
                    .subscribe(([options, formgroup]) => {
                        this.dataSource = new OptionsDataSource(options);
                        this.form = formgroup as FormGroup;
                        this.isLoaded = true;        
                    })
            });
    }

    ngOnDestroy() {
        if(this.caseDataSubscription) {
            this.caseDataSubscription.unsubscribe();
        }
    }

    hasField(name: string) : boolean {
        if(this.caseData === null) {          
            throw new Error("No Case Data.");
        }
        return this.caseData.fields.filter(f => f.name === name).length > 0;
    }
/* 
    getValue<T>(name: string): T {
        if(this.caseData === null) {          
            throw new Error("No Case Data.");
        }
        return this.caseData.Fields.filter(f => f.Name === name)[0].Value as T;
    } */

    getField(name: string): BaseCaseField<any> {
        if(this.caseData === null) {          
            throw new Error("No Case Data.");
        }
        const fields = this.caseData.fields.filter(f => f.name === name);
        return fields.length <= 0 ? null : fields[0];
    }

    getValue(name: string) {
        const field = this.getField(name);
        return field != null ? field.value || null : undefined;//null - value is null, undefined - no such field
    }

    goToCaseOverview() {
        this.router.navigate(['/']);
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