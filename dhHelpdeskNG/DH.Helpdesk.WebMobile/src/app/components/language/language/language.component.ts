import { Component, OnInit } from '@angular/core';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { Router } from '@angular/router';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';
import { take, finalize, map } from 'rxjs/operators';
import { MbscListviewOptions } from '@mobiscroll/angular';
import { WindowWrapper } from 'src/app/modules/shared-module/helpers/window-wrapper';
import { OptionItem } from 'src/app/modules/shared-module/models';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent implements OnInit {

  constructor(private router: Router,
    private windowWrapper: WindowWrapper,
    private userSettingsService: UserSettingsApiService,
    private languagesService: LanguagesApiService) { }

  languages: any[];
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

  private loadLanguages() {
    this.languageId = this.userSettingsService.getCurrentLanguage() || 0;
    if (this.languageId === 0) {
       return;
    }

    this.isLoadingLanguage = true;
    this.languagesService.getLanguages().pipe(
        take(1),
        map((o: OptionItem[]) => o.map(p => ({ id: parseInt(p.value, 10), text: p.text, selected: this.languageId === parseInt(p.value, 10) }))),
        finalize(() => {
          this.isLoadingLanguage = false;
        })
    ).subscribe((data) => {
        this.languages = data;
    });
  }

  setLanguage(languageId: number) {
    if (languageId) {
      this.userSettingsService.setCurrentLanguage(languageId);
      this.router.navigateByUrl('/');
      setTimeout(() => this.windowWrapper.nativeWindow.location.reload(true), 300);
    }
  }
}
