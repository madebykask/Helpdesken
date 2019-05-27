import { Component, OnInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { CommunicationService, HeaderEventData, Channels } from 'src/app/services/communication';

export enum ContentTypes {
  None = 0,
  Pdf = 1,
  Image,
  TextHtml,
  Other
}

@Component({
  templateUrl: './file-preview.component.html',
  styleUrls: ['./file-preview.component.scss']
})
export class FilePreviewComponent implements OnInit {
  previewHeight = 0;

  contentType: ContentTypes;
  contentTypes = ContentTypes;

  fileData: Blob = null;
  fileName = '';

  constructor(private activatedRoute: ActivatedRoute,
              private router: Router,
              private commService: CommunicationService,
              private windowWrapper: WindowWrapper) {
      this.fileName = this.activatedRoute.snapshot.queryParams.fileName || '';
      this.fileData = <Blob>this.activatedRoute.snapshot.data['fileData'];
   }

  ngOnInit() {
    this.contentType = this.getContentType();
    this.commService.publish(Channels.Header, new HeaderEventData(false));
    this.updatePreviewHeight();
  }

  private getContentType(): ContentTypes {
    const contentType: string = this.fileData.type;
    if (contentType === 'application/pdf') {
      return ContentTypes.Pdf;
    } else if (contentType.match(/^image\//gi)) {
      return ContentTypes.Image;
    } else if (contentType.match(/^text\//gi)) {
      return ContentTypes.TextHtml;
    }
    return ContentTypes.Other;
  }

  goBack() {
    const paramMap = this.activatedRoute.snapshot.paramMap;
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;
    const caseId = +paramMap.get('caseId');
    if (!isNaN(caseId) && caseId > 0) {
      this.router.navigate(['/case', caseId]);
    } else if (paramMap.has('caseKey')) {
      // at the moment there's only one way of creating new cases - via new template page
      if (queryParamMap.has('templateId')) {
        const templateId = +queryParamMap.get('templateId');
        this.router.navigate(['/case/template', templateId]);
      } else {
        this.router.navigate(['/case', paramMap.get('caseKey')]);
      }
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.updatePreviewHeight();
  }

  private updatePreviewHeight() {
    this.previewHeight = this.windowWrapper.nativeWindow.innerHeight - 44; //footer menu width
  }

}
