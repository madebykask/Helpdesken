import { Component, OnInit, Input, ElementRef, Renderer2, HostListener } from '@angular/core';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';

@Component({
  selector: 'image-file-viewer',
  template: `<pinch-zoom backgroundColor="transparent" #pinch>
      <img #img [src]="blobUrl | sanitize:'url'" style="" border="0" (load)="onImageLoad()"/>
      </pinch-zoom>
  `
})
export class ImageFileViewerComponent implements OnInit {

  @Input() fileName: string;
  @Input() height: string;
  @Input() fileData: Blob;

  blobUrl: Blob;

  constructor(private windowWrapper: WindowWrapper,
    private elem: ElementRef,
    private renderer: Renderer2) {
  }

  ngOnInit() {
    this.blobUrl =  this.windowWrapper.nativeWindow.URL.createObjectURL(this.fileData);
  }

  ngAfterViewInit(): void {
    this.updatePinchStyles();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.updatePinchStyles();
  }

  onImageLoad() {
    // TODO: set pinch max scale depending on images dimensions
  }

  private updatePinchStyles() {
    const pinchContainer = (<HTMLElement>this.elem.nativeElement).querySelector('.pinch-zoom-content');
    this.renderer.setStyle(pinchContainer, 'height', this.getHeight());
  }

  private getHeight() {
    const height = this.height || 'auto';
    return +height > 0 ? height + 'px' : height;
  }

}
