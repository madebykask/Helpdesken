import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CasesOverviewComponent } from './components/cases-overview/cases-overview.component';


const routes: Routes = [
  { path: '', component: CasesOverviewComponent },
  { path: ':searchType', component: CasesOverviewComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseOverviewRoutingModule { }
