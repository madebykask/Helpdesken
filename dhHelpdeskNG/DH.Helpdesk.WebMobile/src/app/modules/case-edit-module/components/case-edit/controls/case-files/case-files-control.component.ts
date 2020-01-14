import { Component, Input, SimpleChanges, ViewChild } from '@angular/core';
import { MbscListviewOptions, mobiscroll, MbscListview } from '@mobiscroll/angular';
import { take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CaseFileModel, CaseAccessMode } from 'src/app/modules/case-edit-module/models';

import { CaseFilesApiService } from 'src/app/modules/case-edit-module/services/api/case/case-files-api.service';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts/alert-types';
import { CaseFilesUploadComponent } from '../case-files-upload/case-files-upload.component';
import { TranslateService as NgxTranslateService } from '@ngx-translate/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
  selector: 'case-files-control',
  templateUrl: './case-files-control.component.html',
  styleUrls: ['./case-files-control.component.scss']
})
export class CaseFilesControlComponent {

  @Input() field: string;
  @Input() files: CaseFileModel[];
  @Input() caseKey: string;
  @Input() customerId: number;
  @Input() accessMode: CaseAccessMode;

  fileListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };

  constructor(private caseFilesApiService: CaseFilesApiService,
              private translateService: NgxTranslateService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private userSettingsService: UserSettingsApiService,
              private alertsService: AlertsService) {
  }

  get hasFullAccess() {
    return this.accessMode === CaseAccessMode.FullAccess;
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.configureListActions();
  }

  ngOnDestroy(): void {
  }

  processNewFileUpload(data: { id: number, name: string }) {
    if (data) {
        this.files.push(new CaseFileModel(data.id, data.name));
    }
  }

  downloadFile(item: CaseFileModel) {
    const caseId = +this.caseKey;
    if (!isNaN(caseId) && caseId > 0) {
      const queryParams = {
        cid: this.customerId
      };
      this.router.navigate(['/case', caseId, 'file', item.fileId], {
        queryParams: queryParams
     });
    } else {
      const queryParams = {
        fileName: item.fileName,
        cid: this.customerId
      };
      const templateId = +this.activatedRoute.snapshot.paramMap.get('templateId');
      if (!isNaN(templateId) && templateId > 0) {
        queryParams['templateId'] = templateId;
      }
      this.router.navigate(['/case', this.caseKey, 'file'], {
        queryParams: queryParams
     });
    }
  }

  onFileDelete(event, inst) {
    const self = this;
    const index = +event.index;
    if (!isNaN(index) && this.files.length > index) {
        const fileItem = self.files[index];
        if (fileItem) {
              // todo: move confirm to a separate service!
              mobiscroll.confirm({
                title: '',
                display: 'bottom',
                message: this.translateService.instant('Är du säker på att du vill ta bort bifogad fil') + '?', // do you want to delete attached file?
                okText: this.translateService.instant('Ja'),
                cancelText: this.translateService.instant('Nej'),
              }).then(function (result) {
                  if (result) {
                      self.caseFilesApiService.deleteCaseFile(self.caseKey, fileItem.fileId, fileItem.fileName, this.customerId).pipe(
                          take(1),
                          untilDestroyed(this)
                      ).subscribe(() => {
                          // remove fileItem from the list on success only
                          self.files = self.files.filter(el => el !== fileItem);
                      },
                      (err) => {
                        self.alertsService.showMessage('Failed to delete a file', AlertType.Error);
                        console.error(err);
                      });
                  }
            });
        }
    }
  }

  identify(item) {
    return item.fileId;
  }

  private configureListActions() {
    if (this.userSettingsService.getUserData().canDeleteAttachedFiles) {
      // add swipe actions if has permissions
      this.fileListSettings = {
        swipe: true,
        stages: [{
          percent: -25,
          color: 'red',
          icon: 'fa-trash',
          confirm: true,
          action: this.onFileDelete.bind(this)
        }]
      };
    }
  }
}
