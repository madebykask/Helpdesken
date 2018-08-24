import { Routes, RouterModule } from '@angular/router';

import { HomeComponent, LoginComponent } from './main/components';
import { AuthGuard } from './helpers/guards';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const rootRouting = RouterModule.forRoot(appRoutes, 
    { enableTracing: true } // <-- debugging purposes only
);