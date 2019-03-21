import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './components';
import { LoginComponent, PageNotFoundComponent } from './shared/components';
import { AuthGuard } from './helpers/guards';
import { NgModule } from '@angular/core';
import { ErrorComponent } from './shared/components/error/error.component';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';
import { TestComponent } from './components/test/test.component';

const appRoutes: Routes = [
  { 
    path: '',
    component: AppLayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard]},
      { path: 'casesoverview', 
        loadChildren: './modules/case-overview-module/case-overview.module#CaseOverviewModule',
        canActivate: [AuthGuard] },
      { path: 'case',
        loadChildren: './modules/case-edit-module/case-edit.module#CaseEditModule',
        canActivate: [AuthGuard] 
      },
      { 
        path: 'createcase',
        loadChildren: './modules/case-template-module/case-template.module#CaseTemplateModule',
        canActivate: [AuthGuard]
      }
    ]
  },

  { path: 'test', component: TestComponent },

/*   { 
    path: '', 
    component: AltLayoutComponent,
    children: [
    ]
  },
 */
  //no layout routes  
  { path: 'login', component: LoginComponent},
  { path: 'error', component: ErrorComponent },
  { path: '',   redirectTo: '/', pathMatch: 'full' },
  
  // otherwise redirect to PageNotFoundComponent
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
      RouterModule.forRoot(
        appRoutes,
        // { enableTracing: true } // <-- debugging purposes only
        { preloadingStrategy: PreloadAllModules }
      )
    ],
    exports: [
      RouterModule
    ]
  })
  export class AppRoutingModule {}
