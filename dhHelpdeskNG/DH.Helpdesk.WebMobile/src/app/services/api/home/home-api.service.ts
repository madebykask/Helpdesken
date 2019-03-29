import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../local-storage';

@Injectable({
  providedIn: 'root'
})
export class HomeApiService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }
  
  getCustomerCasesStatus() {
    const url = this.buildResourseUrl("/api/home/casesstatus", null, true, false);
    return this.getJson<any>(url);
  }
}
