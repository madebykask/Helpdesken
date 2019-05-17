import { Component, Input, Renderer2, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Channels, CommunicationService, CaseFieldValueChangedEvent } from 'src/app/services/communication';
import { NotifierService } from 'src/app/modules/case-edit-module/services/notifier.service';
import { NotifierSearchItem, NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';
import { SearchInputBaseComponent } from '../search-input-base/search-input-base.component';

@Component({
  selector: 'notifier-search',
  templateUrl: '../search-input-base/search-input-base.component.html',
  styleUrls: ['../search-input-base/search-input-base.component.scss']
})
export class NotifierSearchComponent extends SearchInputBaseComponent implements OnInit {

  @Input() notifierType: NotifierType;
  @Input('categoryField') categoryFieldName: string;

  private selectedCategoryId = 0;
  private searchResults: NotifierSearchItem[] = [];

  constructor(private notifierService: NotifierService,
    private commService: CommunicationService,
    ngxTranslateService: TranslateService,
    renderer: Renderer2) {
    super(ngxTranslateService, renderer);

    //override select settings from base component
    this.selectOptions.headerText = this.getSelectHeaderText.bind(this);
    this.selectOptions.height = 40;
    this.selectOptions.rows = 5;
    this.selectOptions.multiline = 2;
  }

  ngOnInit() {
    //init base component
    this.initComponent();

    const categoryFormControl = this.getFormControl(this.categoryFieldName);
    this.selectedCategoryId = categoryFormControl && categoryFormControl.value ? +categoryFormControl.value : 0;
    if (isNaN(this.selectedCategoryId)) { this.selectedCategoryId = 0; } // 0 - no category, null - all categories
  }

  // virtual method override
  protected searchData(query: any) {
    const sr = this.notifierService.searchNotifiers(query, this.selectedCategoryId)
    return sr;
  }

  // virtual method override
  protected processSearchResults(data: NotifierSearchItem[], query: string) {
    this.searchResults = this.sortSearchResults(data, query);
    const notifiersData =
      this.searchResults.map((item: NotifierSearchItem) => {
        const itemHeader = this.formatItemHeader(item, query);
        const itemDesc = this.formatItemDesc(item, query);
        console.log(itemHeader);
        console.log(itemDesc);
        return {
          value: item.id,
          text: `${item.userId}|${item.fullName}|${item.email}|${item.phone}|${item.userCode}`,
          html: `<div>
                  <div class="itemHeader">${itemHeader}</div>
                  <div class="itemDesc">${itemDesc}</div>
                </div>`
        };

      });
    return notifiersData;
  }

  private sortSearchResults(data: NotifierSearchItem[], query: string): NotifierSearchItem[] {
    const startsWithItems = [],
          caseSensetiveItems = [],
          otherItems = [];

    const qs = query.toLowerCase();

    for (const item of data) {
      const startsAt = (item.userId || '').indexOf(qs);
      if (startsAt === 0) {
        startsWithItems.push(item);
      } else if (startsAt > 0) {
        caseSensetiveItems.push(item);
      } else {
        otherItems.push(item);
      }
    }
    return [...startsWithItems, ...caseSensetiveItems, ...otherItems];
  }

  private formatItemHeader(item: NotifierSearchItem, query: string) {
    let name = `${item.firstName} ${item.surName}`.trim();
    if (item.surName && item.surName.indexOf(query) > -1) {
      name = `${item.surName} ${item.firstName}`.trim();
    }

    let result =  `${name || ''} - ${item.userId || ''} - ${item.departmentName || ''} - ${item.userCode || ''}`;

    // highlight searched text with  bold
    result = this.highligtQueryText(result, query);
    return result;
  }

  private formatItemDesc(item: NotifierSearchItem, query) {
    const email = (item.email || '').toLowerCase();
    const phone = item.phone || '';
    let result = phone.length ? `${email} - ${phone}` : email;

    // highlight searched text with  bold
    result = this.highligtQueryText(result, query);

    return result;
  }

  // virtual method override
  protected processItemSelected(val) {
    const eventData = new CaseFieldValueChangedEvent((val || '').toString(), this.notifierType.toString(), CaseFieldsNames.PerformerUserId);
    this.commService.publish(Channels.CaseFieldValueChanged, eventData);
  }

  private getSelectHeaderText() {
    const defaultText = this.ngxTranslateService.instant('Användar ID');
    return this.formControl ? this.formControl.label || defaultText : defaultText;
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }

}

