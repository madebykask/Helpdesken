import { Component, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SearchInputBaseComponent } from '../search-input-base/search-input-base.component';
import { UserService } from '../../../../services/user.service';
import { CaseUserSearchGroup } from '../../../../logic/constants/case-user-search-group';
import { EmailsSearchItem } from '../../../../services/model/emails-search-item';
import * as cm from 'src/app/modules/shared-module/Utils/common-methods';

@Component({
  selector: 'lognote-email-input',
  templateUrl: './lognote-email-input.component.html',
  styleUrls: ['../search-input-base/search-input-base.component.scss']
})
export class LognoteEmailInputComponent extends SearchInputBaseComponent {

  constructor(private userService: UserService,
    ngxTranslateService: TranslateService,
    renderer: Renderer2) {
    super(ngxTranslateService, renderer);
    //override select settings from base component
    this.selectOptions.height = 48;
    this.selectOptions.rows = 5;
    this.selectOptions.multiline = 3;
  }

  private searchResults: EmailsSearchItem[] = [];

  ngOnInit() {
    this.initComponent();
  }

  onTextChanged($event) {
    const e = $event;
    const value = (this.formControl.value || '').toString();

    const searchQuery = this.getFilterText(value);
    super.setFilterText(searchQuery);

    if (value && value.length) {
      const emails = [];
      const items = value.split(';');
      if (items.length) {
        for (const item of items) {
          if (cm.isValidEmail(item)) {
            emails.push(item);
          }
        }
      }
      this.formControl.setValue(emails.length ? emails.join(';') + ';' : '');
    }
  }

  private getFilterText(val: string) {
    let res = '';
    if (val && val.length) {
      const items = val.split(';');
      if (items.length > 0) {
        const text = items.pop();
        if (text && cm.isValidEmail(text) === false) {
          // use filter text only if its not a valid email
          res = text;
        }
      }
    }
    return res;
  }

  protected searchData(query: any) {
    const sr = this.userService.searchUsersEmails(query);
    return sr;
  }

  protected processSearchResults(data: EmailsSearchItem[], query: string) {
    this.searchResults = data;
    return data.map(item => {
      const itemHeader = this.formatItemHeader(item, query);
      const itemDesc = this.formatItemDesc(item, query);
      return {
        value: item.id,
        text: item.userId,
        html: `<div>
                 <div class="itemHeader">${itemHeader} </div>
                 <div class="itemDesc">${itemDesc}</div>
               </div>`
      };
    });
  }

  processItemSelected(selectedVal, isSelected: boolean): void {
    const id = +selectedVal;
    if (!isNaN(id) && id > 0) {
      let fieldEmails: string[] = (this.formControl.value || '').split(';').filter(x => !!x).map(x => x.toLowerCase());
      const item = this.searchResults.find(x => x.id === id);
      if (item && item.emails && item.emails.length) {
        for (let email of item.emails) {
          email = email.toLowerCase();
          if (isSelected) {
            //add selected item emails
            if (cm.isValidEmail(email) && fieldEmails.indexOf(email) === -1) {
              fieldEmails.push(email.toLowerCase());
            }
          } else {
            //remove selected item emails
            fieldEmails = fieldEmails.filter(x => x !== email);
          }
        }
      }
      const emailsValue = fieldEmails.length ? `${fieldEmails.join(';')};` : '';
      this.formControl.setValue(emailsValue.toLowerCase());
    }
  }

  private formatItemHeader(item: EmailsSearchItem, query: string) {
    const groupName = this.getSearchGroupName(item.groupType);
    const name = item.firstName + ' ' + item.surName;
    const userId = item.userId != null ? item.userId + ' - ' : '';

    let result = (name + ' - ' + userId + item.departmentName);

    // highlight searched text with  bold
    result = this.highligtQueryText(result, query);

    const resultWithPrefix = groupName + ': ' + result;
    return resultWithPrefix;
  }

  private formatItemDesc(item: EmailsSearchItem, query) {
    let result = '';
    if (item.emails && item.emails.length) {
        const emails = [...item.emails];
        if (emails.length > 1) {
          result = `${emails.shift()}...${emails.pop()}`;
        } else {
          result = emails.pop();
        }
    }
    // highlight searched text with  bold
    result = this.highligtQueryText(result, query);
    return result.toLowerCase();
  }

  private getSearchGroupName(searchGroupType) {
    let groupName = '';
    switch (+searchGroupType) {
      case CaseUserSearchGroup.Initiator:
        groupName = this.ngxTranslateService.instant('Anmälare');
        break;
      case CaseUserSearchGroup.Administaror:
        groupName = this.ngxTranslateService.instant('Handläggare');
        break;
      case CaseUserSearchGroup.WorkingGroup:
        groupName = this.ngxTranslateService.instant('Driftgrupp');
        break;
      case CaseUserSearchGroup.EmailGroup:
        groupName = this.ngxTranslateService.instant('E-postgrupp');
        break;
      case CaseUserSearchGroup.Users:
        groupName = this.ngxTranslateService.instant('Användare');
        break;
      default:
        groupName = 'uknown';
        break;
    }
    return groupName;
  }
}


