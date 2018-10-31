import { Component, OnInit } from '@angular/core';
import { config } from '@env/environment';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss']
})
export class AppLayoutComponent implements OnInit {
    version = config.version;
    constructor() { }

    ngOnInit() {
    }

}