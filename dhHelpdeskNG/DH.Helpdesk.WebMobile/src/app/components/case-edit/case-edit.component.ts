import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { map, finalize, catchError } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseService } from '../../services/case/case.service';
import { CaseEditInputModel } from '../../models';

@Component({
  selector: 'app-case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss']
})
export class CaseEditComponent implements OnInit {
    private caseId: number;
    private caseData: CaseEditInputModel;

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
        this.caseService.getCaseData(this.caseId)
            .subscribe(data => this.caseData = data);
    }

    private goToCaseOverview() {
        this.router.navigate(['/']);
    }
}