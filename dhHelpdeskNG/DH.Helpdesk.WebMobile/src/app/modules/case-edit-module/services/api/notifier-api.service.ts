import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { take } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class NotifierApiService extends HttpApiServiceBase {

  constructor(http: HttpClient, localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }
  
  search(searchKey: string, categoryId?: number): Observable<Array<any>> {
    const data = {
      query: searchKey,
      //categoryId: categoryId
    };
    const url = this.buildResourseUrl('/api/Notifier/Search', data, true, false);
    return this.getJson<Array<any>>(url).pipe(take(1));
  }

  get(id:number) {
    const url = this.buildResourseUrl(`/api/Notifier/${id}`, null, true, false);
    return this.getJson<any>(url).pipe(take(1));
  }

}