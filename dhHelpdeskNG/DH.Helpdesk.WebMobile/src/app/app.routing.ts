import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent, CasesOverviewComponent } from './components';
import { LoginComponent, PageNotFoundComponent } from './shared/components';
import { AuthGuard } from './helpers/guards';
import { NgModule } from '@angular/core';
import { ErrorComponent } from './shared/components/error/error.component';
import { AppLayoutComponent } from './_layout/app-layout/app-layout.component';
import { TestComponent } from './components/test/test.component';
import { CaseFileDataResolver } from './modules/case-edit-module/resolvers/case-file-data.resolver';
import { LogFileDataResolver } from './modules/case-edit-module/resolvers/log-file-data.resolver';
import { FilePreviewComponent } from './shared/components/file-preview/file-preview.component';

const appRoutes: Routes = [
  { 
    path: '',
    component: AppLayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard]},
      { path: 'casesoverview', component: CasesOverviewComponent, canActivate: [AuthGuard] },
      { path: 'test', component: TestComponent },      
      { path: 'casesoverview/:searchType', component: CasesOverviewComponent, canActivate: [AuthGuard] },
      { path: 'case',
        loadChildren: './modules/case-edit-module/case-edit.module#CaseEditModule',
        canActivate: [AuthGuard] 
      },
      
      //case file
      { 
        path: 'case/:caseId/file/:fileId',
        component: FilePreviewComponent, 
        resolve: {
          fileData: CaseFileDataResolver
        }
      },

      // log file preview
      { 
        path: 'case/:caseId/logfile/:fileId', 
        component: FilePreviewComponent, 
        resolve: {
          fileData: LogFileDataResolver
        }
      }
    ]
  },

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
