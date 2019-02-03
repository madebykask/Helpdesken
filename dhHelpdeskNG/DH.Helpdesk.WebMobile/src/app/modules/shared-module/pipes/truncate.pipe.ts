import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number, replaceNewLine:boolean = true, trail:string): string {
    let result = value || '';
    if (value) {
      
      let text =  replaceNewLine ? value.replace(/\n/g, '<br>\n') : value;
      const words = text.split(/\s+|\n/);
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
