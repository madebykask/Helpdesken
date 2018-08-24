import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent, LoginComponent } from './components';
import { AuthGuard } from '../helpers/guards';

const routes: Routes = [
  //{ path: '',  component: HomeComponent, canActivate: [AuthGuard]},
  //{ path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { 
}
