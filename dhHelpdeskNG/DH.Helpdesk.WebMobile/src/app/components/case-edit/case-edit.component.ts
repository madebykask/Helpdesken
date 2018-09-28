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
                    data.Fields.forEach(field => {
                        group[field.Name] = new FormControl({value: field.Value || '', disabled: true});                        
                    });                    
                    observer.next(new FormGroup(group));
                    observer.complete();                    
                });
                forkJoin(op1, op2)
                    .subscribe(([options, formgroup]) => {
                        this.dataSource = new OptionsDataSource(options as CaseOptions);
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
        return this.caseData.Fields.filter(f => f.Name === name).length > 0;
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
        const fields = this.caseData.Fields.filter(f => f.Name === name);
        return fields.length <= 0 ? null : fields[0];
    }

    goToCaseOverview() {
        this.router.navigate(['/']);
    }

    private getCaseOptionsFilter(data: CaseEditInputModel) {
        let filter = new CaseOptionsFilterModel();
        filter.RegionId = this.getField("Region_Id").Value || null;
        filter.DepartmentId = this.getField("Department_Id").Value || null;
        filter.IsAboutRegionId = this.getField("IsAbout_Region_Id").Value || null;
        filter.IsAboutDepartmentId = this.getField("IsAbout_Department_Id").Value || null;
        filter.CaseResponsibleUserId = this.getField("CaseResponsibleUser_Id").Value || null;

        
        return filter;
    }
}