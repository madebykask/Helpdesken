import { Injectable } from "@angular/core";
import { Subject, Observable, Subscription } from "rxjs";
import { filter, map } from "rxjs/operators";
import { NotifierModel, NotifierType } from "src/app/modules/shared-module/models/notifier/notifier.model";

@Injectable({ providedIn: 'root' })
export class CommunicationService {
  private _publishSubscribeSubject$: Subject<any> = new Subject();
  emitter$: Observable<CommEvent>;

  constructor() {
    this.emitter$ = this._publishSubscribeSubject$.asObservable();
  }

  publish(channel: Channels, event: any):void {
    this._publishSubscribeSubject$.next({
      channel: channel,
      event: event
    });
  }

  listen<T>(channel: Channels): Observable<T> {
    return this.emitter$
        .pipe(
            filter(emission => emission.channel === channel),
            map(emission => <T>emission.event)
        )
  }
}

export class CommEvent {
    channel: Channels;
    event: any;
}

export enum Channels {
    Header,
    AuthenticationChange,
    DropdownValueChanged,
    NotifierChanged
}

export class NotifierChangedEvent {
  constructor(public notifier: NotifierModel, public type: NotifierType) {
  }
}

export class DropdownValueChangedEvent {
  constructor(public value: any, public text: string, public name: string) {
  }
}