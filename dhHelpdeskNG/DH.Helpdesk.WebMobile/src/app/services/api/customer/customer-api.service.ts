import { Injectable } from '@angular/core';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../local-storage';
import { CustomerModel } from 'src/app/models/customer/customer.model';
import { Observable } from 'rxjs';
import { finalize, map, take } from 'rxjs/operators';
import { CustomerEmailSettingsModel } from 'src/app/models/customer/customer-email-settings.model';

@Injectable({ providedIn: 'root' })
export class CustomerApiService extends HttpApiServiceBase {

  isLoadingCustomerEmailSettings = false;

  protected constructor(protected http: HttpClient,
    protected localStorageService: LocalStorageService) {
    super(http, localStorageService);
  }

  loadCustomerEmailSettings(customerId: number): Observable<CustomerEmailSettingsModel> {
    this.isLoadingCustomerEmailSettings = true;

    return this.getJson(this.buildResourseUrl(`/api/customer/customeremailsettings?cid=${customerId}`, undefined, false))
      .pipe(
      take(1),
      map((data: any) => {
          if (data) {
            
            const customerEmailSettings: CustomerEmailSettingsModel = {id: data.id, communicateWithNotifier: data.communicateWithNotifier, communicateWithPerformer: data.communicateWithPerformer};
            return customerEmailSettings;
          } else {
            return null;
          }
        }),
        finalize(() => this.isLoadingCustomerEmailSettings = false)
      );
  }

  getCustomer(customerId: number): Observable<CustomerModel> {
    const url = this.buildResourseUrl(`api/customer/${customerId}`, null, false, false);
    return this.getJson<any>(url, null, false).pipe(
      map(data => Object.assign(new CustomerModel(), data))
    );
  }

  getCustomerEmailSettings(customerId: number): Observable<CustomerEmailSettingsModel> {
    const url = this.buildResourseUrl(`api/customer/customeremailsettings?cid=${customerId}`, null, false, false);
    return this.getJson<any>(url, null, false).pipe(
      map(data => Object.assign(new CustomerEmailSettingsModel(), data))
    );
  }
}

