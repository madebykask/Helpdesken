import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { BaseCaseField } from 'src/app/modules/case-edit-module/models';
import { BaseControl } from '../base-control';
import { TranslateService } from '@ngx-translate/core';
import { Channels, CommunicationService } from 'src/app/services/communication';
import { NotifierService } from 'src/app/modules/case-edit-module/services/notifier.service';
import { take, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subject, of } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotifierSearchItem } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { FormStatuses } from 'src/app/modules/shared-module/constants';

@Component({
  selector: 'notifier-search',
  templateUrl: './notifier-search.component.html',
  styleUrls: ['./notifier-search.component.scss']
})
export class NotifierSearchComponent extends BaseControl implements OnInit {

  @ViewChild('notifierInput') notifierInput: any;
  @ViewChild('notifierSelect') notifierSelect: MbscSelect;
  @Input() field: BaseCaseField<string>;
  @Input() disabled = false;

  notifiersData: any[] = [];

  private usersSearchSubject = new Subject<string> ();

  selectOptions: MbscSelectOptions = {
    //showInput: false,
    circular: false,
    showLabel: false,
    theme:"mobiscroll",
    showOnTap: true,
    cssClass: "search-list dhselect-list",
    inputClass: "noinput",
    input: "#notifierInput",
    filter: true,
    display: "center",
    maxWidth: 400,
    multiline: 2,
    //height: 40,

    onFilter: (event, inst) => {
      const filterText = event.filterText || '';
      this.usersSearchSubject.next(filterText);
      // Prevent built-in filtering
      return false;
    },

    onMarkupReady: (event, inst) => {
      const filterInput = event.target.querySelector<HTMLInputElement>(".mbsc-sel-filter-input");
      filterInput.placeholder = this.ngxTranslateService.instant('Filtrering startar när minst två tecken har angetts');
    },
   
    onSet: (event, inst) => {
      let val = +inst.getVal();
      if (!isNaN(val)) {
        this.onNotifierSelected(val);
      }
    }

  };
  
  constructor(private notifierService: NotifierService,
    private commService: CommunicationService,
    private ngxTranslateService: TranslateService) {
    super();
  }

  ngOnInit() {
    this.init(this.field);
    this.updateDisabledState();

    this.initEvents();
    
    //TODO: when initiator categoryId is implemented - replace with value from dropdown
    const categoryId = 0; // 0 - no category, null - all categories

    // subscribe to notifier(user) search input 
    this.usersSearchSubject.asObservable().pipe(
      takeUntil(this.destroy$),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((query:string) => {
        if (query && query.length > 1) {
          return this.notifierService.searchNotifiers(query, categoryId)
        } else {
          return of(null);
        }
      })
    ).subscribe((data:Array<NotifierSearchItem>) => {
        if (data && data.length) {
          this.notifiersData = 
            data.map(item => {
              return {
                value: item.id,
                text: `${item.userId} - ${item.name || ''} - ${item.email}`,
                html: '<div style="font-size:12px;line-height:18px;">' + `${item.userId} - ${item.name || ''} - ${item.email}` + '</div>'
              }
            });
        } else {
          this.notifiersData = [];
        }
    });
  }
  
  ngAfterViewInit(): void {
    if (this.notifierSelect) {
      //set select translations
      this.notifierSelect.instance.option({
        // TODO: translate
        headerText: this.field.label || 'User Id',
        cancelText: this.ngxTranslateService.instant("Avbryt"),
        setText: this.ngxTranslateService.instant("Välj"),
        // set in markupReady event handler
        //filterPlaceholderText: 'Search users',
        //filterEmptyText: 'No results',//	Text for the empty state of the select wheels.
      });
    }
  } 

  ngOnDestroy(): void {
    this.onDestroy();
  }
  
  public get isFormControlDisabled() {
    return this.formControl.status == FormStatuses.DISABLED;
  }

  private updateDisabledState() {
    this.notifierInput.disabled = this.formControl.disabled || this.disabled;
  }

  private onNotifierSelected(userId:number) {
    this.notifierService.getNotifier(userId).pipe(
      take(1)
    ).subscribe(x => {
      //raise event to handle notfier change on case edit component
      this.commService.publish(Channels.NotifierChanged, x);
    });
  }

  private initEvents() {
    this.formControl.statusChanges // track disabled state in form
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((e: any) => {
        if (this.notifierInput.disabled != this.isFormControlDisabled) {
          this.updateDisabledState();
        }
      });
  }

}
