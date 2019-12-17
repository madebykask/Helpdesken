import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { CaseFilesApiService } from '../services/api/case/case-files-api.service';

@Injectable({ providedIn: 'root' })
export class LogFileDataResolver implements Resolve<Observable<Blob>> {

  constructor(private caseFileService: CaseFilesApiService) {
  }

  resolve(activatedRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Blob> {
    const caseId = +activatedRoute.paramMap.get('caseId');
    const fileId = +activatedRoute.paramMap.get('fileId');
    const customerId = +activatedRoute.paramMap.get('cid');

    if (isNaN(caseId) || !caseId) {
      return throwError(`Invalid or missing caseId param. caseId: ${caseId}`);
    }

    if (isNaN(fileId) || !fileId) {
      return throwError(`Invalid or missing fileId param. fileId: ${fileId}`);
    }

    if (isNaN(customerId) || !customerId) {
      return throwError(`Invalid or missing customerId param. customerId: ${customerId}`);
    }

    return this.caseFileService.downloadLogFile(caseId, fileId, customerId);
  }
}

