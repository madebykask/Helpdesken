import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { APP_INITIALIZER } from '@angular/core'
import { MainModule } from './main/main.module'
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import {HttpClientModule, HttpClient} from '@angular/common/http'
import {TranslateModule, TranslateLoader, TranslateService} from '@ngx-translate/core'
import { AppComponent } from './app.component';
import { HttpLoaderFactory, initTranslation } from './shared/translation/translateLoader';

let appRoutes : Routes = [  
  { path: '',  redirectTo: '/main', pathMatch: 'full'},
  { path: '**', component: PageNotFoundComponent }
];


@NgModule({
  bootstrap: [ AppComponent],
  declarations: [AppComponent, PageNotFoundComponent],
  imports: [
    BrowserModule,    
    HttpClientModule,
    FormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      useDefaultLang: true
    }),
    MainModule,
    RouterModule.forRoot(appRoutes),    
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initTranslation,
      deps: [TranslateService],
      multi:true
    }
  ]  
})
export class AppModule { }

