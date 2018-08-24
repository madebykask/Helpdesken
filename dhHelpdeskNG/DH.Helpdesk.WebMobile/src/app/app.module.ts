import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { NgModule } from '@angular/core';

import { APP_INITIALIZER } from '@angular/core'
//import { MainModule } from './main/main.module'
import { HomeComponent, LoginComponent } from './main/components'
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http'
import { TranslateModule, TranslateLoader, TranslateService as NgxTranslateService} from '@ngx-translate/core'
import { AppComponent } from './app.component';
import { HttpLoaderFactory, initTranslation, TranslationApiService } from './services/';

import { AuthInterceptor, ErrorInterceptor } from './helpers/interceptors'

import { rootRouting } from './app.routing';

@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, PageNotFoundComponent, HomeComponent, LoginComponent],
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
    //MainModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
	TranslationApiService,
    {
      provide: APP_INITIALIZER,
      useFactory: initTranslation,
      deps: [NgxTranslateService, TranslationApiService],
      multi:true
    }
  ]  
})
export class AppModule { }

