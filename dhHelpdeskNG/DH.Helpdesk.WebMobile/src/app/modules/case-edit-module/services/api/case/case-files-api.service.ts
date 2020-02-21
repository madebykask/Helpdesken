import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { config } from '@env/environment';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';

@Injectable({ providedIn: 'root' })
export class CaseFilesApiService extends HttpApiServiceBase {
    constructor(httpClient: HttpClient, localStorageService: LocalStorageService) {
        super(httpClient, localStorageService);
    }

    getCaseFileUploadUrl(caseId: string, cid: number) {
      return `${config.apiUrl}/api/case/${caseId}/file?cid=${cid}`;
    }

    downloadLogFile(caseId: number, fileId: number, customerId: number) {
      const url = this.buildResourseUrl(`/api/case/${caseId}/logfile/${fileId}`, { inline: true, cid: customerId }, false, false);
      return this.getFileBody(url, null);
    }

    downloadCaseFile(caseId: number, fileId: number, customerId: number): Observable<Blob> {
        const url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, { inline: true, cid: customerId }, false, false);
        return this.getFileBody(url, null);
    }

    downloadTempCaseFile(caseKey: string, fileName: string, customerId: number): Observable<Blob> {
      const url = this.buildResourseUrl(`/api/case/${caseKey}/file`, { inline: true, fileName: fileName, cid: customerId }, false, false);
      return this.getFileBody(url);
    }

    deleteCaseFile(caseKey: string, fileId: number, fileName: string, customerId: number): Observable<any> {
        //todo: check when new case is ready
        const url =
            fileId > 0 ?
              this.buildResourseUrl(`/api/case/${caseKey}/file/${fileId}`, { fileName: fileName, cid: customerId }, false, false) :
              this.buildResourseUrl(`/api/case/${caseKey}/file`, { fileName: fileName, cid: customerId }, false, false);

        return this.deleteResource(url);
    }

    getFileUploadWhiteList(): Observable<any> {
      const url = this.buildResourseUrl('/api/case/file/getFileUploadWhiteList', {}, false, false);
      return this.postJson<any>(url, null);
  }

    deleteTemplFiles(caseId: number, customerId: number): Observable<any> {
      const url = this.buildResourseUrl(`/api/case/${caseId}/tempfiles`, { cid: customerId }, false, false);
      return this.deleteResource(url);
  }
}
