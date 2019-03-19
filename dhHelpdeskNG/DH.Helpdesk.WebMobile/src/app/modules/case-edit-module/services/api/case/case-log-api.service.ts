import { Injectable } from "@angular/core";
import { LocalStorageService } from "src/app/services/local-storage";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services";
import { HttpClient } from "@angular/common/http";
import { take } from "rxjs/internal/operators";
import { Observable } from "rxjs";
import { config } from '@env/environment';
@Injectable({ providedIn: 'root' })
export class CaseLogApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getUploadLogFileUrl(caseId:string, customerId:number) {
    return `${config.apiUrl}/api/case/${caseId}/logfile/?cid=${customerId}`;
  }

  getCaseLogs(caseId:number): Observable<Array<any>> {
    let url = this.buildResourseUrl(`/api/case/${caseId}/logs`, null, true, true);
    return this.getJson<Array<any>>(url)
        .pipe(
            take(1)
        );
  }

  deleteTempLogFile(caseKey:string, fileName:string): Observable<boolean> {
    //todo: check when new case is ready,
    //todo: encode fileName
    let url = this.buildResourseUrl(`/api/case/${caseKey}/templogfile`, { fileName: fileName }, true, false);
    return this.deleteWithResult<boolean>(url);
  } 

}

