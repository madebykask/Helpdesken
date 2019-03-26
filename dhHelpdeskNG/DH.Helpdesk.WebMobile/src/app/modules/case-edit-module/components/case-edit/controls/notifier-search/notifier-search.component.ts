import { Component, Input, ViewChild } from '@angular/core';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { BaseControl } from '../base-control';
import { TranslateService } from '@ngx-translate/core';
import { Channels, CommunicationService, NotifierChangedEvent } from 'src/app/services/communication';
import { NotifierService } from 'src/app/modules/case-edit-module/services/notifier.service';
import { take, debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';
import { Subject, of } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotifierSearchItem, NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { FormStatuses } from 'src/app/modules/shared-module/constants';

@Component({
  selector: 'notifier-search',
  templateUrl: './notifier-search.component.html',
  styleUrls: ['./notifier-search.component.scss']
})
export class NotifierSearchComponent extends BaseControl<string> {

  @ViewChild('notifierInput') notifierInput: any;
  @ViewChild('notifierSelect') notifierSelect: MbscSelect;
  @Input() disabled = false;
  @Input() notifierType: NotifierType;

  notifiersData: any[] = [];

  private usersSearchSubject = new Subject<string> ();

  selectOptions: MbscSelectOptions = {
    input: "#notifierInput",
    showOnTap: false,
    circular: false,
    theme:"mobiscroll",
    cssClass: "search-list",
    filter: true,
    display: "center",
    maxWidth: 400,
    multiline: 2,
    buttons: ['cancel'],
    headerText: () => this.getHeaderText(),
    cancelText: this.ngxTranslateService.instant("Avbryt"),
    setText: this.ngxTranslateService.instant("Välj"),
    filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
    filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),
    // Methods
    onFilter: (event, inst) => {
      const filterText = event.filterText || '';
      this.usersSearchSubject.next(filterText);
      // Prevent built-in filtering
      return false;
    },

    onMarkupReady: (event: { target: HTMLElement }, inst:any) => {
      this.createProgressIcon(event.target);
    },
   
    onSet: (event, inst) => {
      let val = +inst.getVal();
      if (!isNaN(val) && val != 0) {
        this.onNotifierSelected(val);
      }
      else {
        this.onNotifierSelected(null);
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
      debounceTime(150),
      distinctUntilChanged(),
      switchMap((query:string) => {
        if (query && query.length > 1) {
          this.toggleProgress(true);
          return this.notifierService.searchNotifiers(query, categoryId)
        } else {
          return of(null);
        }
      })
    ).subscribe((data:Array<NotifierSearchItem>) => {
        this.toggleProgress(false);
        if (data && data.length) {
          this.notifiersData = 
            data.map(item => {
              return {
                value: item.id,
                text: `${item.userId} - ${item.name || ''} - ${item.email}`,
                html: '<div class="select-li">' + `${item.userId} - ${item.name || ''} - ${(item.email || '').toLowerCase()}` + '</div>'
              }
            });
        } else {
          this.notifiersData = [];
        }
        //add empty
        this.notifiersData.unshift({ value: '', text: ''});
    });
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
    if (userId != null){
      this.notifierService.getNotifier(userId).pipe(
        take(1)
      ).subscribe(x => {
        //raise event to handle notfier change on case edit component
        this.commService.publish(Channels.NotifierChanged, new NotifierChangedEvent(x, this.notifierType));
      });
    }
    else {
      this.commService.publish(Channels.NotifierChanged, new NotifierChangedEvent(null, this.notifierType));
    }
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

  private getHeaderText() {
    const defaultText = this.ngxTranslateService.instant("Användar ID");
    return this.field ? this.formControl.label || defaultText : defaultText;
  }

  private createProgressIcon(selectNode: HTMLElement) {
    const progressSpan = document.createElement("span")
    progressSpan.id = 'notifierProgress';
    progressSpan.className = "notifierProgress mbsc-ic";
    progressSpan.innerHTML = '<img src="content/img/bars.gif" border="0" />'
    progressSpan.style.display = "none";
    const filterNode = selectNode.querySelector<HTMLElement>(".mbsc-sel-filter-cont");
    filterNode.appendChild(progressSpan);
  }

  private toggleProgress(show) {
    const progressSpan = document.querySelector<HTMLSpanElement>('#notifierProgress');
    progressSpan.style.display = show ? '' : 'none';
  }

}
