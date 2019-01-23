import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { NgModule, ErrorHandler } from '@angular/core';
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
import { HomeComponent, CasesOverviewComponent } from './components';
import { AppRoutingModule } from './app.routing';
import { FooterComponent } from './shared/components/footer/footer.component';
import { RequireAuthDirective } from './helpers/directives/require-auth.directive';
import { GlobalErrorHandler } from './helpers/errors/global-error-handler';
import { ErrorComponent } from './shared/components/error/error.component';
import { initUserData } from './services/user';
import { UserSettingsApiService } from './services/api/user/user-settings-api.service';
import { AuthenticationService } from './services/authentication';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';
import { AltLayoutComponent } from './_layout/alt-layout/alt-layout.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '@env/environment';
import { SharedModule } from './modules/shared-module/shared.module';
import { GetByKeyPipe } from './helpers/pipes/filter-case-overview.pipe';
import { TestComponent } from './components/test/test.component';

@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, AppLayoutComponent, PageNotFoundComponent,
     HeaderTitleComponent, FooterComponent,
     LoginComponent,
     HomeComponent, CasesOverviewComponent,
     GetByKeyPipe,
     RequireAuthDirective,
     ErrorComponent,
     AltLayoutComponent,
     TestComponent,
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
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
  ],
  providers: [
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // { provide: LOCALE_ID, useValue: "sv-SE" },
    // { provide: LOCALE_ID, deps: [SettingsService], useFactory: (settingsService) => settingsService.getLanguage()},
    {
      provide: APP_INITIALIZER,
      useFactory: initTranslation,
      deps: [NgxTranslateService, TranslationApiService, LocalStorageService, LoggerService],
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initUserData,
      deps: [UserSettingsApiService, AuthenticationService, LoggerService],
      multi: true
    }
  ]
})
export class AppModule { }

/* import { registerLocaleData } from '@angular/common';
import localeSv from '@angular/common/locales/sv';

// the second parameter 'fr' is optional
registerLocaleData(localeSv, ); */

