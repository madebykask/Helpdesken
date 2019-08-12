import { NgModule, ErrorHandler, Injector } from '@angular/core';
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

import { ExtendedCaseComponent } from './app/components/extended-case.component';
import { ExtendedCaseTabComponent } from './app/components/ec-tab.component';
import { ExtendedCaseSectionComponent } from './app/components/ec-section.component';
import { ExtendedCaseReviewSectionComponent } from './app/components/ec-review-section.component';
import { ExtendedCaseTextBoxComponent } from './app/components/controls/ec-textbox.component';
import { ExtendedCaseLabelComponent } from './app/components/controls/ec-label.component';
import { ExtendedCaseTextBoxSearchComponent } from './app/components/controls/ec-textbox-search.component';
import { ExtendedCaseTextAreaComponent } from './app/components/controls/ec-textarea.component';
import { ExtendedCaseCheckboxComponent } from './app/components/controls/ec-checkbox.component';
import { ExtendedCaseDateComponent } from './app/components/controls/ec-date.component';
import { ExtendedCaseDropdownComponent } from './app/components/controls/ec-dropdown.component';
import { ExtendedCaseRadioComponent } from './app/components/controls/ec-radio.component';
import { ExtendedCaseCheckboxListComponent } from './app/components/controls/ec-checkbox-list.component';
import { ExtendedCaseMultiselectComponent } from './app/components/controls/ec-multiselect.component';
import { ExtendedCaseReviewComponent } from './app/components/controls/ec-review.component';
import { ExtendedCaseReviewComponentEx } from './app/components/controls/ec-review-ex.component';
import { ExtendedCaseReviewSectionInstanceComponent } from './app/components/ec-review-section-instance.component';
import { ExtendedCaseHtmlComponent } from './app/components/controls/ec-html.component';
import { ValidationErrorComponent } from './app/components/validation/validation-errors.component';
import { ValidationWarningComponent } from './app/components/validation/validation-warnings.component';
import { ExtendedUnknowControlComponent } from './app/components/controls/ec-unknown.component';
import { ExtendedCaseFormsListComponent } from './app/components/forms-list.component';

import { AlertsFilter } from './app/pipes/alerts-filter';
import { SafeHtml } from './app/pipes/safeHtml-pipe';
import { SafeStyle } from './app/pipes/safeStyle-pipe';
import { Mask } from './app/directives/masks/mask.directive';
import { TrimValueAccessor } from './app/directives/input-trim.directive';
import { DatepickerModule, TabsModule, TypeaheadModule, ModalModule  } from 'ngx-bootstrap';
//import { SelectModule } from './modules/ng-select/ng-select';
import {SelectModule} from 'ng-select';
import { ToNGSelectOptions } from './app/pipes/ng-select-options.pipe';
import { AlertComponent } from './app/components/shared/alert.component';
import { ProgressComponent } from './app/components/shared/progress.component';
import { AlertsService } from './app/services/alerts.service';
import { GlobalErrorHandler } from './app/shared/global-error-handler';
import { ErrorHandlingService } from './app/services/error-handling.service';
import { ClientLogApiService } from './app/services/data/client-log-api.service';
import { ClipboardModule } from 'ngx-clipboard';

import { createCustomElement } from '@angular/elements';
import { DynamicModule } from 'ng-dynamic-component';

//import './styles/css/site.scss';
import { routes } from './routes';
import { RouterModule } from '@angular/router';
import { ExtendedCaseElementComponent } from './app/components/extended-case-element.component';


@NgModule({
    imports: [BrowserModule, BrowserAnimationsModule, HttpClientModule, FormsModule, ReactiveFormsModule,
        DatepickerModule.forRoot(),
        TypeaheadModule.forRoot(),
        TabsModule.forRoot(),
        ModalModule.forRoot(),
        RouterModule.forRoot(routes),
        SelectModule,
        ClipboardModule,
        DynamicModule.withComponents([ExtendedCaseTextBoxComponent, ExtendedUnknowControlComponent, ExtendedCaseLabelComponent,
            ExtendedCaseTextBoxSearchComponent, ExtendedCaseTextAreaComponent, ExtendedCaseDropdownComponent,
            ExtendedCaseMultiselectComponent, ExtendedCaseDateComponent, ExtendedCaseCheckboxListComponent,
            ExtendedCaseCheckboxComponent, ExtendedCaseRadioComponent, ExtendedCaseReviewComponent, ExtendedCaseHtmlComponent])
        ],
    declarations: [ExtendedCaseElementComponent, ExtendedCaseComponent, ExtendedCaseFormsListComponent, ExtendedCaseTabComponent,
        ExtendedCaseSectionComponent, ExtendedCaseReviewSectionComponent,
        ExtendedCaseTextBoxComponent, ExtendedCaseLabelComponent, ExtendedCaseTextBoxSearchComponent,
        ExtendedCaseTextAreaComponent, ExtendedCaseCheckboxComponent, ExtendedCaseDateComponent,
        ExtendedCaseDropdownComponent, ExtendedCaseRadioComponent, AlertComponent, ProgressComponent,
        ExtendedCaseCheckboxListComponent, ExtendedCaseMultiselectComponent, ExtendedCaseReviewComponent,
        ExtendedCaseReviewComponentEx, ExtendedCaseReviewSectionInstanceComponent, ExtendedCaseHtmlComponent, ValidationErrorComponent,
        ValidationWarningComponent, Mask, TrimValueAccessor, ToNGSelectOptions, AlertsFilter, SafeHtml, SafeStyle,
        ExtendedUnknowControlComponent],
        entryComponents: [ExtendedCaseElementComponent, ExtendedCaseComponent],
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
    ]
})
export class AppModule {

  constructor(private injector: Injector) { }

  ngDoBootstrap() {
    // register extended case component as web element
    const custom = createCustomElement(ExtendedCaseElementComponent, { injector: this.injector});
    customElements.define('extended-case-element', custom);
  }

};
