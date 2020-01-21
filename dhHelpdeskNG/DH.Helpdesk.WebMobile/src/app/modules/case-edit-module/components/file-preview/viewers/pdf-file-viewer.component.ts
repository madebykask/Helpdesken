import { Component, ViewChild, ElementRef, Input, AfterViewInit, OnInit } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

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
export class PdfFileViewerComponent implements OnInit {

  @Input() fileName: string;
  @Input() fileData: Blob;

  blobUrl: any;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit(): void {
    this.blobUrl = this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
  }

}
