import { Component, OnInit, OnDestroy } from '@angular/core';
import { config } from '@env/environment';
import { CommunicationService, Channels } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/header-event-data';
import { takeUntil, delay } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss']
})
export class AppLayoutComponent implements OnInit, OnDestroy {
    private _destroy$ = new Subject();
    version = config.version;
    isFooterVisible = true;
    constructor(private _commService: CommunicationService) { }

    ngOnInit() {
        this._commService.listen<HeaderEventData>(Channels.Header)
        .pipe(
          delay(0),
          takeUntil(this._destroy$)
        )
        .subscribe(e => {
          this.isFooterVisible = e.isVisible;
        });
    }

    ngOnDestroy(): void {
        this._destroy$.next();
      }

}