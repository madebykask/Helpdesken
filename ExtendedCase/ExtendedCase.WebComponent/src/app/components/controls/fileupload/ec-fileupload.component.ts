import { BaseControl } from '../base-control';
import { Component, Input, ChangeDetectorRef, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ComponentCommService } from '@app/services/component-comm.service';
import { SingleControlFieldModel } from '@app/models/form.model';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders, FileLikeObject } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { AppConfig } from '@app/shared/app-config/app-config';
import { IAppConfig } from '@app/shared/app-config/app-config.interface';
import { CaseFileModel } from '@app/models/case-file.model';
import { CaseFilesService } from '@app/services/case-files.service';
import { LogService } from '@app/services/log.service';

@Component({
  selector: 'ec-fileupload',
  templateUrl: './ec-fileupload.component.html',
  styleUrls: ['./ec-fileupload.component.css']
})

export class ExtendedCaseFileUploadComponent extends BaseControl {
  @Input() fieldModel: SingleControlFieldModel;
  @Input() form: FormGroup;
  @Input() whiteList: string[] = [];

  fileUploader = new FileUploader({});
  hasBaseDropZoneOver = false;

  constructor(@Inject(AppConfig) private config: IAppConfig,
              componentCommService: ComponentCommService,
              changeDetector: ChangeDetectorRef,
              private caseFilesService: CaseFilesService,
              private logService: LogService) {
      super(componentCommService, changeDetector);
  }

  ngOnInit(): void {
/*     this.caseFileApiService.getFileUploadWhiteList().pipe(
      take(1),
    ).subscribe((whiteList: string[]) => {
      this.whiteList = whiteList;
    }); */

    // init file uploader
    this.fileUploader.setOptions(<FileUploaderOptions>{
      autoUpload: true,
      // filters: [ { name: 'IsInWhiteList', fn: (o) => this.isInWhiteList(o.name, this.whiteList) } ],
      isHTML5: true,
      // authToken: accessToken,
      url: this.getUploadUrl()
    });

    // subscribe to events
    // this.fileUploader.onBeforeUploadItem = this.onBeforeUpload.bind(this);
    // this.fileUploader.onBuildItemForm = this.processFileUploadRequest.bind(this);
    this.fileUploader.onCompleteItem = this.onFileUploadComplete.bind(this);
    this.fileUploader.onWhenAddingFileFailed = this.onWhenAddingFileFailed.bind(this);
    this.fileUploader.onErrorItem = this.onError.bind(this);

    this.validateAndUpdateFiles();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  getStatusStyles(item: FileItem) {
    const style = {
      'color': '#c0c0c0'
    };

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

  identify(item: CaseFileModel) {
    return item.fileId;
  }

  downloadFile(item: CaseFileModel) {
    // const caseId = +this.caseGuid;
    ((!isNaN(this.formInfo.caseId) && this.formInfo.caseId > 0) ?
     this.caseFilesService.downloadCaseFile(this.formInfo.caseId, item.fileId, this.formInfo.caseNumber, this.formInfo.customerId) :
     this.caseFilesService.downloadTempCaseFile(this.formInfo.caseGuid, item.fileName, this.formInfo.customerId))
     .subscribe(b => {
       this.download(b, item);
     });
  }

  deleteFile(item: CaseFileModel) {
    (item.fileId && item.fileId > 0 ?
      this.caseFilesService.deleteFile(this.formInfo.caseId, item.fileId, this.formInfo.customerId, this.formInfo.caseNumber, item.fileName, '') :
      this.caseFilesService.deleteTempFile(this.formInfo.caseGuid, item.fileName, ''))
      .subscribe(() => {
        let files = this.files;
        const fileIndex = files.findIndex(i => item.fileName === i.fileName);
        if (fileIndex >= 0) {
          files.splice(fileIndex, 1);
          this.setFiles(files);
          this.changeDetector.markForCheck();
        }
      });
  }

  deleteError(item: FileItem) {
    this.fileUploader.removeFromQueue(item);
  }

  get files() {
    return this.fieldModel.control.value ? JSON.parse(this.fieldModel.control.value) as Array<CaseFileModel> : new Array<CaseFileModel>();
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

  private download(fileData: Blob, file: CaseFileModel) {
    let newBlob = new Blob([fileData], { type: 'application/octet-stream' });

    // IE doesn't allow using a blob object directly as link href
    // instead it is necessary to use msSaveOrOpenBlob
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(newBlob);
      return;
    }

    // For other browsers:
    // Create a link pointing to the ObjectURL containing the blob.
    const data = window.URL.createObjectURL(newBlob);

    let link = document.createElement('a');
    link.href = data;
    link.download = file.fileName;
    // this is necessary as link.click() does not work on the latest firefox
    link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

    setTimeout(function () {
      // For Firefox it is necessary to delay revoking the ObjectURL
      window.URL.revokeObjectURL(data);
      link.remove();
    }, 100);
  }

  private validateAndUpdateFiles() {
    this.logService.debug(`Upload control:${this.fieldModel.id}. ValidateAndUpdateFiles`);
    if (this.formInfo.caseId > 0 && this.formInfo.caseFiles != null) { // sync files only for existing case
      const ecfiles = this.files.filter(ef => {
        const caseFile = this.formInfo.caseFiles.find(cf => cf.fileName === ef.fileName);
        const caseFileExists = caseFile != null;
        if (caseFileExists && ef.fileId === 0 && caseFile.fileId !== 0) { // if file from case got id after case save - update ex file with id
          ef.fileId = caseFile.fileId;
          this.logService.debug(`Upload control:${this.fieldModel.id}: file id updated: ${ef.fileName}`)
        } else if (!caseFileExists && ef.fileId !== 0) { // if file from case get deleted in case, remove them in ex also
          this.logService.debug(`Upload control:${this.fieldModel.id}: file removed: ${ef.fileName}`)
          return false;
        }
        return true;
      });
      this.setFiles(ecfiles);
    }
  }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
    // console.log(`File upload complete. File: ${fileItem.file.name}, IsSuccess: ${fileItem.isSuccess}, Response: ${response}`);
    if (fileItem.isUploaded && fileItem.isSuccess) {
        fileItem.remove(); // remove success files only
        const data = JSON.parse(response);
        if (data) {
          let files = this.files;
          if (typeof data === 'string') {
            files.push(new CaseFileModel(0, data)); // temp file upload returns file name only
            // this.NewFileUploadComplete.emit({ id: 0, name: data });
          } else {
            files.push(new CaseFileModel(data.id, data.name));
            // this.NewFileUploadComplete.emit({id: data.id, name: data.name });
          }
          this.setFiles(files);
        }
    }
    this.changeDetector.markForCheck();
  }

  private setFiles(files: CaseFileModel[]) {
    this.fieldModel.setControlValue(files && files.length ? JSON.stringify(files) : '');
  }

  private processFileUploadDelete(fileItem: FileItem) {
    if (fileItem.isUploading) {
      fileItem.cancel();
    }
    fileItem.remove();
  }

  private onWhenAddingFileFailed(item: FileLikeObject, filter: any, options: any): any {
    alert(`Failed loading file ${item.name}.`);
  }

  private onError(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
    if (status === 413) {
      alert(`Error. File is too big: ${item.file.name}.`);
    } else if (status === 403) {
      alert(`Error. ${item.file.name} has invalid file extension.`);
    } else {
      alert(`Error. Failed loading file ${item.file.name}.`);
    }
  }

  private getUploadUrl() {
    // return `${this.config.apiHost}/api/files/${this.formInfo.caseGuid}/file`;
    return this.formInfo.caseId > 0 ?
     `${this.config.apiHost}/api/files/${this.formInfo.caseId}/file?cid=${this.formInfo.customerId}&caseNumber=${this.formInfo.caseNumber || ''}&userName=${this.formInfo.currentUser || ''}` :
     `${this.config.apiHost}/api/files/${this.formInfo.caseGuid}/file`;
  }
}
