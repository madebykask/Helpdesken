import { Component, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SearchInputBaseComponent } from '../search-input-base/search-input-base.component';
import { UserService } from '../../../../services/user.service';
import { CaseUserSearchGroup } from '../../../../logic/constants/case-user-search-group';
import { EmailsSearchItem } from '../../../../services/model/emails-search-item';
import { isValidEmail } from 'src/app/modules/shared-module/Utils/common-methods';

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

    this.selectOptions.select = 'multiple';
    this.selectOptions.buttons = ['cancel'];
    this.selectOptions.cancelText = this.ngxTranslateService.instant('Stäng');
  }

  private searchResults: EmailsSearchItem[] = [];

  ngOnInit() {
    this.initComponent();
  }

  onTextChanged($event) {
    const e = $event;
    const value = this.formControl.value;

    if (value && value.length) {
      const emails = [];
      const items = value.split(';');
      if (items.length) {
        for (const item of items) {
          if (isValidEmail(item)) {
            emails.push(item);
          }
        }
      }
      this.formControl.setValue(emails.length ? emails.join(';') + ';' : '');
    }
  }

  protected searchData(query: any) {
    const sr = this.userService.searchUsersEmails(query);
    return sr;
  }

  protected processSearchResults(data: EmailsSearchItem[]) {
    this.searchResults = data;
    return data.map(item => {
      const itemText = this.formatItemText(item).toLowerCase();
      return {
        value: item.id,
        text: itemText,
        html: '<div class="select-li">' + itemText + '</div>'
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
            if (isValidEmail(email) && fieldEmails.indexOf(email) === -1) {
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

  private formatItemText(item: EmailsSearchItem) {
    const groupName = this.getSearchGroupName(item.groupType);
    const name = item.firstName + ' ' + item.surName;
    const email = item.emails.length ? item.emails[0] : '';
    const userId = item.userId != null ? item.userId + ' - ' : '';
    const result = '<b>' + groupName + '</b>' + ': ' + name + ' - ' + userId + email + ' - ' + item.departmentName;
    return result;
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


