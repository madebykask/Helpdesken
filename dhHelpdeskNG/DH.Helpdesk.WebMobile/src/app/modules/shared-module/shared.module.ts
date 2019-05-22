import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimeFormatPipe, DateFormatPipe } from './pipes';
import { TranslateModule } from '@ngx-translate/core';
import { TruncatePipe } from './pipes/truncate.pipe';
import { TruncateTextDirective } from './directives/truncate-text.directive';
import { NewlinePipe } from './pipes/newline.pipe';
import { SanitizePipe } from './pipes/sanitize.pipe';
import { CollapsibleDirective } from './directives/collapsible.directive';

@NgModule({
  declarations: [ DateTimeFormatPipe, DateFormatPipe, AlertsFilterPipe, TruncatePipe, TruncateTextDirective,
    CollapsibleDirective, NewlinePipe, SanitizePipe ],
  imports: [
    CommonModule,
    TranslateModule.forChild()
  ],
  exports: [ DateTimeFormatPipe, DateFormatPipe, AlertsFilterPipe, TranslateModule, TruncatePipe,
    CollapsibleDirective, TruncateTextDirective, NewlinePipe, SanitizePipe]
})
export class SharedModule { }
