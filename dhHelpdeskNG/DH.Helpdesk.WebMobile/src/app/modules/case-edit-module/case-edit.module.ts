import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CaseEditRoutingModule } from './case-edit-routing.module';
import { CaseEditComponent } from './components/case-edit/case-edit.component';
import { MbscFormGroupExpandDirective } from './components/case-edit/directives/form-group-expand.directive';
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
import { CaseActionsComponent } from './components/case-actions/case-actions.component';
import { GeneralActionComponent } from './components/case-actions/actions/general-action/general-action.component';
import { FieldChangeActionComponent } from './components/case-actions/actions/field-change-action/field-change-action.component';
import { LogNoteActionComponent } from './components/case-actions/actions/log-note-action/log-note-action.component';
import { CaseActionContainerComponent } from './components/case-actions/case-action-container.component';
import { CaseActionHostDirective } from './components/case-actions/actions/case-action-host.directive';
import { ActionsFilterPipe } from './pipes/actions-filter.pipe';
import { LogFilesUploadComponent } from './components/case-edit/controls/log-files-upload/log-files-upload.component';
import { CaseLogInputComponent } from './components/case-edit/case-log-input/case-log-input.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { FilePreviewComponent } from './components/file-preview/file-preview.component';
import { CommonFileViewerComponent } from './components/file-preview/viewers/common-file-viewer.component';
import { PdfFileViewerComponent } from './components/file-preview/viewers/pdf-file-viewer.component';
import { ImageFileViewerComponent } from './components/file-preview/viewers/image-file-viewer.component';
import { TextFileViewerComponent } from './components/file-preview/viewers/text-file-viewer.component';
import { Pdf2FileViewerComponent } from './components/file-preview/viewers/pdf2-file-viewer.component';
import { Pdf3FileViewerComponent } from './components/file-preview/viewers/pdf3-file-viewer.component';
import { NotifierSearchComponent } from './components/case-edit/controls/notifier-search/notifier-search.component';
import { FilterExtDirective } from './directives/filter-ext.directive';
import { LognoteEmailInputComponent } from './components/case-edit/controls/lognote-email-input/lognote-email-input.component';
import { CaseMenuComponent } from './components/case-menu/case-menu.component';
import { PinchZoomModule } from 'ngx-pinch-zoom';

@NgModule({
  declarations: [ CaseEditComponent,
    CaseTextboxComponent, CaseDateComponent, CaseDropdownComponent,  CaseMultiDropdownComponent,
    CaseSwitchComponent, CaseTextareaComponent, CaseDateTimeComponent, MailtoticketControlComponent,
    CaseFilesUploadComponent, CaseFilesControlComponent, CaseActionsComponent, CaseActionHostDirective,
    CaseActionContainerComponent, GeneralActionComponent, FieldChangeActionComponent, LogNoteActionComponent, ActionsFilterPipe,
    CaseLogInputComponent, LogFilesUploadComponent, FilePreviewComponent, PdfFileViewerComponent, CommonFileViewerComponent, ImageFileViewerComponent,
    TextFileViewerComponent, Pdf2FileViewerComponent, Pdf3FileViewerComponent, NotifierSearchComponent, MbscFormGroupExpandDirective,
    FilterExtDirective, LognoteEmailInputComponent, CaseMenuComponent
],
  imports: [
    MbscModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
    FileUploadModule, // todo: check if required?
    PdfViewerModule, // todo: replace with different approach?
    CaseEditRoutingModule,
    PinchZoomModule
  ],
  entryComponents: [FieldChangeActionComponent, LogNoteActionComponent, GeneralActionComponent],
  exports: [],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class CaseEditModule { }
