import { Component, OnInit } from '@angular/core';
import { config } from '@env/environment';

@Component({
  selector: 'alt-layout',
  templateUrl: './alt-layout.component.html',
  styleUrls: ['./alt-layout.component.scss']
})
export class AltLayoutComponent implements OnInit {
  version = config.version;
  constructor() { }

  ngOnInit() {
  }

}