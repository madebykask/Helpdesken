
export class StringUtil {
  static convertToHtml(text: string): string {
    if (!text) { return text; }

    let html = text.replace(/(?:\r\n\r\n|\r\r|\n\n)/g, '</p><p>');
    html = html.replace(/(?:\r\n|\r|\n)/g, '<br>');
    return html;
  }

  static format(template: string, ...args: any[]) {
    return template.replace(/{(\d+)}/g, (match, number) => { 
      return typeof args[number] != 'undefined'
        ? args[number].toString != null ? args[number].toString() : args[number]
        : match
      ;
    });
  }
}
