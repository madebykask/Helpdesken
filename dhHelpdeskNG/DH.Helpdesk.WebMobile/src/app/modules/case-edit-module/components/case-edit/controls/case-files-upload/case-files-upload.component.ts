import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders } from 'ng2-file-upload' 
import { config } from '@env/environment';
import { AuthenticationService } from 'src/app/services/authentication';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { LocalStorageService } from 'src/app/services/local-storage';

@Component({
  selector: 'case-files-upload',
  templateUrl: './case-files-upload.component.html',
  styleUrls: ['./case-files-upload.component.scss']
})
export class CaseFilesUploadComponent {
  
  @Output() NewFileUploadComplete: EventEmitter<any> = new EventEmitter<any>();

  @Input('caseKey') caseKey: string;
  
  fileUploader = new FileUploader({});

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    // swipe effects for cancel or delete
    stages: [{
      percent: -25,
      color: 'red',
      text: 'Delete', // todo: translate
      confirm: true,
      action: (event, inst) => {
        let itemIndex = +event.index;
        if (this.fileUploader.queue && this.fileUploader.queue.length > itemIndex) {
          let fileItem = this.fileUploader.queue[event.index] as FileItem;
          this.processFileUploadDelete(fileItem);
        }
      }
    }]  
  }; 

  constructor(private authenticationService: AuthenticationService,
              private localStateStorage: LocalStorageService) {
  } 

  ngOnInit() {
    // console.log('>>> file-upload.onInit: called. CaseKey: %s', this.caseKey);
        
    const accessToken = this.authenticationService.getAuthorizationHeaderValue();
    const userData = this.localStateStorage.getCurrentUser();
    const cid = userData.currentData.selectedCustomerId;

    // init file uploader 
    this.fileUploader.setOptions(<FileUploaderOptions>{
      autoUpload: true,
      filters: [],
      isHTML5: true,
      authToken: accessToken,
      url: `${config.apiUrl}/api/case/${this.caseKey}/file?cid=${cid}` //todo:replace with shared method call
    });

    // subscribe to events
    this.fileUploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    this.fileUploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
    this.fileUploader.onCompleteItem = this.onFileUploadComplete.bind(this);

    // FileUploader events:
    // this.uploader.onAfterAddingFile(fileItem: FileItem): any;
    // this.uploader.onAfterAddingAll(fileItems: any): any;
    // this.uploader.onProgressItem(fileItem: FileItem, progress: any): any;
    // onCompleteItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any;
    // this.uploader.onSuccessItem
    // this.uploader.onErrorItem
    // this.uploader.onProgressAll(progress: any): any;
    // this.uploader.onCompleteAll
  } 
 
  private processFileUploadRequest(fileItem: FileItem, form: FormData) {
    // console.log('processFileUploadRequest called. CaseKey: %s', this.caseKey);
    //let fi = fileItem;
    //let f = form;
  }

  private onBeforeUpload(fileItem: FileItem) {
      //let fi = fileItem;
  }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
    // console.log(`File upload complete. File: ${fileItem.file.name}, IsSuccess: ${fileItem.isSuccess}, Response: ${response}`);
    if (fileItem.isUploaded && fileItem.isSuccess) {
        fileItem.remove();// remove success files only
        var data = JSON.parse(response);
        if (data && data.hasOwnProperty("id")) {
            this.NewFileUploadComplete.emit({id: data.id, name: data.name });
        }
    }
  }

  private processFileUploadDelete(fileItem: FileItem) {
    //todo: check different upload states handling!!! (queued, ready, uploading, error, canceled)
    if (fileItem.isUploading) {
      fileItem.cancel();
    }
    fileItem.remove();
  }

  getStatusStyles(item:FileItem) {
    let style = {
      'color': '#c0c0c0'
    };

    /* 
    // FileItem available status fields:
    this.isReady = true;
    this.isUploading = true;
    this.isUploaded = false;
    this.isSuccess = false;
    this.isCancel = false;
    this.isError = false;
    */

    // todo: review - use css class instead ?
    if (item) {
     if (item.isUploaded) {
        if (item.isError) {
          style.color = '#f5504e';
        } else if (item.isCancel) {
          style.color = '#f8b042';
        }
      }
    }
    return style;
  }
}