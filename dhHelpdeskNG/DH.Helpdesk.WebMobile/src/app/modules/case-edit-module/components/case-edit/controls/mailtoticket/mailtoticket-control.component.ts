import { Component, Input } from '@angular/core';
import { BaseControl } from '../base-control';
import { IKeyValue } from 'src/app/modules/case-edit-module/models';

@Component({
  selector: 'case-mailtoticket-control',
  templateUrl: './mailtoticket-control.component.html',
  styleUrls: ['./mailtoticket-control.component.scss']
})
export class MailtoticketControlComponent extends BaseControl<number> {
  mailTO: string[];
  mailCC: string[];
  @Input() value;
  @Input() options;

  ngOnInit(): void {
    //Email
    if (this.value === 3) {
        this.mailTO = this.getEmails('to', this.options);
        this.mailCC = this.getEmails('cc', this.options);
    }
  }

  private getEmails(emailType: string, options: IKeyValue[]): string[] {
    let emails = [];
    if (options && options.length) {
      const val = options.find(m => m.key.toLowerCase() === emailType);
      if (val && val.value.length){
        emails = [...val.value.split(';')];
      }
    }
    return emails;
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }
}
