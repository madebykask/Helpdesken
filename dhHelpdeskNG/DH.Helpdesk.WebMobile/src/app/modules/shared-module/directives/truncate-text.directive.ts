import { Input, Directive, ElementRef, HostListener } from '@angular/core';
import { TruncatePipe } from '../pipes/truncate.pipe';

@Directive({
  selector: '[truncate]',
  exportAs: "truncate",
  providers: [ TruncatePipe ]
})
export class TruncateTextDirective {

  @Input("truncate") words: number;
  @Input("truncate-symbol") trail: string = '...';

  canTruncate: boolean = false;
  isTruncated:boolean = false;

  private originalText:string;
  private nativeEl;

  constructor(private elementRef: ElementRef, private truncatePipe: TruncatePipe) {
      this.nativeEl = elementRef.nativeElement;
  }

  ngAfterViewInit(): void {
    this.originalText = this.nativeEl.textContent || '';
    setTimeout(() => this.truncateInner(), 50);
  }

  truncate() {
    this.truncateInner();
  }

  showText(){
    if (this.isTruncated){
      this.setText(this.originalText);
      this.isTruncated = false;
    }
  }

  private truncateInner() {
    if (this.words < 1) return false;
  
    let text = this.truncatePipe.transform(this.originalText, this.words, null);
    this.isTruncated = text.length < this.originalText.length;
    if (this.isTruncated)
    {
        if (this.trail && this.trail.length)  text += this.trail;
        this.setText(text);
        this.canTruncate = true;
    }
  }

  private setText(text){
    this.nativeEl.innerText = text;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.truncateInner();
  }
}