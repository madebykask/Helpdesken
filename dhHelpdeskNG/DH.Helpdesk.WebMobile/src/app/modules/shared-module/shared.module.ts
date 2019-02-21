import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimeFormatPipe, DateFormatPipe } from './pipes';
import { TranslateModule } from '@ngx-translate/core';
import { TruncatePipe } from './pipes/truncate.pipe';
import { TruncateTextDirective } from './directives/truncate-text.directive';
import { NewlinePipe } from './pipes/newline.pipe';
import { SanitizePipe } from './pipes/sanitize.pipe';

@NgModule({
  declarations: [ DateTimeFormatPipe, DateFormatPipe, AlertsFilterPipe, TruncatePipe, TruncateTextDirective, NewlinePipe, SanitizePipe ],
  imports: [
    CommonModule,
    TranslateModule.forChild()
  ],
  exports: [ DateTimeFormatPipe, DateFormatPipe, AlertsFilterPipe, TranslateModule, TruncatePipe, TruncateTextDirective, NewlinePipe, SanitizePipe]
})
export class SharedModule { }
