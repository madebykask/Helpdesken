import { BehaviorSubject, Observable } from 'rxjs';
import { pluck } from 'rxjs/operators';

export abstract class StoreBase<T> {
  private subject: BehaviorSubject<T>;
  private state$: Observable<T>;

  protected constructor (initialState: T) {
    this.subject = new BehaviorSubject(initialState);
    this.state$ = this.subject.asObservable();
  }

  get state() {
    return this.subject.getValue();
  }

  set(name: string, val: any) {
    let newState = this.subject.value;
    newState = {... this.state, [name]: val };
    this.raiseChanges(newState);
  }

  select<TState>(name: string): Observable<TState> {
    //return specific property as an observable
    return this.state$.pipe(
      pluck(name)
    );
  }

  private raiseChanges(newState: T) {
    this.subject.next(newState);
  }

}
