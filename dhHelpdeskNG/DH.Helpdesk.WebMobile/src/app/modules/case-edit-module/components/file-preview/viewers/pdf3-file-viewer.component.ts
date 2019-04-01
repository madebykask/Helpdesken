import { Component, OnInit, Input } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

@Component({
  styles:[`
    :host {
      display:block;
      position:relative;
      height:100%;
      width:100%;
    }
    .pdfWrapper {
        position:relative;
        display: table;
        position: relative;
        min-height:100%;
        min-width: 100%;
    }
    iframe,
    embed,
    object {
      position:absolute;
      overflow:hidden;
      min-height:100%;
      min-width: 100%;
    }
  `],
  selector: 'pdf3-file-viewer',
  template: `
    <object [attr.data]="content | sanitize:'resourceUrl'" type="application/pdf" style="max-width:100%;max-height:100%" width="100%" height="100%">
        <embed [attr.src]="content | sanitize:'resourceUrl'" type="application/pdf" style="max-width:100%;max-height:100%" width="100%" height="100%" />
    </object>
  `
})
export class Pdf3FileViewerComponent implements OnInit {

  @Input() fileName:string;
  @Input() fileData:Blob;
  
  content:any;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit(): void {
    this.content = this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
  }

}
