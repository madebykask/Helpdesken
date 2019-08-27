import { Pipe } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser'

@Pipe({ name: 'safeStyle' })
export class SafeStyle {
    constructor(private sanitizer: DomSanitizer) { }

    transform(style: string) {
        return this.sanitizer.bypassSecurityTrustStyle(style);
    }
}