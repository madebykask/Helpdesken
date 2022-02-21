import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule, ErrorHandler } from '@angular/core';
import { APP_INITIALIZER } from '@angular/core';
import { LoginComponent, HeaderTitleComponent } from './shared/components';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateModule, TranslateLoader, TranslateService as NgxTranslateService } from '@ngx-translate/core';
import { AppComponent } from './app.component';
import { LocalStorageService } from './services/local-storage';
import { LoggerService } from './services/logging';
import { AuthInterceptor, ErrorInterceptor } from './helpers/interceptors';
import { HomeComponent } from './components';
import { AppRoutingModule } from './app.routing';
import { FooterComponent } from './shared/components/footer/footer.component';
import { RequireAuthDirective } from './helpers/directives/require-auth.directive';
import { GlobalErrorHandler } from './helpers/errors/global-error-handler';
import { ErrorComponent } from './shared/components/error/error.component';
import { UserSettingsApiService } from './services/api/user/user-settings-api.service';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';
import { AltLayoutComponent } from './_layout/alt-layout/alt-layout.component';
import { SharedModule } from './modules/shared-module/shared.module';
import { TestComponent } from './components/test/test.component';
import { initApplication } from './logic/app-configuration/app-configuration';
import { HttpLoaderFactory } from './logic/translation';
import { TranslationApiService } from './services/api/translation/translation-api.service';
import { CaseTemplateComponent } from './components/case-template/case-template.component';
import { LanguageComponent } from './components/language/language/language.component';
import { RouteReuseStrategy } from '@angular/router';
import { CaseRouteReuseStrategy } from './helpers/case-route-resolver.stategy';
import { CasesStatusComponent } from './components/cases-status/cases-status.component';
import { VersionComponent } from './components/version.component';
import { MsalInterceptor, MsalModule } from '@azure/msal-angular';
import { config } from '@env/environment';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export const protectedResourceMap: [string, string[]][] = [
  ['https://graph.microsoft.com/v1.0/me', ['user.read']]
];

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent, AppLayoutComponent, PageNotFoundComponent,
    HeaderTitleComponent, FooterComponent,
    LoginComponent,
    HomeComponent,
    CaseTemplateComponent,
    RequireAuthDirective,
    ErrorComponent,
    AltLayoutComponent,
    TestComponent,
    LanguageComponent,
    CasesStatusComponent,
    VersionComponent
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
    SharedModule,
    //ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    MsalModule.forRoot({
      auth: {
        clientId: config.microsoftClientId,
        authority: config.microsoftAuthority + (config.microsoftAuthority.endsWith('/') ? '' : '/') + config.microsoftTenant,
        validateAuthority: true,
        redirectUri: config.microsoftRedirectUri,
        navigateToLoginRequestUrl: true,
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: isIE, // set to true for IE 11
      },
    }, {
      popUp: !isIE,
      consentScopes: [
        'user.read',
        'openid',
      ],
      unprotectedResources: ['https://www.microsoft.com/en-us/'],
      protectedResourceMap,
      extraQueryParameters: {}
    }
    )
  ],
  providers: [
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // { provide: LOCALE_ID, useValue: "sv-SE" },
    // { provide: LOCALE_ID, deps: [SettingsService], useFactory: (settingsService) => settingsService.getLanguage()},
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: MsalInterceptor,
    //   multi: true
    // },
    {
      provide: APP_INITIALIZER,
      useFactory: initApplication,
      deps: [NgxTranslateService, UserSettingsApiService, TranslationApiService, LocalStorageService, LoggerService],
      multi: true
    },
    {
      provide: RouteReuseStrategy,
      useClass: CaseRouteReuseStrategy
    }
  ],
  exports: [LanguageComponent]
})
export class AppModule { }

/* import { registerLocaleData } from '@angular/common';
import localeSv from '@angular/common/locales/sv';

// the second parameter 'fr' is optional
registerLocaleData(localeSv, ); */

