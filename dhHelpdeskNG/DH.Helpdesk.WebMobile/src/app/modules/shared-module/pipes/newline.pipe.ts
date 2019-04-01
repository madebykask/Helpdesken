import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'newline'
})
export class NewlinePipe implements PipeTransform {
  transform(value: any): any {
    value = value.replace(/(?:\r\n\r\n|\r\r|\n\n)/g, '</p><p>');
    value = value.replace(/(?:\r\n|\r|\n)/g, '<br>');
    return value;
  }
}
