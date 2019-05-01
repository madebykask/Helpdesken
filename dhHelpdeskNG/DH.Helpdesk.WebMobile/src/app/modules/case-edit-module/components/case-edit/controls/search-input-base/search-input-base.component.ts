import { ViewChild, Input, Renderer2 } from '@angular/core';
import { MbscSelect, MbscSelectOptions } from '@mobiscroll/angular';
import { Subject, of } from 'rxjs';
import { takeUntil, debounceTime, switchMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { BaseControl } from '../base-control';

export abstract class SearchInputBaseComponent extends BaseControl<string> {

  constructor(protected ngxTranslateService: TranslateService,
    protected renderer: Renderer2) {
    super();
  }

  @ViewChild('searchInput') searchInput: any; // MbscInputBase
  @ViewChild('select') select: MbscSelect;

  @Input() required: boolean;

  @Input( ) set disabled(val) {
    this._disabled = val;
    this.updateDisabledState();
  }

  get disabled() {
    return this.formControl && this.formControl.disabled || this._disabled;
  }

  selectDataItems: any = [];

  get fieldName(): string {
    return this.field;
  }

  private _disabled = false;
  private progressIconEl: any = null;
  protected searchSubject = new Subject<string> ();

  selectOptions: MbscSelectOptions = {
    theme: 'mobiscroll',
    display: 'center',
    cssClass: 'select-hdn',
    showInput: false,
    showOnTap: false,
    input: '#' + this.field,
    focusOnClose: false,
    filter: true,
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
      const filterText = (this.searchInput.element.value || '').trim();
      if (filterText && filterText.length) {
        const el = event.target.querySelector<HTMLInputElement>('input.mbsc-sel-filter-input');
        if (el) {
          el.value = filterText;
          el.nextElementSibling.classList.add('mbsc-sel-filter-show-clear');
          setTimeout(() => this.searchSubject.next(filterText), 200);
          //const ev = new Event('input', { bubbles: true });
          //el.dispatchEvent(ev);
        }
      }
    },

    onBeforeClose: (event, inst) => {
      this.selectDataItems = [];
    },

    onFilter: (event, inst) => {
      const filterText = (event.filterText || '').trim();
      this.searchSubject.next(filterText);
      // Prevent built-in filtering
      return false;
    },

    onMarkupReady: (event: { target: HTMLElement }, inst: any) => {
      this.createProgressIcon(event.target);
    },

    onSet: (event, inst) => {
      const val = inst.getVal();
      this.processItemSelected(val);
    }
  };

  protected initComponent() {

    this.init(this.field);

    this.updateDisabledState();
    this.initEvents();

    // subscribe to notifier(user) search input
    this.searchSubject.asObservable().pipe(
      takeUntil(this.destroy$),
      debounceTime(150),
      //distinfieldanged(),
      switchMap((query: string) => {
        if (query && query.length > 1) {
          this.toggleProgress(true);
          return this.searchData(query);
        } else {
          return of(null);
        }
      })
    ).subscribe((data: any[]) => {
        this.toggleProgress(false);
        if (data && data.length) {
          this.selectDataItems = this.processSearchResults(data);
        } else {
          this.selectDataItems = [];
        }
    });
  }

  private getHeaderText() {
    const defaultText = this.ngxTranslateService.instant('Användar ID');
    return this.formControl ? this.formControl.label || defaultText : defaultText;
  }

  private initEvents() {
    if (this.formControl) {
      this.formControl.statusChanges.pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((e: any) => {
        if (this.searchInput.disabled !== this.isFormControlDisabled) {
          this.updateDisabledState();
        }
      });
    }
  } 

  private updateDisabledState() {
    this.searchInput.disabled = this.formControl ? this.isFormControlDisabled || this.disabled : this.disabled;
  }

  protected abstract searchData(query);

  protected abstract processSearchResults(data);

  protected abstract processItemSelected(val);

  private createProgressIcon(selectNode: HTMLElement) {
    const progressSpan = this.renderer.createElement('span');
    progressSpan.id = 'notifierProgress';
    progressSpan.className = 'notifierProgress mbsc-ic';
    progressSpan.innerHTML = '<img src="content/img/bars.gif" border="0" />'
    progressSpan.style.display = 'none';

    const filterNode = selectNode.querySelector<HTMLElement>('.mbsc-sel-filter-cont');
    this.progressIconEl = filterNode.appendChild(progressSpan);
  }

  protected toggleProgress(show) {
    if (this.progressIconEl) {
      this.progressIconEl.style.display = show ? '' : 'none';
    }
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }
}
