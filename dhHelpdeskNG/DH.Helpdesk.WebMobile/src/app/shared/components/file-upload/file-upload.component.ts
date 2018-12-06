import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload' 
import { config } from '@env/environment';

@Component({
  selector: 'file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  private apiUrl:string = '';
  
  uploader:FileUploader = new FileUploader({ 
      url: config.apiUrl + '/case/uploadfiles'
  });

  constructor() {  
  }

  ngOnInit() {
  }

}
