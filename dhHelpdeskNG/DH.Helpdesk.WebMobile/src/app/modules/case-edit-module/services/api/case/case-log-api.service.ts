import { Injectable } from '@angular/core';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { config } from '@env/environment';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';

@Injectable({ providedIn: 'root' })
export class CaseLogApiService extends HttpApiServiceBase {

  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getUploadLogFileUrl(caseId: string, customerId: number, type: LogFileType) {
    return `${config.apiUrl}/api/case/${caseId}/logfile/?cid=${customerId}&type=${type}`;
  }

  getCaseLogs(caseId: number, customerId: number): Observable<Array<any>> {
    const url = this.buildResourseUrl(`/api/case/${caseId}/logs`, { cid: customerId }, false, true);
    return this.getJson<Array<any>>(url);
  }

  deleteTempLogFile(caseKey: string, fileName: string, type: LogFileType, customerId: number): Observable<boolean> {
    const url = this.buildResourseUrl(`/api/case/${caseKey}/templogfile`, { fileName: fileName, type: type.toString(), cid: customerId }, false, false);
    return this.deleteWithResult<boolean>(url);
  }
}

