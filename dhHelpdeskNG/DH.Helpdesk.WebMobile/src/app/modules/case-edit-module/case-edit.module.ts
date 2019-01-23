import { NgModule } from '@angular/core';
import { CaseEditRoutingModule } from './case-edit-routing.module';
import { CaseEditComponent } from './components/case-edit/case-edit.component';
import { CaseTextboxComponent } from './components/case-edit/controls/textbox/case-textbox-control.component';
import { CaseDateComponent } from './components/case-edit/controls/date/case-date-control.component';
import { CaseDropdownComponent } from './components/case-edit/controls/dropdown/case-dropdown-control.component';
import { CaseMultiDropdownComponent } from './components/case-edit/controls/dropdown/case-multi-dropdown-control.component';
import { CaseSwitchComponent } from './components/case-edit/controls/switch/case-switch-control.component';
import { CaseTextareaComponent } from './components/case-edit/controls/textarea/case-textarea-control.component';
import { CaseDateTimeComponent } from './components/case-edit/controls/date/case-datetime-control.component';
import { MailtoticketControlComponent } from './components/case-edit/controls/mailtoticket/mailtoticket-control.component';
import { CaseFilesUploadComponent } from './components/case-edit/controls/case-files-upload/case-files-upload.component';
import { FileUploadModule } from 'ng2-file-upload';
import { SharedModule } from '../shared-module/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MbscModule } from '@mobiscroll/angular';
import { HttpClientModule } from '@angular/common/http';
import { CaseFilesControlComponent } from './components/case-edit/controls/case-files/case-files-control.component';
import { CommonModule } from '@angular/common';
import { CaseActionsComponent } from './components/case-actions/case-actions.component';

@NgModule({
  declarations: [ CaseEditComponent,
    CaseTextboxComponent, CaseDateComponent, CaseDropdownComponent,  CaseMultiDropdownComponent,
    CaseSwitchComponent, CaseTextareaComponent, CaseDateTimeComponent, MailtoticketControlComponent,
    CaseFilesUploadComponent, CaseFilesControlComponent, CaseActionsComponent
],
  imports: [
    CommonModule,
    MbscModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
    FileUploadModule,
    CaseEditRoutingModule
  ],
  exports: [ ]
})
export class CaseEditModule { }
