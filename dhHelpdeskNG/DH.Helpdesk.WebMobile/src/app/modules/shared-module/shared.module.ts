import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimezonePipe } from './pipes';
import { TranslateModule } from '@ngx-translate/core';
import { TruncatePipe } from './pipes/truncate.pipe';
import { TruncateTextDirective } from './directives/truncate-text.directive';
import { NewlinePipe } from './pipes/newline.pipe';

@NgModule({
  declarations: [ DateTimezonePipe, AlertsFilterPipe, TruncatePipe, TruncateTextDirective, NewlinePipe ],
  imports: [
    CommonModule,
    TranslateModule.forChild()
  ],
  exports: [ DateTimezonePipe, AlertsFilterPipe, TranslateModule, TruncatePipe, TruncateTextDirective, NewlinePipe]
})
export class SharedModule { }
