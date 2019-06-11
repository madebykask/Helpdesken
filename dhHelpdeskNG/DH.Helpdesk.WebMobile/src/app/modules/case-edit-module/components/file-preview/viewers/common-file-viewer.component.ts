import { Component, ViewChild, ElementRef, Input, AfterViewInit } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

@Component({
  selector: 'common-file-viewer',
  template: `
    <div class="iframe-image">
        <iframe #frame id="filePreview" style="max-width:100%;max-height:100%;border: none;"></iframe>
    </div>
  `
})
export class CommonFileViewer implements AfterViewInit {

  @Input() fileName: string;
  @Input() fileData: Blob;

  @ViewChild('frame') frameElement: ElementRef<HTMLIFrameElement>;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngAfterViewInit(): void {
    if (this.fileData && this.frameElement) {
      this.frameElement.nativeElement.src =
          this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
    }
  }

}
