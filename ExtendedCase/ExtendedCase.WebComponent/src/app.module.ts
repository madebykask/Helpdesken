﻿import { NgModule, ErrorHandler, Injector } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from './app/services/data/httpInterceptorService';

import { ExtendedHttpService } from './app/services/data/extended-http.service'
import { LogService } from './app/services/log.service'
import { UuidGenerator } from './app/utils/uuid-generator';
import { SubscriptionManager } from './app/shared/subscription-manager';
import { AppConfig, AppDiConfig } from './app/shared/app-config/app-config';
import { WindowWrapper } from './app/shared/window-wrapper'

import { ExtendedCaseTabComponent } from './app/components/ec-tab.component';
import { ExtendedCaseSectionComponent } from './app/components/ec-section.component';
import { ExtendedCaseReviewSectionComponent } from './app/components/ec-review-section.component';
import { ExtendedCaseReviewSectionInstanceComponent } from './app/components/ec-review-section-instance.component';
import { ValidationErrorComponent } from './app/components/validation/validation-errors.component';
import { ValidationWarningComponent } from './app/components/validation/validation-warnings.component';
import { ExtendedCaseFormsListComponent } from './app/components/forms-list.component';

import { AlertsFilter } from './app/pipes/alerts-filter';
import { SafeHtml } from './app/pipes/safeHtml-pipe';
import { SafeStyle } from './app/pipes/safeStyle-pipe';
import { Mask } from './app/directives/masks/mask.directive';
import { TrimValueAccessor } from './app/directives/input-trim.directive';
import { DatepickerModule, TabsModule, TypeaheadModule, ModalModule  } from 'ngx-bootstrap';
import { SelectModule } from 'ng-select';
import { ToNGSelectOptions } from './app/pipes/ng-select-options.pipe';
import { AlertComponent } from './app/components/shared/alert.component';
import { ProgressComponent } from './app/components/shared/progress.component';
import { AlertsService } from './app/services/alerts.service';
import { GlobalErrorHandler } from './app/shared/global-error-handler';
import { ErrorHandlingService } from './app/services/error-handling.service';
import { ClientLogApiService } from './app/services/data/client-log-api.service';
import { ClipboardModule } from 'ngx-clipboard';

import { FileUploadModule } from 'ng2-file-upload';
import { createCustomElement } from '@angular/elements';
import { DynamicModule } from 'ng-dynamic-component';

// import './styles/css/site.scss';
import { routes } from './routes';
import { RouterModule } from '@angular/router';
import { ExtendedCaseElementComponent } from './app/components/extended-case-element.component';
import { ExtendedCaseComponent } from './app/components/extended-case.component';
import { ExtendedCaseTextBoxComponent, ExtendedUnknowControlComponent, ExtendedCaseLabelComponent,
      ExtendedCaseTextBoxSearchComponent, ExtendedCaseTextAreaComponent, ExtendedCaseDropdownComponent,
      ExtendedCaseMultiselectComponent, ExtendedCaseDateComponent, ExtendedCaseCheckboxListComponent,
      ExtendedCaseCheckboxComponent, ExtendedCaseRadioComponent, ExtendedCaseReviewComponent, ExtendedCaseHtmlComponent,
      ExtendedCaseReviewComponentEx, ExtendedCaseFileUploadComponent } from './app/components/controls';

@NgModule({
    imports: [BrowserModule, BrowserAnimationsModule, HttpClientModule, FormsModule, ReactiveFormsModule,
        DatepickerModule.forRoot(),
        TypeaheadModule.forRoot(),
        TabsModule.forRoot(),
        ModalModule.forRoot(),
        RouterModule.forRoot(routes),
        SelectModule,
        ClipboardModule,
        FileUploadModule,
        DynamicModule.withComponents([ExtendedCaseTextBoxComponent, ExtendedUnknowControlComponent, ExtendedCaseLabelComponent,
            ExtendedCaseTextBoxSearchComponent, ExtendedCaseTextAreaComponent, ExtendedCaseDropdownComponent,
            ExtendedCaseMultiselectComponent, ExtendedCaseDateComponent, ExtendedCaseCheckboxListComponent,
            ExtendedCaseCheckboxComponent, ExtendedCaseRadioComponent, ExtendedCaseReviewComponent, ExtendedCaseHtmlComponent,
            ExtendedCaseFileUploadComponent])
        ],
    declarations: [ExtendedCaseElementComponent, ExtendedCaseComponent, ExtendedCaseFormsListComponent, ExtendedCaseTabComponent,
        ExtendedCaseSectionComponent, ExtendedCaseReviewSectionComponent,
        ExtendedCaseTextBoxComponent, ExtendedCaseLabelComponent, ExtendedCaseTextBoxSearchComponent,
        ExtendedCaseTextAreaComponent, ExtendedCaseCheckboxComponent, ExtendedCaseDateComponent,
        ExtendedCaseDropdownComponent, ExtendedCaseRadioComponent, AlertComponent, ProgressComponent,
        ExtendedCaseCheckboxListComponent, ExtendedCaseFileUploadComponent, ExtendedCaseMultiselectComponent, ExtendedCaseReviewComponent,
        ExtendedCaseReviewComponentEx, ExtendedCaseReviewSectionInstanceComponent, ExtendedCaseHtmlComponent, ValidationErrorComponent,
        ValidationWarningComponent, Mask, TrimValueAccessor, ToNGSelectOptions, AlertsFilter, SafeHtml, SafeStyle,
        ExtendedUnknowControlComponent],
        entryComponents: [ExtendedCaseComponent],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpInterceptorService,
            multi: true,
        },
        { provide: AppConfig, useValue: AppDiConfig },
        LogService,
        ExtendedHttpService,
        AlertsService,
        ErrorHandlingService,
        ClientLogApiService,
        { provide: ErrorHandler, useClass: GlobalErrorHandler },
        UuidGenerator,
        SubscriptionManager,
        WindowWrapper
    ],
})
export class AppModule {

  constructor(private injector: Injector) { }

  ngDoBootstrap() {
    // register extended case component as web element
    const custom = createCustomElement(ExtendedCaseComponent, { injector: this.injector});
    customElements.define('extended-case', custom);
  }

};
