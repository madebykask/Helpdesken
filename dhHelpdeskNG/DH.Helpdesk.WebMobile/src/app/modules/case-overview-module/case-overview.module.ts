import { NgModule } from '@angular/core';
import { SharedModule } from '../shared-module/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MbscModule } from '@mobiscroll/angular';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { CaseOverviewRoutingModule } from './case-overview-routing.module';
import { GetByKeyPipe, CaseHasValuePipe } from './pipes/filter-case-overview.pipe';
import { CasesOverviewComponent } from './components/cases-overview/cases-overview.component';
import { CasesFilterComponent } from './components/cases-filter/cases-filter.component';
import { CasesSortMenuComponent } from './components/cases-sort-menu/cases-sort-menu.component';

@NgModule({
  declarations: [ CasesOverviewComponent, GetByKeyPipe, CasesFilterComponent, CasesSortMenuComponent, CaseHasValuePipe ],
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
