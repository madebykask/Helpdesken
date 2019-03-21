import { NgModule } from '@angular/core';
import { SharedModule } from '../shared-module/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MbscModule } from '@mobiscroll/angular';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { CaseTemplateComponent } from './components/case-template/case-template.component';
import { CaseTemplateRoutingModule } from './case-template-routing.module';

@NgModule({
  declarations: [ CaseTemplateComponent ],
  imports: [
    CommonModule,
    MbscModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
    CaseTemplateRoutingModule
  ],
  entryComponents: [],
  exports: []
})
export class CaseTemplateModule 
{ 
}

