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

  search(searchKey: string, categoryId: number = null): Observable<Array<any>> {
    const data = {
      query: searchKey,
      categoryId: categoryId
    };
    const url = this.buildResourseUrl('/api/Notifier/Search', data, true, false);
    return this.getJson<Array<any>>(url);
  }

  get(id: number) {
    const url = this.buildResourseUrl(`/api/Notifier/${id}`, null, true, false);
    return this.getJson<any>(url);
  }
}
