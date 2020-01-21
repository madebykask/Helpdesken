import { Injectable } from '@angular/core';
import { NotifierApiService } from './api/notifier-api.service';
import { NotifierModel, NotifierSearchItem } from '../../shared-module/models/notifier/notifier.model';
import { take, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class NotifierService {

  constructor(private notifierApiService: NotifierApiService) {
  }

  searchNotifiers(query: string, categoryId: number = null, customerId: number): Observable<Array<NotifierSearchItem>> {
    return this.notifierApiService.search(query, categoryId, customerId).pipe(
      take(1),
      map((data: Array<any>) =>
        data.map(x => <NotifierSearchItem>Object.assign(new NotifierSearchItem(), x)))
    );
  }

  getNotifier(id: number, customerId: number): Observable<NotifierModel> {
    return this.notifierApiService.get(id, customerId).pipe(
      take(1),
      map(data => <NotifierModel>Object.assign(new NotifierModel(), data))
    );
  }
}
