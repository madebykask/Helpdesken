import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../local-storage';
import { CustomerModel } from 'src/app/models/customer/customer.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class CustomerApiService extends HttpApiServiceBase {

  protected constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  getCustomer(customerId: number): Observable<CustomerModel> {
    const url = this.buildResourseUrl(`api/customer/${customerId}`, null, false, false);
    return this.getJson<any>(url, null, false).pipe(
      map(data => Object.assign(new CustomerModel(), data))
    );
  }
}

