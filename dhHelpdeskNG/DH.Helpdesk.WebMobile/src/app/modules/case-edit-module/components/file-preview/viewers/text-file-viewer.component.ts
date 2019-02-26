import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'text-file-viewer',
  template: `
    <div [innerHTML]="content | sanitize:'html'" class="mbsc-padding">
    </div>
  `
})
export class TextFileViewerComponent implements OnInit {

  @Input() fileName:string;
  @Input() fileData:Blob;
  
  content:string;
  
  constructor() { }

  ngOnInit() {
    const fileReader = new FileReader();
    fileReader.onload = () => { 
      this.content = fileReader.result.toString() || '';
    };

    fileReader.readAsText(this.fileData);
  }

}
