import { Injectable, Inject} from '@angular/core';
import { AppConfig } from '@app/shared/app-config/app-config';
import { IAppConfig } from '@app/shared/app-config/app-config.interface';
import { ExtendedHttpService } from './data/extended-http.service';
import { HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs/internal/observable/throwError';

@Injectable({ providedIn: 'root'})
export class CaseFilesService {

  constructor(@Inject(AppConfig) private config: IAppConfig,
    private httpService: ExtendedHttpService) {}

  deleteFile(caseId: number, fileId: number, customerId: number, caseNumber: string, fileName: string, authToken: string) {
    let url = `${this.config.apiHost}/api/files/${caseId}/file/${fileId}?cid=${customerId}&caseNumber=${caseNumber || ''}&fileName=${fileName || ''}`;
    return this.httpService.deleteJson<any>(url, { helpdeskAuthToken: authToken });
  }

  deleteTempFile(caseGuid: string, fileName: string, authToken: string) {
    let url = `${this.config.apiHost}/api/files/${caseGuid}/file?fileName=${fileName || ''}`;
    return this.httpService.deleteJson<any>(url, { helpdeskAuthToken: authToken });
  }

  downloadCaseFile(caseId: number, fileId: number, caseNumber: string, customerId: number) {
    const url = `${this.config.apiHost}/api/files/${caseId}/file/${fileId}?cid=${customerId}&caseNumber=${caseNumber || ''}`;
    return this.httpService.getFileBody(url, null);
  }

  downloadTempCaseFile(caseKey: string, fileName: string, customerId: number) {
    const url = `${this.config.apiHost}/api/files/${caseKey}/file?cid=${customerId}&fileName=${fileName || ''}`;
    return this.httpService.getFileBody(url);
  }
}
