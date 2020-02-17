import { Component, OnInit, OnDestroy } from '@angular/core';
import { config } from '@env/environment';
import { CommunicationService, Channels } from 'src/app/services/communication/communication.service';
import { HeaderEventData } from 'src/app/services/communication/data/header-event-data';
import { takeUntil, delay } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss']
})
export class AppLayoutComponent implements OnInit, OnDestroy {
    version = config.version;
    isFooterVisible = true;
    pageSettings = {};

    constructor(private commService: CommunicationService) {
    }

    ngOnInit() {
        this.commService.listen<HeaderEventData>(Channels.Header)
        .pipe(
          delay(0),
          untilDestroyed(this)
        )
        .subscribe(e => {
          this.isFooterVisible = e.isVisible;
        });
    }

    ngOnDestroy(): void {
    }
}
