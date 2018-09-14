import { MbscModule } from '@mobiscroll/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { NgModule } from '@angular/core';

import { APP_INITIALIZER } from '@angular/core'
import { LoginComponent, HeaderComponent } from './shared/components'
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { TranslateModule, TranslateLoader, TranslateService as NgxTranslateService} from '@ngx-translate/core'
import { AppComponent } from './app.component';
import { HttpLoaderFactory, initTranslation, TranslationApiService } from './services/translation';
import { LocalStorageService } from './services/localStorage'
import { LoggerService } from './services/logging';
import { MomentModule } from 'ngx-moment';

import { AuthInterceptor, ErrorInterceptor } from './helpers/interceptors'
import { HomeComponent, CasesOverviewComponent, CaseEditComponent } from './components';
import { GetByKeyPipe } from './helpers/pipes';

import { rootRouting } from './app.routing';
import { FooterComponent } from './shared/components/footer/footer.component';
import { RequireAuthDirective } from './helpers/directives/require-auth.directive';

@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, PageNotFoundComponent, HeaderComponent, FooterComponent,
     LoginComponent,
     HomeComponent, CasesOverviewComponent, CaseEditComponent,
     GetByKeyPipe,
     RequireAuthDirective],
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

