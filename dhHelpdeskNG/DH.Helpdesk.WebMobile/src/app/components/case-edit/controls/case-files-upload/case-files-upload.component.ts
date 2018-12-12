import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders } from 'ng2-file-upload' 
import { config } from '@env/environment';
import {  AuthenticationService } from 'src/app/services/authentication';
import { MbscListviewOptions } from '@mobiscroll/angular';

@Component({
  selector: 'case-files-upload',
  templateUrl: './case-files-upload.component.html',
  styleUrls: ['./case-files-upload.component.scss']
})
export class CaseFilesUploadComponent implements OnInit {
  
  @Input('caseKey') caseKey: string;

  fileListSettings:MbscListviewOptions = {
    enhance: true,
    swipe: true,
    //todo: add swipe effects for cancel or delete   
    stages: [{
      percent: -25,
      color: 'red',
      text: 'Delete',
      confirm: true,
      action: (event, inst) => {
          console.log('delete file');
      }
    }]  
  };

  uploader:FileUploader = new FileUploader({});

  constructor(private authenticationService: AuthenticationService) {            
  }

  getStatusStyles(item:FileItem) {
    let style = {};

    /*
    this.isReady = true;
    this.isUploading = true;
    this.isUploaded = false;
    this.isSuccess = false;
    this.isCancel = false;
    this.isError = false;
    */

    if (item) {
      if (item.isUploading) {
        style = {
          'color': '#c0c0c0'
        };
      } else if (item.isUploaded) {        
        if (item.isSuccess) {
          style = {
            'font-weight':'500',
            'color': '#43BE5F'
          };
        } else if (item.isError) {
          style = {
            'font-weight':'500',
            'color': '#f5504e'
          };        
        } else if (item.isCancel) {
          style = {
            'font-weight':'500',
            'color': '#f8b042'
          };
        }         
      } //uploaded 
    }
    return style;
  }

  ngOnInit() {    
    //console.log('>>> file-upload.onInit: called. CaseKey: %s', this.caseKey);    
        
    let accessToken = this.authenticationService.getAuthorizationHeaderValue();    
    
    //init file uploader 
    this.uploader.setOptions(<FileUploaderOptions>{       
      autoUpload: true,
      filters: [],
      isHTML5: true,
      authToken: accessToken,
      url: `${config.apiUrl}/api/case/${this.caseKey}/uploadfile`
    });      

    //subscribe to events
    this.uploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    this.uploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
    this.uploader.onCompleteItem = this.onFileUploadComplete.bind(this);
    
    //other events: 
    //this.uploader.onAfterAddingFile(fileItem: FileItem): any;
    //this.uploader.onAfterAddingAll(fileItems: any): any;
    //this.uploader.onProgressItem(fileItem: FileItem, progress: any): any;    
    //onCompleteItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any;
    //this.uploader.onSuccessItem 
    //this.uploader.onErrorItem      
    //this.uploader.onProgressAll(progress: any): any;
    //this.uploader.onCompleteAll
  }

  private processFileUploadRequest(fileItem: FileItem, form: FormData) {
    //console.log('processFileUploadRequest called. CaseKey: %s', this.caseKey);
    let fi = fileItem;
    let f = form;
  }

  private onBeforeUpload(fileItem: FileItem) {
      let fi = fileItem;
  }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders){
    //console.log('file upload complete: ' + fileItem.alias);
  } 

    
}
