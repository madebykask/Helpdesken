import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {
  pageSettings = {};
  today:Date = new Date();
  errorGuid: string;

  constructor(private activatedRoute: ActivatedRoute) { 
  }

  ngOnInit() {
      this.errorGuid = this.activatedRoute.snapshot.queryParamMap.get("errorGuid");
  }

}
