import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { config } from '@env/environment';
import { LocalStorageService } from "src/app/services/local-storage";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";

@Injectable({ providedIn: 'root' })
export class CaseFilesApiService extends HttpApiServiceBase {
    constructor(httpClient:HttpClient, localStorageService: LocalStorageService) {
        super(httpClient, localStorageService);
    }

    getCaseFileUploadUrl(caseId: string, cid: number) {
      return `${config.apiUrl}/api/case/${caseId}/file?cid=${cid}`;
    }

    downloadLogFile(caseId:number, fileId:number) {
      let url = this.buildResourseUrl(`/api/case/${caseId}/logfile/${fileId}`, { inline: true }, true, false);
      return this.getFileBody(url, null);
    }

    downloadCaseFile(caseId:number, fileId:number): Observable<Blob> {
        let url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, { inline: true }, true, false);
        return this.getFileBody(url, null);
    }

    deleteCaseFile(caseKey:string, fileId:number, fileName:string): Observable<any> {
        //todo: check when new case is ready
        let url = 
            fileId > 0 ? 
              this.buildResourseUrl(`/api/case/${caseKey}/file/${fileId}`, { fileName: fileName }, true, false) :
              this.buildResourseUrl(`/api/case/${caseKey}/file`, { fileName: fileName }, true, false); 
              
        return this.deleteResource(url);
    } 

    deleteTemplFiles(caseId:number): Observable<any> {
      let url = this.buildResourseUrl(`/api/case/${caseId}/tempfiles`, null, true, false);
      return this.deleteResource(url);
  } 
}
