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
    this.selectOptions.buttons = ['set', 'cancel'];
  }

  private searchResults: EmailsSearchItem[] = [];

  ngOnInit() {
    this.initComponent();
  }

  protected onTextChanged($event) {
    const e = $event;
    const value = e.target.value;

    if (value && value.length) {
      const emails = [];
      const items = value.split(';');
      //check one item
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

  processItemSelected(ids: any[]): void {
    //todo: check array res
    if (ids && ids.length) {
      let emails: string = this.formControl.value || '';
      for (const id of ids) {
        const item: EmailsSearchItem = this.searchResults.find(x => x.id === id);
        if (item && item.emails && item.emails.length) {
          for (const email of item.emails) {
            if (isValidEmail(email)) {
              emails += email + ';';
            }
          }
        }
      }
      this.formControl.setValue(emails.toLowerCase());
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


