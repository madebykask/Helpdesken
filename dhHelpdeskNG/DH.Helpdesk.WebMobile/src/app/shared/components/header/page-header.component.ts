import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../../../../environments/environment'
import { map, takeUntil, take, finalize, filter, switchMap } from 'rxjs/operators';
import { MbscSelectOptions, MbscSelect } from '@mobiscroll/angular';
import { UserSettingsService } from '../../../services/user';
import { LanguagesService } from '../../../services/language/languages.service';
import { Subject, interval } from 'rxjs';
import { OptionItem } from '../../../models';


@Component({
  selector: 'app-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject();
  @ViewChild('languages') languagesCtrl: MbscSelect;

  languageId: number;
  isLoadingLanguage: boolean = true;

  hamburgerSettings: any = {
    type: 'hamburger'
  };

  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'top',
    data: [],
    onSet: (event, inst) => {
      const item = (<OptionItem[]>inst.settings.data).find(item => item.text == event.valueText);
      this.setLanguage(item ? +item.value : null);
    }
  }

  constructor(private _userSettingsService : UserSettingsService, private _languagesService: LanguagesService) { }

  ngOnInit() {
      let until$ = new Subject();
      let timer = interval(500);
      timer.pipe(
        filter(() => this._userSettingsService.isLoadingUserSettings == false),
        map(() => {
            this.languageId = this._userSettingsService.getCurrentLanguage();
            this.getLanguages();        
            until$.next();
        }),
        takeUntil(until$)
      ).subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
  }

  openLanguages() {
    if (!this.isLoadingLanguage) {
      this.languagesCtrl.instance.show();
    }
  }

  getLanguages() {
    this.isLoadingLanguage = true;
    this._languagesService.getLanguages()
      .pipe(
        take(1),
        takeUntil(this._destroy$)
      )
      .subscribe((data) => {
        this.languagesCtrl.instance.refresh(data); 
        this.isLoadingLanguage = false;
      })
  }

  private setLanguage(languageId: number) {
    if (languageId == null) return;

    this._userSettingsService.setCurrentLanguage(languageId);
    window.location.reload(true);    
  }
}
