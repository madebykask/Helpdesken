import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders, FileLikeObject } from 'ng2-file-upload'
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { LocalStorageService } from 'src/app/services/local-storage/local-storage.service';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts';
import { CaseLogApiService } from 'src/app/modules/case-edit-module/services/api/case/case-log-api.service';
import { LogFileType } from 'src/app/modules/shared-module/constants/logFileType.enum';
import { CaseFilesApiService } from 'src/app/modules/case-edit-module/services/api/case/case-files-api.service';
import { take } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'log-files-upload',
  templateUrl: './log-files-upload.component.html',
  styleUrls: ['./log-files-upload.component.scss']
})
export class LogFilesUploadComponent {

  @Input() caseKey: string;
  @Input() customerId: number;
  @Input() type: LogFileType;
  @Output() fileUploaded: EventEmitter<FileUploadArgs> = new EventEmitter<FileUploadArgs>();

  fileUploader = new FileUploader({});
  id: string;
  whiteList: string[];

  constructor(private authenticationService: AuthenticationService,
              private localStateStorage: LocalStorageService,
              private caseLogApiService: CaseLogApiService,
              private alertsService: AlertsService,
              private caseFileApiService: CaseFilesApiService,
              private translateService: TranslateService) {
  }

  ngOnInit() {
    const accessToken = this.authenticationService.getAuthorizationHeaderValue();
    const userData = this.localStateStorage.getCurrentUser();
    this.id = `logfileupload${this.type}`;

    this.caseFileApiService.getFileUploadWhiteList().pipe(
      take(1),
    ).subscribe((whiteList: string[]) => {
      this.whiteList = whiteList;
    });


    // init file uploader
    this.fileUploader.setOptions(<FileUploaderOptions> {
      autoUpload: true,
      filters: [{ name: 'IsInWhiteList', fn: (o) => this.isInWhiteList(o.name, this.whiteList) }],
      isHTML5: true,
      authToken: accessToken,
      url: this.caseLogApiService.getUploadLogFileUrl(this.caseKey, this.customerId, this.type)
    });

    // subscribe to events
    //this.fileUploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    //this.fileUploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
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

  private onWhenAddingFileFailed(item: FileLikeObject, filter: any, options: any): any {
    alert(item.name + ' ' + this.translateService.instant('har inte en giltig filändelse'));
  }

  private isInWhiteList(item: string, whiteList: string[]): Boolean {
    if (whiteList != null) {
      const lastDot = item.lastIndexOf('.');
      if (lastDot >= 0 && item.length > (lastDot + 1)) {
        const extension = item.substring(lastDot + 1).toLowerCase();

        if (whiteList.indexOf(extension) >= 0) {
          return true;
        }
      }
      return false;
    }
    return true;
  }

}

export class FileUploadArgs {
  constructor(public file: string, public type: LogFileType) {}
}
