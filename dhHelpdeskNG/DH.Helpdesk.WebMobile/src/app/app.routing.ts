import { Routes, RouterModule } from '@angular/router';

import { HomeComponent, CaseEditComponent } from './components';
import { LoginComponent, PageNotFoundComponent } from './shared/components';
import { AuthGuard } from './helpers/guards';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component:  LoginComponent},

    { path: 'caseedit/:id', component:  CaseEditComponent, canActivate: [AuthGuard]},

    // otherwise redirect to home
    { path: '**', component: PageNotFoundComponent }
];

export const rootRouting = RouterModule.forRoot(appRoutes, 
    //{ enableTracing: true } // <-- debugging purposes only
);