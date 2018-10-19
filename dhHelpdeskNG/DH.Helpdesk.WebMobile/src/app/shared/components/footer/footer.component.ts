import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, interval } from 'rxjs';
import { MbscSelect, MbscSelectOptions, MbscNavOptions } from '@mobiscroll/angular';
import { take, takeUntil, filter, map } from 'rxjs/operators';
import { UserSettingsService } from 'src/app/services/user';
import { OptionItem } from 'src/app/models';
import { LanguagesService } from 'src/app/services/language/languages.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject();
  @ViewChild('languages') languagesCtrl: MbscSelect;

  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'bottom',
    data: [],
    onSet: (event, inst) => {
      const item = (<OptionItem[]>inst.settings.data).find(item => item.text == event.valueText);
      this.setLanguage(item ? +item.value : null);
    }

  }

  bottomMenuSettings: MbscNavOptions = {
    //layout: 'fixed',
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null
  }

  languageId: number;
  isLoadingLanguage: boolean = true;
  constructor(private _router: Router, private _userSettingsService : UserSettingsService, private _languagesService: LanguagesService) { }

  ngOnInit() {
    let until$ = new Subject();
    let timer = interval(500);
    timer.pipe( //TODO: this is hack to wait untill usersettings are loaded. otherwise cid ang langid is undefined. Refactor
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

  goTo(url: string = null) {
    if(url == null) return;
    this._router.navigate([url]);
  }

  private setLanguage(languageId: number) {
    if (languageId == null) return;

    this._userSettingsService.setCurrentLanguage(languageId);
    window.location.reload(true);    
  }
}


