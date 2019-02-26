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
  previewHeight:number = 0;  
  
  contentType: ContentTypes;
  contentTypes = ContentTypes;

  fileData:Blob = null;
  fileName:string = '';

  constructor(private activatedRoute:ActivatedRoute,
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
    let contentType: string = this.fileData.type;
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
    let caseId = this.activatedRoute.snapshot.params['caseId'];
    this.router.navigate(['/case', caseId]);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event:any): void {
    this.updatePreviewHeight();
  }

  private updatePreviewHeight() {
    this.previewHeight = this.windowWrapper.nativeWindow.innerHeight - 44; //footer menu width
  }

}
