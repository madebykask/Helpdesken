import { Component, OnInit, Input } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { saveAs } from 'file-saver';

@Component({
  styles: [`
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
  object {
    position:absolute;
    overflow:hidden;
    min-height:100%;
    min-width: 100%;
    width:100%;
    height:100%;
  }
  `],
  selector: 'pdf2-file-viewer',
  template: `
    <div class="pdfWrapper">
      <object [attr.data]="content | sanitize:'resourceUrl'" type="application/pdf">
        <iframe [src]="content | sanitize:'resourceUrl'">
            <p>Your browser does not support PDFs. <a (click)="downloadPdf()">Download the PDF</a>.</p>
        </iframe>
      </object>
    </div>
  `
})
export class Pdf2FileViewerComponent implements OnInit {

  @Input() fileName: string;
  @Input() fileData: Blob;

  content: any;

  constructor(private windowWrapper: WindowWrapper) {
  }

  ngOnInit(): void {
    this.content = this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData); // +"#view=FitW";
  }

  downloadPdf() {
    saveAs(this.fileData, this.fileName);
  }

}
