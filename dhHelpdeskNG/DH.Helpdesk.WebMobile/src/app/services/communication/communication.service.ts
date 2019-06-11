import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class CommunicationService {
  private publishSubscribeSubject$: Subject<any> = new Subject();
  emitter$: Observable<CommEvent>;

  constructor() {
    this.emitter$ = this.publishSubscribeSubject$.asObservable();
  }

  publish(channel: Channels, event: any): void {
    this.publishSubscribeSubject$.next({
      channel: channel,
      event: event
    });
  }

  listen<T>(channel: Channels): Observable<T> {
    return this.emitter$.pipe(
        filter(emission => emission.channel === channel),
        map(emission => <T>emission.event)
    );
  }
}

export class CommEvent {
    channel: Channels;
    event: any;
}

export enum Channels {
    Header,
    AuthenticationChange,
    CaseFieldValueChanged
}

export class CaseFieldValueChangedEvent {
  constructor(public value: any,
              public text: string,
              public name: string) {
  }
}
