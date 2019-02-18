
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { CaseFilesApiService } from '../services/api/case/case-files-api.service';

@Injectable({ providedIn: 'root' })
export class CaseFileDataResolver implements Resolve<Observable<Blob>> {
  
  constructor(private caseFileService: CaseFilesApiService) {
  }

  resolve(activatedRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Blob> {
    const caseId = +activatedRoute.params['caseId'];
    const fileId = +activatedRoute.params['fileId'];

    if (isNaN(caseId) || !caseId) {
      throw Observable.throw(`Invalid or missing caseId param. caseId: ${caseId}`);
    }

    if (isNaN(fileId) || !fileId) {
      throw Observable.throw(`Invalid or missing fileId param. fileId: ${fileId}`);
    } 
   
    return this.caseFileService.downloadCaseFile(caseId, fileId);
  }
}

