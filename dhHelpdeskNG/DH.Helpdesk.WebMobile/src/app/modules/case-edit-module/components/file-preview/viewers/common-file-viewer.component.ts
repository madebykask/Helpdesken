import { Component, ViewChild, ElementRef, Input, AfterViewInit, OnInit } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

@Component({
  selector: 'common-file-viewer',
  template: `
    <div class="iframe-image">
        <iframe #frame id="filePreview" style="max-width:100%;max-height:100%;border: none;"></iframe>
    </div>
  `
})
export class CommonFileViewerComponent implements OnInit {

  @Input() fileName: string;
  @Input() fileData: Blob;

  @ViewChild('frame', { static: true }) frameElement: ElementRef<HTMLIFrameElement>;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit(): void {
    if (this.fileData && this.frameElement) {
      this.frameElement.nativeElement.src =
          this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
    }
  }

}
