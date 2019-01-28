import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimezonePipe } from './pipes';
import { TranslateModule } from '@ngx-translate/core';
import { TruncatePipe } from './pipes/truncate.pipe';

@NgModule({
  declarations: [ DateTimezonePipe, AlertsFilterPipe, TruncatePipe ],
  imports: [
    CommonModule,
    TranslateModule.forChild()
  ],
  exports: [ DateTimezonePipe, AlertsFilterPipe, TranslateModule, TruncatePipe]
})
export class SharedModule { }
