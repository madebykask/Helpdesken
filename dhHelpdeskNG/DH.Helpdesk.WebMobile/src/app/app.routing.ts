import { Routes, RouterModule } from '@angular/router';

import { HomeComponent, CaseEditComponent, CasesOverviewComponent } from './components';
import { LoginComponent, PageNotFoundComponent } from './shared/components';
import { AuthGuard } from './helpers/guards';
import { NgModule } from '@angular/core';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component:  LoginComponent},
    { path: 'casesoverview', component:  CasesOverviewComponent, canActivate: [AuthGuard]},

    { path: 'case/:id', component:  CaseEditComponent, canActivate: [AuthGuard]},

    { path: '',   redirectTo: '/', pathMatch: 'full' },
    // otherwise redirect to home
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
      RouterModule.forRoot(
        appRoutes,
        // { enableTracing: true } // <-- debugging purposes only
      )
    ],
    exports: [
      RouterModule
    ]
  })
  export class AppRoutingModule {}
