import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { State } from './state';
import { pluck } from 'rxjs/operators';

export const AppStoreKeys = {
  Templates: 'templates'
};

//initial state
const state: State =  {
  templates: []
};

@Injectable({ providedIn: 'root' })
export class AppStore {
  //subject stores state and subcribers about notifies about changes
  private subject = new BehaviorSubject<State>(state);
  private store$: Observable<State> = this.subject.asObservable();

  set(name: string, val: any) {
    let newState = this.subject.value;
    newState = {... this.value, [name]: val };
    this.raiseChanges(newState);
  }

  select<T>(name: string): Observable<T> {
    //return specific property as an observable
    return this.store$.pipe(
      pluck(name)
    );
  }

  get value() {
    return this.subject.value;
  }

  private raiseChanges(newState: State) {
    this.subject.next(newState);
  }

}
