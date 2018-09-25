import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { map, finalize, catchError } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel, BaseCaseField } from '../../models';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent implements OnInit, OnDestroy {
    private caseId: number;
    private caseData: CaseEditInputModel;
    private caseDataSubscription: Subscription;

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
            .subscribe(data => {
                let group: any = {};
                this.caseData = data;
                data.Fields.forEach(field => {
                    group[field.Name] = new FormControl({value: field.Value || '', disabled: true});
                })
                this.form = new FormGroup(group);
                this.isLoaded = true;
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
        return this.caseData.Fields.filter(f => f.Name === name)[0];
    }

    goToCaseOverview() {
        this.router.navigate(['/']);
    }
}