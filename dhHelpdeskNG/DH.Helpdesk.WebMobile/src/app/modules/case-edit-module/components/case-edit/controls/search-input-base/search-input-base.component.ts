import { ViewChild, Input, Renderer2 } from '@angular/core';
import { MbscSelect, MbscSelectOptions } from '@mobiscroll/angular';
import { Subject, of } from 'rxjs';
import { takeUntil, debounceTime, switchMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { BaseControl } from '../base-control';
import { untilDestroyed } from 'ngx-take-until-destroy';

export abstract class SearchInputBaseComponent extends BaseControl<string> {

  constructor(
    protected ngxTranslateService: TranslateService,
    protected renderer: Renderer2) {
    super();
  }

  @ViewChild('searchInput', { static: false }) searchInput: any; // MbscInputBase | MbscTextArea | MbscInputBase
  @ViewChild('select', { static: false }) select: MbscSelect;

  @Input() disabled = false;
  @Input() protected customerId: number;

  selectDataItems: any = [];

  get fieldName(): string {
    return this.field;
  }

  protected query: string;
  filterText: string;

  private progressIconEl: any = null;
  protected searchSubject = new Subject<string>();
  protected selectDefaultOptions;

  get isMultiSelectMode() {
    return this.selectOptions.select === 'multiple';
  }

  selectOptions: MbscSelectOptions = {
    theme: 'mobiscroll',
    display: 'center',
    cssClass: 'single-select',
    showInput: false,
    showOnTap: false,
    input: '#' + this.field,
    focusOnClose: false,
    select: 'single',
    filter: true,
    buttons: ['cancel'],
    headerText: this.getHeaderText.bind(this),
    cancelText: this.ngxTranslateService.instant('Avbryt'),
    setText: this.ngxTranslateService.instant('Välj'),
    filterPlaceholderText: this.ngxTranslateService.instant('Skriv för att filtrera'),
    filterEmptyText: this.ngxTranslateService.instant('Inget resultat'),

    /// Select Events: https://docs.mobiscroll.com/angular/select#events
    onInit: (event, inst) => {
      if (this.formControl && this.formControl.fieldInfo.isReadonly) {
        inst.disable();
      }
    },
    onMarkupReady: (event: { target: HTMLElement }, inst: any) => {
      this.createProgressIcon(event.target);
    },
    onBeforeShow: function (event, inst) {
    },
    onPosition: function (event, inst) {
    },
    onShow: (event, inst) => {
      //setting filter text from user input on case page
      const filterText = (this.filterText  || '').trim();
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
    onSet: (event, inst) => {
      //event.valueText: The selected value as text (if any)
      const val = inst.getVal();
      if (!this.isMultiSelectMode) {
        this.processItemSelected(val, true);
        inst.clear(); // clear selections if any
      }
    },
    onItemTap: (event, inst) => {
      //console.log(`>>> emailsSearchSelect: onItemTap: ${event.value}`);
      if (this.isMultiSelectMode) {
        this.processItemSelected(event.value, !event.selected);
      }
    },
    onDestroy: function (event, inst) {
    },
    onClose: function (event, inst) {
    },
    onCancel: function (event, inst) {
      inst.clear(); // clear selections if any
    },
    onBeforeClose: (event, inst) => {
      this.selectDataItems = [];
    },
    onClear: function (event, inst) {
    },
    onFilter: (event, inst) => {
      const filterText = (event.filterText || '').trim();
      this.searchSubject.next(filterText);
      // Prevent built-in filtering
      return false;
    },
    onChange: function (event, inst) {
      // Your custom event handler goes here
      //console.log(`>>> emailsSearchSelect: ${event.valueText}`);
    }
    /// End of Select Events 
  };

  protected initComponent() {

    this.init(this.field);

    setTimeout(() => this.updateDisabledState(), 100);

    this.initEvents();

    // subscribe to notifier(user) search input
    this.searchSubject.asObservable().pipe(
      debounceTime(150),
      switchMap((query: string) => {
        if (query && query.length > 1) {
          this.toggleProgress(true);
          this.query = query;
          return this.searchData(query);
        } else {
          return of(null);
        }
      }),
      untilDestroyed(this)
    ).subscribe((data: any[]) => {
      this.toggleProgress(false);
      if (data && data.length) {
        this.selectDataItems = this.processSearchResults(data, this.query);
      } else {
        this.selectDataItems = [];
      }
    });
  }

  protected setFilterText(val: string) {
    this.filterText = val;
  }

  protected highligtQueryText(text: string, query: string) {
    if (text && text.length) {
      text = text.replace(RegExp('(' + query + ')', 'ig'), '<b>$1</b>');
    }
    return text;
  }

  private getHeaderText() {
    const defaultText = this.ngxTranslateService.instant('Användar ID');
    return this.formControl ? this.formControl.label || defaultText : defaultText;
  }

  private initEvents() {
    if (this.formControl) {
      this.formControl.statusChanges.pipe(
        untilDestroyed(this)
      )
        .subscribe((e: any) => {
          if (this.searchInput.disabled !== this.isFormControlDisabled) {
            this.updateDisabledState();
          }
        });
    }
  }

  showSelectPopup(searchQuery) {
    this.filterText = searchQuery;
    this.select.instance.show();
  }

  private updateDisabledState() {
    this.searchInput.disabled = this.formControl ? this.isFormControlDisabled || this.disabled : this.disabled;
  }

  protected abstract searchData(query);

  protected abstract processSearchResults(data, query);

  protected abstract processItemSelected(val, isSelected);

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
  }
}
