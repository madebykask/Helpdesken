import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class NotifierApiService extends HttpApiServiceBase {

  constructor(http: HttpClient, localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  search(searchKey: string, categoryId: number = null, customerId: number): Observable<Array<any>> {
    const data = {
      query: searchKey,
      categoryId: categoryId,
      cid: customerId
    };
    const url = this.buildResourseUrl('/api/Notifier/Search', data, false, false);
    return this.getJson<Array<any>>(url);
  }

  get(id: number, customerId: number) {
    const url = this.buildResourseUrl(`/api/Notifier/${id}`, { cid: customerId }, false, false);
    return this.getJson<any>(url);
  }
}
