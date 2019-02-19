import { Component, ViewChild, ElementRef, Input, AfterViewInit, OnInit } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { PdfViewerComponent } from 'ng2-pdf-viewer';

@Component({
  selector: 'pdf-file-viewer',
  template: `
      <pdf-viewer [src]="blobUrl"
                  [original-size]="true" 
                  [autoresize]="false"
                  [render-text]="false"></pdf-viewer>
  `,
  styles: [``]
})
export class PdfFileViewer implements OnInit {
  
  @Input() fileName:string;
  @Input() fileData:Blob;
  
  blobUrl:any;

  @ViewChild(PdfViewerComponent) private pdfComponent: PdfViewerComponent; 

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit(): void {
    this.blobUrl = this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
  }

}
