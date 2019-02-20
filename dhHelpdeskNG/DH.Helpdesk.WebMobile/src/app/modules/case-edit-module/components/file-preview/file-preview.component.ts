import { Component, OnInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { CommunicationService, HeaderEventData, Channels } from 'src/app/services/communication';

@Component({  
  templateUrl: './file-preview.component.html',
  styleUrls: ['./file-preview.component.scss']
})
export class FilePreviewComponent implements OnInit {
  previewHeight:number = 0;
  isPdf:boolean = false;
  
  fileData:any = null;
  fileName:string = '';

  constructor(private activatedRoute:ActivatedRoute,
              private router: Router,
              private commService: CommunicationService,
              private windowWrapper: WindowWrapper) {
      this.fileName = this.activatedRoute.snapshot.queryParams.fileName || '';
      this.fileData = this.activatedRoute.snapshot.data['fileData'];
      this.isPdf = this.fileName && this.fileName.indexOf('.pdf') > 0
   }
 
  ngOnInit() {
    this.commService.publish(Channels.Header, new HeaderEventData(false));
    this.updatePreviewHeight();
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
