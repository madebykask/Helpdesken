import { Component, Input, ViewChild, Renderer2 } from '@angular/core';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { BaseControl } from '../base-control';
import { TranslateService } from '@ngx-translate/core';
import { Channels, CommunicationService, NotifierChangedEvent, FormValueChangedEvent } from 'src/app/services/communication';
import { NotifierService } from 'src/app/modules/case-edit-module/services/notifier.service';
import { take, debounceTime, switchMap } from 'rxjs/operators';
import { Subject, of } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotifierSearchItem, NotifierType } from 'src/app/modules/shared-module/models/notifier/notifier.model';
import { CaseFieldsNames } from 'src/app/modules/shared-module/constants';

@Component({
  selector: 'notifier-search',
  templateUrl: './notifier-search.component.html',
  styleUrls: ['./notifier-search.component.scss']
})
export class NotifierSearchComponent extends BaseControl<string> {

  constructor(private notifierService: NotifierService,
    private commService: CommunicationService,
    private ngxTranslateService: TranslateService,
    private renderer: Renderer2) {
    super();
  }

  @ViewChild('notifierInput') notifierInput: any; //MbscInput
  @ViewChild('notifierSelect') notifierSelect: MbscSelect;
  @ViewChild('searchButton') searchButton: any;

  @Input() notifierType: NotifierType;
  @Input('categoryField') categoryFieldName: string;

  notifiersData: any[] = [];

  private progressIconEl: any = null;
  private usersSearchSubject = new Subject<string> ();

  selectOptions: MbscSelectOptions = {
    input: '#notifierInput',
    showOnTap: false,
    circular: false,
    theme: 'mobiscroll',
    cssClass: 'search-list',
    filter: true,
    display: 'center',
    maxWidth: 400,
    multiline: 2,
    buttons: ['cancel'],
    headerText: () => this.getHeaderText(),
    cancelText: this.ngxTranslateService.instant('Avbryt'),
    setText: this.ngxTranslateService.instant('Välj'),
    filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
    filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),

    onShow: (event, inst) => {
      //setting filter text from user input on case page
      const filterText = (this.notifierInput.element.value || '').trim();
      if (filterText && filterText.length) {
        const el = event.target.querySelector<HTMLInputElement>('input.mbsc-sel-filter-input');
        if (el) {
          el.value = filterText;
          el.nextElementSibling.classList.add('mbsc-sel-filter-show-clear');
          setTimeout(() => this.usersSearchSubject.next(filterText), 200);
          //const ev = new Event('input', { bubbles: true });
          //el.dispatchEvent(ev);
        }
      }
    },

    onBeforeClose: (event, inst) => {
      this.notifiersData = [];
    },

    onFilter: (event, inst) => {
      const filterText = (event.filterText || '').trim();
      this.usersSearchSubject.next(filterText);
      // Prevent built-in filtering
      return false;
    },

    onMarkupReady: (event: { target: HTMLElement }, inst: any) => {
      this.createProgressIcon(event.target);
    },

    onSet: (event, inst) => {
      const val = +inst.getVal();
      this.processSelectedItem(val);
    }
  };

  ngOnInit() {
    this.init(this.field);
    this.updateDisabledState();

    this.initEvents();

    const categoryFormControl = this.getFormControl(this.categoryFieldName);
    let categoryId = categoryFormControl && categoryFormControl.value ? +categoryFormControl.value : 0;
    if (isNaN(categoryId)) categoryId = 0; // 0 - no category, null - all categories

    // subscribe to notifier(user) search input
    this.usersSearchSubject.asObservable().pipe(
      takeUntil(this.destroy$),
      debounceTime(150),
      //distinctUntilChanged(),
      switchMap((query: string) => {
        if (query && query.length > 1) {
          this.toggleProgress(true);
          return this.notifierService.searchNotifiers(query, categoryId);
        } else {
          return of(null);
        }
      })
    ).subscribe((data: NotifierSearchItem[]) => {
        this.toggleProgress(false);
        if (data && data.length) {
          this.notifiersData =
            data.map(item => {
              return {
                value: item.id,
                text: `${item.userId} - ${item.name || ''} - ${item.email}`,
                html: '<div class="select-li">' + `${item.userId} - ${item.name || ''} - ${(item.email || '').toLowerCase()}` + '</div>'
              };
            });
        } else {
          this.notifiersData = [];
        }
    });
  }

  private updateDisabledState() {
    this.notifierInput.disabled = this.formControl ? this.isFormControlDisabled : false;
  }

  protected processSelectedItem(val) {
    const notfierId = +val;
    if (!isNaN(notfierId) && notfierId > 0) {
      this.onNotifierSelected(notfierId);
    } else {
      this.onNotifierSelected(null);
    }
  }

  private onNotifierSelected(userId: number) {
    if (userId != null) {
      this.notifierService.getNotifier(userId).pipe(
        take(1)
      ).subscribe(x => {
        //todo: change to be handled in reducers!!!
        this.commService.publish(Channels.NotifierChanged, new NotifierChangedEvent(x, this.notifierType));

        // raise event to be handled in reducers
        this.commService.publish(Channels.FormValueChanged, new FormValueChangedEvent(x.email, x.email, CaseFieldsNames.PersonEmail));
      });
    } else {
      //todo: change to be handled in reducers!!!
      this.commService.publish(Channels.NotifierChanged, new NotifierChangedEvent(null, this.notifierType));

      // raise event to be handled in reducers
      this.commService.publish(Channels.FormValueChanged, new FormValueChangedEvent('', '', CaseFieldsNames.PersonEmail));
    }
  }

  private initEvents() {
    this.formControl.statusChanges // track disabled state in form
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((e: any) => {
        if (this.notifierInput.disabled !== this.isFormControlDisabled) {
          this.updateDisabledState();
        }
      });
  }

  private getHeaderText() {
    const defaultText = this.ngxTranslateService.instant('Användar ID');
    return this.formControl ? this.formControl.label || defaultText : defaultText;
  }

  private createProgressIcon(selectNode: HTMLElement) {
    const progressSpan = this.renderer.createElement('span');
    progressSpan.id = 'notifierProgress';
    progressSpan.className = 'notifierProgress mbsc-ic';
    progressSpan.innerHTML = '<img src="content/img/bars.gif" border="0" />'
    progressSpan.style.display = 'none';

    const filterNode = selectNode.querySelector<HTMLElement>('.mbsc-sel-filter-cont');
    this.progressIconEl = filterNode.appendChild(progressSpan);
  }

  private toggleProgress(show) {
    if (this.progressIconEl) {
      this.progressIconEl.style.display = show ? '' : 'none';
    }
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }

}

