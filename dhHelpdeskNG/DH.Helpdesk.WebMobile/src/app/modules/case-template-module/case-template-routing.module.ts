import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CaseTemplateComponent } from './components/case-template/case-template.component';


const routes: Routes = [
  { path: '', component: CaseTemplateComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseTemplateRoutingModule { }
