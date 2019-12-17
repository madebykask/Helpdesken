import { Injectable } from '@angular/core';
import { take, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UserApiService } from './api/case/user-api.service';
import { EmailsSearchItem } from './model/emails-search-item';

@Injectable({ providedIn: 'root' })
export class UserService {

  constructor(private userApiService: UserApiService) {
  }

  searchUsersEmails(query: string, customerId: number): Observable<Array<EmailsSearchItem>> {
    return this.userApiService.searchUsersEmails(query, customerId).pipe(
      take(1),
      map((data: Array<any>) => {
        let id = 0;
        return data.map(x => {
          id += 1; //psedo identifier
          return this.createSearchResultItem(x, id);
        });
      })
    );
  }

  private createSearchResultItem(data: any, id: number): EmailsSearchItem {
    return Object.assign(new EmailsSearchItem(id), data);
  }
}


