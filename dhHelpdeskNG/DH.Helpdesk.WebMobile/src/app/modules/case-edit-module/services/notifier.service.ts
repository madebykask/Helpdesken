import { Injectable } from '@angular/core';
import { NotifierApiService } from './api/notifier-api.service';
import { NotifierModel } from '../../shared-module/models/notifier/notifier.model';
import { take, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotifierService {

  constructor(private notifierApiService:NotifierApiService) {   
  }

  getNotifier(id:number): Observable<NotifierModel> {
    return this.notifierApiService.get(id).pipe(
      take(1),
      map(data => <NotifierModel>Object.assign(new NotifierModel(), data))
    );
  }

}
