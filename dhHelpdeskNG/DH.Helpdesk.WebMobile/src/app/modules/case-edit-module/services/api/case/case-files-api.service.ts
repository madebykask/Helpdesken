import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { take, catchError } from "rxjs/operators";

import { Observable, throwError } from "rxjs";
import { LocalStorageService } from "src/app/services/local-storage";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";
import { WindowWrapper } from "src/app/modules/shared-module/helpers/window-wrapper";

@Injectable({ providedIn: 'root' })
export class CaseFilesApiService extends HttpApiServiceBase {
    constructor(httpClient:HttpClient, localStorageService: LocalStorageService, private windowWrapper: WindowWrapper) {
        super(httpClient, localStorageService);
    }

    downloadLogFile(fileId:number, caseId:number) {
      let window = this.windowWrapper.nativeWindow;
      let url = this.buildResourseUrl(`/api/case/${caseId}/logfile/${fileId}`, { inline: true }, true, false);
      this.getFileBody(url, null).pipe(
          take(1),
      ).subscribe(data => {
          //saveAs(data, fileName); // uses file-saver.js
          window.location.href = window.URL.createObjectURL(data);
      });
    }

    downloadCaseFile(caseId:number, fileId:number, fileName:string) {
        let window = this.windowWrapper.nativeWindow;
        let url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, { inline: true }, true, false);
        this.getFileBody(url, null).pipe(
            take(1),
        ).subscribe(data => {
            //saveAs(data, fileName); // uses file-saver.js
            window.location.href = window.URL.createObjectURL(data);
        });
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
