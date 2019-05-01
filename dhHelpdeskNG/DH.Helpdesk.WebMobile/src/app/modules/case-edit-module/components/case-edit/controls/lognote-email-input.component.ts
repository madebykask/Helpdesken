import { Component, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SearchInputBaseComponent } from './search-input-base/search-input-base.component';
import { UserService } from '../../../services/user.service';
import { CaseUserSearchGroup } from '../../../logic/constants/case-user-search-group';
import { EmailsSearchItem } from '../../../services/model/emails-search-item';

@Component({
  selector: 'lognote-email-input',
  templateUrl: './search-input-base/search-input-base.component.html',
  styleUrls: ['./search-input-base/search-input-base.component.scss']
})
export class LognoteEmailInputComponent extends SearchInputBaseComponent {

  constructor(private userService: UserService,
    ngxTranslateService: TranslateService,
    renderer: Renderer2) {
      super(ngxTranslateService, renderer);
  }

  private searchResults: EmailsSearchItem[] = [];

  ngOnInit() {
    this.initComponent();
  }

  protected searchData(query: any) {
    const sr = this.userService.searchUsersEmails(query);
    return sr;
  }

  protected processSearchResults(data: EmailsSearchItem[]) {
      this.searchResults = data;
      return data.map(item => {
          const itemText = this.formatText(item);
          return {
            value: item,
            text: itemText,
            html: '<div class="select-li">' + itemText + '</div>'
          };
        });
  }

  processItemSelected(val: any): void {
     if (val && val.emails && val.emails.length) {
        const emailsText = val.emails.join(';');
        this.formControl.setValue(emailsText);
     }
  }

  private formatText(item: EmailsSearchItem) {
    const groupName = this.getSearchGroupName(item.groupType);
    const name = item.firstName + ' ' + item.surName;
    const email = item.emails.length ? item.emails[0] : '';
    const userId = item.userId != null ? item.userId + ' - ' : '';
    const result = groupName + ': ' + name + ' - ' + userId + email + ' - ' + item.departmentName;
    return result;
  }

  private getSearchGroupName(searchGroupType) {
    /*
        adminLabel: '@Translation.GetForJS("Handläggare")',
        emailLabel: '@Translation.GetForJS("E-postgrupp")',
        initLabel: '@Translation.GetForJS("Anmälare")',
        wgLabel: '@Translation.GetForJS("Driftgrupp")',
        usersLabel: '@Translation.GetForJS("Användare")',
    */
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
        break;
    }
    return groupName;
  }
}


