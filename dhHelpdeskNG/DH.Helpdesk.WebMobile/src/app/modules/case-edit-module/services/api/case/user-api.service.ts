import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from 'src/app/services/local-storage';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserApiService extends HttpApiServiceBase {
    constructor(httpClient: HttpClient, localStorageService: LocalStorageService) {
        super(httpClient, localStorageService);
    }

    searchUsersEmails(searchKey: string): Observable<Array<any>> {
      const data = {
        query: searchKey
      };
      const url = this.buildResourseUrl('/api/users/searchEmails', data, true, false);
      return this.getJson(url);
    }
}
