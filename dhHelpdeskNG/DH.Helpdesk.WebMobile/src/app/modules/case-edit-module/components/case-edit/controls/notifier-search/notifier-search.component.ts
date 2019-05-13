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
    this.searchResults = data;
    const notifiersData =
      data.map(item => {
        const itemHeader = this.formatItemHeader(item, query);
        const itemDesc = this.formatItemDesc(item, query);
        return {
          value: item.id,
          text: `${item.userId} - ${item.name || ''} - ${item.email}`,
          html: `<div>
                  <div class="itemHeader">${itemHeader} </div>
                  <div class="itemDesc">${itemDesc}</div>
                </div>`
        };
      });
    return notifiersData;
  }

  private formatItemHeader(item: NotifierSearchItem, query: string) {
    const userId = item.userId != null ? item.userId + ' - ' : '';
    let result = userId  + (item.name || '');

    // highlight searched text with  bold
    result = this.highligtQueryText(result, query);
    return result;
  }

  private formatItemDesc(item: NotifierSearchItem, query) {
    const email = (item.email || '').toLowerCase();
    if (email && email.length) {
      this.highligtQueryText(email, query);
    }
    return email;
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

