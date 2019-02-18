import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { MbscNavOptions } from '@mobiscroll/angular';
import { CommunicationService, HeaderEventData, Channels } from 'src/app/services/communication';

@Component({  
  templateUrl: './file-preview.component.html',
  styleUrls: ['./file-preview.component.scss']
})
export class FilePreviewComponent implements OnInit {
  previewHeight:number = 0;
  fileData:any = null;

  @ViewChild('frame') frameElement: ElementRef<HTMLIFrameElement>;

  bottomMenuSettings: MbscNavOptions = <MbscNavOptions>{  
    type: 'bottom',
    moreText: null,
    menuIcon: null,
    menuText: null
  };

  constructor(private activatedRoute:ActivatedRoute,
              private router: Router,
              private commService: CommunicationService,
              private windowWrapper: WindowWrapper) {
   }

  ngOnInit() {
    this.commService.publish(Channels.Header, new HeaderEventData(false));
    this.updatePreviewHeight();
  }

  ngAfterViewInit(): void {
    let fileData = this.activatedRoute.snapshot.data['fileData'];
    if (fileData !== null) {
      this.frameElement.nativeElement.src = 
          this.windowWrapper.nativeWindow.URL.createObjectURL(fileData);
    }
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
