import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CaseEditComponent } from './components/case-edit/case-edit.component';

const routes: Routes = [
  {
    path: ':id',
    component: CaseEditComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseEditRoutingModule { }
