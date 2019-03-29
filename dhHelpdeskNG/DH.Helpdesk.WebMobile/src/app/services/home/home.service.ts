import { Injectable } from '@angular/core';
import { HomeApiService } from '../api/home/home-api.service';
import { CustomerCasesStatusModel } from 'src/app/modules/shared-module/models/home/customerCasesStatus.model';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private homeApiService: HomeApiService) {
  }

  getCustomerCasesStatus(): Observable<CustomerCasesStatusModel> {
    return this.homeApiService.getCustomerCasesStatus().pipe(
      map(x => <CustomerCasesStatusModel>{ ...x })
    );
  }
  
}
