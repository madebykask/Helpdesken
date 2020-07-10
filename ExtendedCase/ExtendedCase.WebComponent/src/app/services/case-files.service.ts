import { Injectable, Inject} from '@angular/core';
import { AppConfig } from '@app/shared/app-config/app-config';
import { IAppConfig } from '@app/shared/app-config/app-config.interface';
import { ExtendedHttpService } from './data/extended-http.service';

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
}
