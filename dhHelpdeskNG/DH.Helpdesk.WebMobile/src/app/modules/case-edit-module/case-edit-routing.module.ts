import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CaseEditComponent } from './components/case-edit/case-edit.component';
import { FilePreviewComponent } from './components/file-preview/file-preview.component';
import { LogFileDataResolver } from './resolvers/log-file-data.resolver';
import { CaseFileDataResolver } from './resolvers/case-file-data.resolver';

const routes: Routes = [
  //existing case file
  { 
    path: ':caseId/file/:fileId',
    component: FilePreviewComponent, 
    resolve: {
      fileData: CaseFileDataResolver
    }
  },

  //temp case file with no Id
  { 
    path: ':caseKey/file',
    component: FilePreviewComponent, 
    resolve: {
      fileData: CaseFileDataResolver
    }
  },

  // log file preview
  { 
    path: ':caseId/logfile/:fileId', 
    component: FilePreviewComponent, 
    resolve: {
      fileData: LogFileDataResolver
    }
  },
   
  {
    path: 'template/:templateId',
    component: CaseEditComponent
  },

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
