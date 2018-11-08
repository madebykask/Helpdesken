import { Component, OnInit, Input } from '@angular/core';
import { BaseCaseField, KeyValue } from 'src/app/models';
import { BaseControl } from '../base-control';

@Component({
  selector: 'case-mailtoticket-control',
  templateUrl: './mailtoticket-control.component.html',
  styleUrls: ['./mailtoticket-control.component.scss']
})
export class MailtoticketControlComponent 
              extends BaseControl 
              implements OnInit {

  @Input() field: BaseCaseField<number>; 
  
  mailTO: string[];    
  mailCC: string[];

  ngOnInit(): void {      
    //Email
    if (this.field.value == 3) {
        this.mailTO = this.getEmails('to', this.field.options);
        this.mailCC = this.getEmails('cc', this.field.options);
    }
  }    

  private getEmails(emailType: string, options: KeyValue[]): string[] {
    let emails:string[] = [];
    if (options && options.length)
    {
      let val = options.find(m => m.key.toLowerCase() === emailType);
      if (val && val.value.length){
        emails = [...val.value.split(';')];
      }      
    }
    return emails;
  }

  ngOnDestroy(): void {
  } 
}
