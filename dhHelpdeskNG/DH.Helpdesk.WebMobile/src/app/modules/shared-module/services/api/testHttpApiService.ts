import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../../../services/local-storage';
import { HttpApiServiceBase } from './httpServiceBase';
import { ErrorHandlingService } from 'src/app/services/logging';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TestHttpApiService extends HttpApiServiceBase {
  constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, protected errorHandlingService: ErrorHandlingService) {
    super(http, localStorageService);
  }

  testLoginOptions() {
    const headers = {
      "Access-Control-Request-Headers": "authorization,content-type",
      "Access-Control-Request-Method": "POST"
    };
    return this.sendOptions(this.buildResourseUrl('/api/account/login', undefined, false), headers, true);
  }
}
