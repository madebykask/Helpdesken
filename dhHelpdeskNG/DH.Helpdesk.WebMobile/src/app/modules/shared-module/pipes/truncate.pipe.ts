import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number, trail: string): string {
    let result = value || '';
    if (value) {
      const words = value.split(/\s+/);
      if (words.length > Math.abs(limit)) {
          result = words.slice(0, limit).join(' ');
          if (trail && trail.length){
            result += trail;
          }
      }
    }
    return result;
  }
}
