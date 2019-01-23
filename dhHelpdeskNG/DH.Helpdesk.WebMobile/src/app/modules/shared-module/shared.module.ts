import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimezonePipe } from './pipes';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [ DateTimezonePipe, AlertsFilterPipe ],
  imports: [
    CommonModule,
    TranslateModule.forChild()
  ],
  exports: [ DateTimezonePipe, AlertsFilterPipe, TranslateModule ]
})
export class SharedModule { }
