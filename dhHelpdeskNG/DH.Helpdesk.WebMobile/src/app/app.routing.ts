import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './components';
import { LoginComponent, PageNotFoundComponent } from './shared/components';
import { AuthGuard } from './helpers/guards';
import { NgModule } from '@angular/core';
import { ErrorComponent } from './shared/components/error/error.component';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';
import { TestComponent } from './components/test/test.component';
import { CaseTemplateComponent } from './components/case-template/case-template.component';
import { LanguageComponent } from './components/language/language/language.component';


const appRoutes: Routes = [{
    path: '',
    component: AppLayoutComponent,
    children: [{
        path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard]
      }, {
        path: 'casesoverview',
        loadChildren: () => import('./modules/case-overview-module/case-overview.module').then(m => m.CaseOverviewModule),
        canActivate: [AuthGuard]
      }, {
        path: 'case',
        loadChildren: () => import('./modules/case-edit-module/case-edit.module').then(m => m.CaseEditModule),
        canActivate: [AuthGuard]
      }, {
        path: 'createcase',
        component: CaseTemplateComponent,
        canActivate: [AuthGuard]
      }, {
        path: 'language',
        component: LanguageComponent,
        canActivate: [AuthGuard]
      }
    ]
  },

  { path: 'test', component: TestComponent },

/*{
    path: '',
    component: AltLayoutComponent,
    children: [
    ]
  },
 */
  //no layout routes
  { path: 'login', component: LoginComponent },
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
        { useHash: false, preloadingStrategy: PreloadAllModules }
      )
    ],
    exports: [
      RouterModule
    ]
  })
  export class AppRoutingModule {}
