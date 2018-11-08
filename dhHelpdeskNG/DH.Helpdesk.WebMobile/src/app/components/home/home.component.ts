import { Component, OnInit } from '@angular/core';
import { config } from '@env/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  version = config.version;

  hamburgerSettings: any = {
    type: 'hamburger'
  };

  constructor() { }

  ngOnInit() {
  }
}
