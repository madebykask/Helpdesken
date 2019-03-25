import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslationApiService } from 'src/app/services/api/translation/translation-api.service';
import { Language } from 'src/app/models';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { Router } from '@angular/router';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';
import { take, finalize } from 'rxjs/operators';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent implements OnInit {

  constructor(private router: Router,
    private userSettingsService : UserSettingsApiService,
    private languagesService: LanguagesApiService ) { }

  @ViewChild('languageListView') languageListView: MbscListview;

  languages: OptionItem[];
  languageId: number;
  isLoadingLanguage: Boolean;


  languageSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false,
    select: 'single'
  };
  
  ngOnInit() {
    this.loadLanguages();
  }

  private selectLanguage(id: number) {
    this.setLanguage(id);
  }

  private loadLanguages() {
    this.languageId = this.userSettingsService.getCurrentLanguage() || 0;
    if (this.languageId === 0)
       return;
    
    this.isLoadingLanguage = true;
    this.languagesService.getLanguages().pipe(
        take(1),
        finalize(() => {
          this.isLoadingLanguage = false;
//          this.languageListView.select(this.languageId);
        })
    ).subscribe((data) => {
        this.languages = data;
    });
  }

  setLanguage(languageId: number) {
    if (languageId) {
      this.userSettingsService.setCurrentLanguage(languageId);

      // reload will reopen the app
      window.location.reload(true);
    }
  }
}
