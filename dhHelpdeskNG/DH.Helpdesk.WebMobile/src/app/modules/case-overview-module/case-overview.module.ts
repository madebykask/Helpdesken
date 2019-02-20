import { NgModule } from '@angular/core';
import { SharedModule } from '../shared-module/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MbscModule } from '@mobiscroll/angular';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { CaseOverviewRoutingModule } from './case-overview-routing.module';
import { GetByKeyPipe } from './pipes/filter-case-overview.pipe';
import { CasesOverviewComponent } from './components/cases-overview/cases-overview.component';

@NgModule({
  declarations: [ CasesOverviewComponent, GetByKeyPipe ],
  imports: [
    CommonModule,
    MbscModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
    CaseOverviewRoutingModule
  ],
  entryComponents: [],
  exports: []
})
export class CaseOverviewModule { }
