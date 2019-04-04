import { Component, OnInit } from '@angular/core';
import { CustomerCasesStatusModel } from 'src/app/modules/shared-module/models/home/customerCasesStatus.model';
import { HomeService } from 'src/app/services/home/home.service';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { MbscListviewOptions } from '@mobiscroll/angular';

@Component({
  selector: 'cases-status',
  templateUrl: './cases-status.component.html',
  styleUrls: ['./cases-status.component.scss']
})
export class CasesStatusComponent implements OnInit {

  casesStatus: CustomerCasesStatusModel = null;
  options: MbscListviewOptions = {
    fillAnimation: false,
    animateIcons: false,
    animateAddRemove: false,
    swipe: false,
    select: 'off'
  };

  constructor(private homeService: HomeService,
    private router: Router) {
  }

  ngOnInit() {
    this.homeService.getCustomerCasesStatus().pipe(
      take(1)
    ).subscribe(data => this.casesStatus = data);
  }

  goTo(url: string = null) {
    this.router.navigateByUrl(url);
  }
}
