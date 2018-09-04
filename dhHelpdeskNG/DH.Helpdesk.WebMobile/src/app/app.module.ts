import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { NgModule } from '@angular/core';

import { APP_INITIALIZER } from '@angular/core'
import { MainModule } from './main/main.module'
import { LoginComponent } from './shared/components'
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { TranslateModule, TranslateLoader, TranslateService as NgxTranslateService} from '@ngx-translate/core'
import { AppComponent } from './app.component';
import { HttpLoaderFactory, initTranslation, TranslationApiService } from './services/translation';
import { LocalStorageService } from './services/localStorage'
import { LoggerService } from './services/logging';
import { MomentModule } from 'ngx-moment';

import { AuthInterceptor, ErrorInterceptor } from './helpers/interceptors'

import { rootRouting } from './app.routing';
import { GetByKeyPipe } from './helpers/pipes/filterCaseOverview.pipe';

@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, PageNotFoundComponent, LoginComponent, GetByKeyPipe],
  imports: [ 
    MbscModule,
    BrowserModule,    
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    rootRouting,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [TranslationApiService]
      },
      useDefaultLang: true
    }),
    MomentModule,
    MainModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
	TranslationApiService,
    {
      provide: APP_INITIALIZER,
      useFactory: initTranslation,
      deps: [NgxTranslateService, TranslationApiService, LoggerService],
      multi:true
    },
    LocalStorageService    
  ]  
})
export class AppModule { }

