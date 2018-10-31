import { Injectable } from "@angular/core";
import { Subject, Observable, Subscription } from "rxjs";
import { filter, map } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class CommunicationService {
  private publishSubscribeSubject$: Subject<any> = new Subject();
  emitter$: Observable<any>;

  constructor() {
    this.emitter$ = this.publishSubscribeSubject$.asObservable();
  }

  publish(channel:string, event:any):void {
    this.publishSubscribeSubject$.next({
      channel: channel,
      event: event
    });
  }

  subscribe(channel:string): Observable<CommEvent> {
    return this.emitter$
        .pipe(
            filter((emission: CommEvent) => emission.channel === channel),
            map((emission: CommEvent) => emission.event)
        )
  }
}

export class CommEvent {
    channel: string;
    event: any;
}

