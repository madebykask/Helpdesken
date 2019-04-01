import { Component, OnInit } from '@angular/core';
import { config } from '@env/environment';

@Component({
  selector: 'version',
  template: `<div class="version">Version: {{ version }}</div>`,
  styles: [ `
  .version {
    margin:20px;
    color:#a0a0a0;
    font-size:12px;
  }` ]
})
export class VersionComponent implements OnInit {
  version = config.version;
  
  constructor() { }

  ngOnInit() {
  }

}
