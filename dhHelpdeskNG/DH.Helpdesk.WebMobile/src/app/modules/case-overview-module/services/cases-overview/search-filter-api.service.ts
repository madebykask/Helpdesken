import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SearchFilterApiService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService) {
      super(http, localStorageService);
  }

  getFavoritersFilters(): Observable<any[]> {
    const url = this.buildResourseUrl('/api/casesoverview/favoritefilters');
    return this.getJson<any[]>(url, null, false);
  }
}
