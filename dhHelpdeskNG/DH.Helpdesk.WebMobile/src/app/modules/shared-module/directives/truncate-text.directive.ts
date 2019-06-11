import { Input, Directive, ElementRef, HostListener } from '@angular/core';
import { TruncatePipe } from '../pipes/truncate.pipe';
import { SanitizePipe } from '../pipes/sanitize.pipe';

@Directive({
  selector: '[truncate]',
  exportAs: 'truncate',
  providers: [ TruncatePipe, SanitizePipe ]
})
export class TruncateTextDirective {

  @Input('truncate') words: number;
  @Input('truncate-symbol') trail = '...';

  canTruncate = false;
  isTruncated = false;

  private originalText: string;
  private nativeEl;

  constructor(private elementRef: ElementRef,
              private truncatePipe: TruncatePipe,
              private sanitizeHtmlPipe: SanitizePipe) {
      this.nativeEl = elementRef.nativeElement;
  }

  ngAfterViewInit(): void {
    this.originalText = this.Text;
    setTimeout(() => this.truncateInner(), 50);
  }

  truncate() {
    this.truncateInner();
  }

  showText() {
    if (this.isTruncated) {
      this.Text = this.originalText;
      this.isTruncated = false;
    }
  }

  private truncateInner() {
    if (this.words < 1) { return false; }

    let text = this.truncatePipe.transform(this.originalText, this.words, true, null);
    this.isTruncated = text.length < this.originalText.length;
    if (this.isTruncated) {
        if (this.trail && this.trail.length) {
          text += this.trail;
        }
        this.Text = text;
        this.canTruncate = true;
    }
  }

  private get Text() {
    return this.nativeEl.innerHTML || '';
  }

  private set Text(text) {
    this.nativeEl.innerHTML = text;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.truncateInner();
  }
}
