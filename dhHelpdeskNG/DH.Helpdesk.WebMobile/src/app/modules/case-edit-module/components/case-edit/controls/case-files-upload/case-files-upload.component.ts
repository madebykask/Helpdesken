import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { FileItem, FileUploader, FileUploaderOptions, ParsedResponseHeaders, FileLikeObject } from 'ng2-file-upload';
import { CaseFilesApiService } from 'src/app/modules/case-edit-module/services/api/case/case-files-api.service';
import { AuthenticationService } from 'src/app/services/authentication';
import { map, take } from 'rxjs/operators';

@Component({
  selector: 'case-files-upload',
  templateUrl: './case-files-upload.component.html',
  styleUrls: ['./case-files-upload.component.scss']
})
export class CaseFilesUploadComponent {

  @Output() NewFileUploadComplete: EventEmitter<any> = new EventEmitter<any>();
  @Input('caseKey') caseKey: string;
  @Input('customerId') customerId: number;

  whiteList: string[] = [];

  fileUploader = new FileUploader({});

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: true,
    // swipe effects for cancel or delete
    stages: [{
      percent: -25,
      color: 'red',
      text: 'Delete',
      confirm: true,
      action: (event, inst) => {
        const itemIndex = +event.index;
        if (this.fileUploader.queue && this.fileUploader.queue.length > itemIndex) {
          const fileItem = this.fileUploader.queue[event.index] as FileItem;
          this.processFileUploadDelete(fileItem);
        }
      }
    }]
  };

  constructor(private authenticationService: AuthenticationService,
              private caseFileApiService: CaseFilesApiService) {
  }

  ngOnInit() {
    const accessToken = this.authenticationService.getAuthorizationHeaderValue();
    this.caseFileApiService.getFileUploadWhiteList().pipe(map((o: any) => { 
      alert (o);
      return this.buildWhiteList(o); 
    }));

    // init file uploader
    this.fileUploader.setOptions(<FileUploaderOptions>{
      autoUpload: true,
      filters: [ { name: 'IsInWhiteList', fn: this.isInWhiteList } ],
      isHTML5: true,
      authToken: accessToken,
      url: this.caseFileApiService.getCaseFileUploadUrl(this.caseKey, this.customerId)
    });

    // subscribe to events
    this.fileUploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    this.fileUploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
    this.fileUploader.onCompleteItem = this.onFileUploadComplete.bind(this);
    this.fileUploader.onWhenAddingFileFailed = this.onWhenAddingFileFailed.bind(this);

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
      //let wait = true;
      //return false;
  }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
    // console.log(`File upload complete. File: ${fileItem.file.name}, IsSuccess: ${fileItem.isSuccess}, Response: ${response}`);
    if (fileItem.isUploaded && fileItem.isSuccess) {
        fileItem.remove(); // remove success files only
        const data = JSON.parse(response);
        if (data) {
          if (typeof data === 'string') {
            this.NewFileUploadComplete.emit({ id: 0, name: data }); //temp file upload returns file name only
          } else {
            this.NewFileUploadComplete.emit({id: data.id, name: data.name });
          }
        }
    }
  }

  private processFileUploadDelete(fileItem: FileItem) {
    if (fileItem.isUploading) {
      fileItem.cancel();
    }
    fileItem.remove();
  }

  private onWhenAddingFileFailed(item: FileLikeObject, filter: any, options: any) : any {
    alert(`File extension not supported for: ${item.name}`);
  }

  private isInWhiteList(item: any) : Boolean {
    return true;
  }

  private buildWhiteList(item: any) {
    return true;
  }

  getStatusStyles(item: FileItem) {
    const style = {
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
