import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { NgModule, LOCALE_ID, ErrorHandler } from '@angular/core';

import { APP_INITIALIZER } from '@angular/core';
import { LoginComponent, HeaderTitleComponent } from './shared/components';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateModule, TranslateLoader, TranslateService as NgxTranslateService} from '@ngx-translate/core';
import { AppComponent } from './app.component';
import { HttpLoaderFactory, initTranslation, TranslationApiService } from './services/translation';
import { LocalStorageService } from './services/local-storage';
import { LoggerService } from './services/logging';

import { AuthInterceptor, ErrorInterceptor } from './helpers/interceptors';
import { HomeComponent, CasesOverviewComponent, CaseEditComponent, CaseTextboxComponent,
   CaseDateComponent, CaseDropdownComponent, CaseMultiDropdownComponent, CaseSwitchComponent,
   CaseTextareaComponent, CaseDateTimeComponent } from './components';
import { GetByKeyPipe, DateTimezonePipe, AlertsFilter } from './helpers/pipes';

import { AppRoutingModule } from './app.routing';
import { FooterComponent } from './shared/components/footer/footer.component';
import { RequireAuthDirective } from './helpers/directives/require-auth.directive';
import { CaseFilesControlComponent } from './components/case-edit/controls/case-files/case-files-control.component';
import { GlobalErrorHandler } from './helpers/errors/global-error-handler';
import { AlertComponent } from './shared/components/alert/alert.component';
import { ErrorComponent } from './shared/components/error/error.component';
import { UserSettingsService, initUserData } from './services/user';
import { AuthenticationService } from './services/authentication';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';

@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, AppLayoutComponent, PageNotFoundComponent, HeaderTitleComponent, FooterComponent,
     LoginComponent,
     HomeComponent, CasesOverviewComponent, CaseEditComponent,
     CaseTextboxComponent, CaseDateComponent, CaseDropdownComponent,  CaseMultiDropdownComponent,
     CaseSwitchComponent, CaseTextareaComponent, CaseDateTimeComponent,
     GetByKeyPipe, DateTimezonePipe, AlertsFilter,
     RequireAuthDirective,
     CaseFilesControlComponent,
     AlertComponent,
     ErrorComponent,
    ],
  imports: [
    MbscModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [TranslationApiService]
      },
      useDefaultLang: true
    }),
  ],
  providers: [
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // { provide: LOCALE_ID, useValue: "sv-SE" },
    // { provide: LOCALE_ID, deps: [SettingsService], useFactory: (settingsService) => settingsService.getLanguage()},
    TranslationApiService,      
    {
      provide: APP_INITIALIZER,
      useFactory: initTranslation,
      deps: [NgxTranslateService, TranslationApiService, LocalStorageService, LoggerService],
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initUserData,
      deps: [UserSettingsService, AuthenticationService, LoggerService],
      multi: true
    },   
    LocalStorageService
  ]
})
export class AppModule { }

/* import { registerLocaleData } from '@angular/common';
import localeSv from '@angular/common/locales/sv';

// the second parameter 'fr' is optional
registerLocaleData(localeSv, ); */
