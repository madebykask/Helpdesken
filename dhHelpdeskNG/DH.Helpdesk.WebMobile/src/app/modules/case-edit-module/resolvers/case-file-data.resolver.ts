import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { CaseFilesApiService } from '../services/api/case/case-files-api.service';

@Injectable({ providedIn: 'root' })
export class CaseFileDataResolver implements Resolve<Observable<Blob>> {

  constructor(private caseFileService: CaseFilesApiService) {
  }

  resolve(activatedRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Blob> {
    const caseId = +activatedRoute.paramMap.get('caseId');
    const caseKey = activatedRoute.paramMap.get('caseKey');
    const customerId = +activatedRoute.paramMap.get('customerId');

    if (!isNaN(caseId) && caseId > 0) {
      const fileId = +activatedRoute.paramMap.get('fileId');
      return this.caseFileService.downloadCaseFile(caseId, fileId, customerId);
    } else if (caseKey && caseKey.length) {
      const fileName = activatedRoute.queryParamMap.get('fileName') || '';
      if (fileName.length === 0) {
        return throwError(`Invalid or missing file name param. fileName: ${fileName}`);
      }
      return this.caseFileService.downloadTempCaseFile(caseKey, fileName, customerId);
    } else {
      return throwError(`Invalid or missing params. caseId: ${caseId}, caseKey: ${caseKey}`);
    }
  }
}

