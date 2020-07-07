import { BaseControl } from '../base-control';
import { Component, Input, ChangeDetectorRef, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ComponentCommService } from '@app/services/component-comm.service';
import { SingleControlFieldModel } from '@app/models/form.model';
import { FileUploader, FileUploaderOptions, FileItem, ParsedResponseHeaders, FileLikeObject } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { AppConfig } from '@app/shared/app-config/app-config';
import { IAppConfig } from '@app/shared/app-config/app-config.interface';

@Component({
  selector: 'ec-fileupload',
  templateUrl: './ec-fileupload.component.html',
  styleUrls: ['./ec-fileupload.component.css']
})

export class ExtendedCaseFileUploadComponent extends BaseControl {
  @Input() fieldModel: SingleControlFieldModel;
  @Input() form: FormGroup;

  fileUploader = new FileUploader({});
  customerId: number = 1; // TODO:
  caseGuid: string = '12345678-D62B-4F54-85B3-63A8EFB80782'; // TODO:
  url: string = ''
  caseId: number = 0;

  hasBaseDropZoneOver = false;
  hasAnotherDropZoneOver = false;

  constructor(@Inject(AppConfig) private config: IAppConfig,
              componentCommService: ComponentCommService,
              changeDetector: ChangeDetectorRef) {
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
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  private onFileUploadComplete(fileItem: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
    // console.log(`File upload complete. File: ${fileItem.file.name}, IsSuccess: ${fileItem.isSuccess}, Response: ${response}`);
    if (fileItem.isUploaded && fileItem.isSuccess) {
        fileItem.remove(); // remove success files only
        const data = JSON.parse(response);
        if (data) {
          if (typeof data === 'string') {
            //this.files.push(new CaseFileModel(0, data)); // temp file upload returns file name only
            // this.NewFileUploadComplete.emit({ id: 0, name: data });
          } else {
            //this.files.push(new CaseFileModel(data.id, data.name));
            // this.NewFileUploadComplete.emit({id: data.id, name: data.name });
          }
        }
    }
    this.changeDetector.markForCheck();
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
    return this.caseId > 0 ? `` : `${this.config.apiHost}/api/files/${this.caseGuid}/file`;
  }
}
