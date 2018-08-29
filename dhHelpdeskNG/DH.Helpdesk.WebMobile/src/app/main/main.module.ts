import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MbscModule } from '@mobiscroll/angular';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClient } from '@angular/common/http'

import { MainRoutingModule } from './main-routing.module';
import { HomeComponent, CasesOverviewComponent } from './components';

@NgModule({
  imports: [
    MbscModule,
    CommonModule,
    HttpClientModule,
    MainRoutingModule,
    FormsModule,
    BrowserModule,
    ReactiveFormsModule,
  ],
  exports: [],
  declarations: [HomeComponent, CasesOverviewComponent]
})
export class MainModule { }
