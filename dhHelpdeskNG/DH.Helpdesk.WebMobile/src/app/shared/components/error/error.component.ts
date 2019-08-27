import { Component, OnInit, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {
  pageSettings = {};
  today: Date = new Date();
  errorGuid: string;

  constructor(private ngZone: NgZone, private activatedRoute: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
      this.errorGuid = this.activatedRoute.snapshot.queryParamMap.get('errorGuid');
  }

  navigate(url: string) {
    if (url == null) { return; }
    this.ngZone.run(() => this.router.navigate([url])).then();
  }
}
