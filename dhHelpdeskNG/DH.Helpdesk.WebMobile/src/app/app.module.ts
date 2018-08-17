import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { MainModule } from './main/main.module'
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

import {HttpClientModule, HttpClient} from '@angular/common/http'
import {TranslateModule, TranslateLoader} from '@ngx-translate/core'
import { AppComponent } from './app.component';
import { HttpLoaderFactory } from './shared/translation/translateLoader';

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
      }
    }),
    MainModule,
    RouterModule.forRoot(appRoutes),    
  ],
  providers: []  
})
export class AppModule { }

