import { Component, OnInit } from '@angular/core';
import { CustomerCasesStatusModel } from 'src/app/modules/shared-module/models/home/customerCasesStatus.model';
import { HomeService } from 'src/app/services/home/home.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'cases-status',
  templateUrl: './cases-status.component.html',
  styleUrls: ['./cases-status.component.scss']
})
export class CasesStatusComponent implements OnInit {

  casesStatus: CustomerCasesStatusModel = null;

  constructor(private homeService: HomeService) {
  }

  ngOnInit() {
    this.homeService.getCustomerCasesStatus().pipe(
      take(1)
    ).subscribe(data => this.casesStatus = data);
  }

}
