import { Component, OnInit, Input } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

@Component({
  selector: 'image-file-viewer',
  template: `
      <img #img [src]="blobUrl | sanitize:'url'" style="max-width:100%;max-height:100%" border="0" />
  `
})
export class ImageFileViewerComponent implements OnInit {

  @Input() fileName: string;
  @Input() fileData: Blob;

  blobUrl: Blob;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit() {
    this.blobUrl =  this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
  }

}
