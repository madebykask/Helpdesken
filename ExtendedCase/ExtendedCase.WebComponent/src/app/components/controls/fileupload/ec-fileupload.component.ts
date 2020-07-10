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
    if (!isNaN(this.formInfo.caseId) && this.formInfo.caseId > 0) {
      const queryParams = {
        cid: this.formInfo.customerId
      };
/*       this.router.navigate(['/case', this.caseId, 'file', item.fileId], {
        queryParams: queryParams
      }); */
    } else {
      const queryParams = {
        fileName: item.fileName,
        cid: this.formInfo.customerId
      };
/*       const templateId = +this.activatedRoute.snapshot.paramMap.get('templateId');
      if (!isNaN(templateId) && templateId > 0) {
        queryParams['templateId'] = templateId;
      } */
/*       this.router.navigate(['/case', this.caseGuid, 'file'], {
        queryParams: queryParams
     }); */
    }
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

  get files() {
    return this.fieldModel.control.value ? JSON.parse(this.fieldModel.control.value) as Array<CaseFileModel> : new Array<CaseFileModel>();
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
    this.fieldModel.setControlValue(JSON.stringify(files));
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

  private getUploadUrl() {
    // return `${this.config.apiHost}/api/files/${this.formInfo.caseGuid}/file`;
    return this.formInfo.caseId > 0 ?
     `${this.config.apiHost}/api/files/${this.formInfo.caseId}/file?cid=${this.formInfo.customerId}&caseNumber=${this.formInfo.caseNumber || ''}&userName=${this.formInfo.currentUser || ''}` :
     `${this.config.apiHost}/api/files/${this.formInfo.caseGuid}/file`;
  }
}
