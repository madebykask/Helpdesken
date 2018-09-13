import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../../../../environments/environment'
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss']
})
export class HeaderComponent implements OnInit {

  hamburgerSettings: any = {
    theme: 'ios',
    type: 'hamburger'
  };

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  sendTestRequest(event) {
    this.http.get<any>(`${config.apiUrl}/api/test/anyrole`)
    .subscribe();//TODO: Remove - test only
  }
}
