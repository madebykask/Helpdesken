import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../../../../environments/environment'
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  hamburgerSettings: any = {
    theme: 'auto',
    type: 'hamburger'
  };

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  sendTestRequest(event) {
    this.http.get<any>(`${config.apiUrl}/api/test/anyrole`)
    .subscribe();
  }
}
