import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders } from 'ng2-file-upload'
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { LocalStorageService } from 'src/app/services/local-storage/local-storage.service';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts';
import { CaseLogApiService } from 'src/app/modules/case-edit-module/services/api/case/case-log-api.service';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';

@Component({
  selector: 'log-files-upload',
  templateUrl: './log-files-upload.component.html',
  styleUrls: ['./log-files-upload.component.scss']
})
export class LogFilesUploadComponent {

  @Input() caseKey: string;
  @Input() type: LogFileType;
  @Output() fileUploaded: EventEmitter<FileUploadArgs> = new EventEmitter<FileUploadArgs>();

  fileUploader = new FileUploader({});
  id: string;

  constructor(private authenticationService: AuthenticationService,
              private localStateStorage: LocalStorageService,
              private caseLogApiService: CaseLogApiService,
              private alertsService: AlertsService) {
  }

  ngOnInit() {
    const accessToken = this.authenticationService.getAuthorizationHeaderValue();
    const userData = this.localStateStorage.getCurrentUser();
    const cid = userData.currentData.selectedCustomerId;
    this.id = `logfileupload${this.type}`;

    // init file uploader
    this.fileUploader.setOptions(<FileUploaderOptions> {
      autoUpload: true,
      filters: [],
      isHTML5: true,
      authToken: accessToken,
      url: this.caseLogApiService.getUploadLogFileUrl(this.caseKey, cid, this.type)
    });

    // subscribe to events
    //this.fileUploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    //this.fileUploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
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

  //private processFileUploadRequest(fileItem: FileItem, form: FormData) { let fi = fileItem; }
  //private onBeforeUpload(fileItem: FileItem) { let fi = fileItem; }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
    //console.log(`File upload complete. File: ${fileItem.file.name}, IsSuccess: ${fileItem.isSuccess}, Response: ${response}`);
    if (fileItem.isUploaded && fileItem.isSuccess) {
        const file = JSON.parse(response);
        if (file) {
            this.fileUploaded.emit(new FileUploadArgs(file, this.type));
            fileItem.file.name = file;
        }
        fileItem.remove(); // remove file from upload queue
    } else if (!fileItem.isSuccess) {
        const data = JSON.parse(response);
        const  msg = data.Message || '';
        this.alertsService.showMessage('Unknown error.' + msg, AlertType.Error);
    }
  }
}

export class FileUploadArgs {
  constructor(public file: string, public type: LogFileType) {}
}
