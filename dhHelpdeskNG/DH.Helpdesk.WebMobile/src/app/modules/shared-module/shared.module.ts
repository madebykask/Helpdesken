import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsFilterPipe, DateTimezonePipe } from './pipes';

@NgModule({
  declarations: [ DateTimezonePipe, AlertsFilterPipe ],
  imports: [
    CommonModule
  ],
  exports: [ DateTimezonePipe, AlertsFilterPipe ]
})
export class SharedModule { }
