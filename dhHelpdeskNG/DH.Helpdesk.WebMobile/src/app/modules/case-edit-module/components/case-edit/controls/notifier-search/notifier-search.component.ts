import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { BaseCaseField } from 'src/app/modules/case-edit-module/models';
import { BaseControl } from '../base-control';
import { NotifierApiService } from 'src/app/modules/case-edit-module/services/api/notifier-api.service';
import { TranslateService } from '@ngx-translate/core';
import { Channels, HeaderEventData, CommunicationService } from 'src/app/services/communication';
import { NotifierService } from 'src/app/modules/case-edit-module/services/notifier.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'notifier-search',
  templateUrl: './notifier-search.component.html',
  styleUrls: ['./notifier-search.component.scss']
})
export class NotifierSearchComponent extends BaseControl implements OnInit {

  @ViewChild('notifierSelect') notifierSelect: MbscSelect;
  @Input() field: BaseCaseField<string>;

  selectOptions: MbscSelectOptions = {
    showInput: false,
    showLabel: true,
    showOnTap: true,
    cssClass: "search-list",
    input: "#notifierInput",
    filter: true,
    display: "bottom",
    multiline: 2,
    height: 36,
    data: {
      url: this.notifierApiService.getSearchApiUrl() + '&query=',
      remoteFilter: true,
      dataType: "json",
      processResponse: (data: any) => {
        const res =
          data.map(item => {
            return {
              value: item.id,
              text: `${item.userId} - ${item.name || ''} - ${item.email}`,
              html: '<div style="font-size:12px;line-height:18px;">' + `${item.userId} - ${item.name || ''} - ${item.email}` + '</div>'
            }
          });
        return res;
      }
    },
    onSet: (event, inst) => {
      let val = +inst.getVal();
      if (!isNaN(val)) {
        this.onNotifierSelected(val);
      }
    }
  };

  options: any[] = [];

  constructor(private notifierService: NotifierService,
    private notifierApiService: NotifierApiService,
    private commService: CommunicationService,
    private ngxTranslateService: TranslateService) {
    super();
  }

  ngOnInit() {
    this.init(this.field);
  }

  ngAfterViewInit(): void {
    //set select translations
    this.notifierSelect.instance.option({
      // TODO: translate
      headerText: this.field.label || 'User Id',
      cancelText: this.ngxTranslateService.instant("Avbryt"),
      setText: this.ngxTranslateService.instant("Välj"),
      // can be set in markupReady event handler!
      //filterPlaceholderText: 'Search users',
      //filterEmptyText: 'No results',//	Text for the empty state of the select wheels.    
    });
  }

  private onNotifierSelected(userId:number) {
    //raise event to handle notfier change on case edit component
    this.notifierService.getNotifier(userId).pipe(
      take(1)
    ).subscribe(x => this.commService.publish(Channels.NotifierChanged, x));
  }

}
