import { Injectable } from '@angular/core';
import { take, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UserApiService } from './api/case/user-api.service';
import { EmailsSearchItem } from './model/emails-search-item';

@Injectable({ providedIn: 'root' })
export class UserService {

  constructor(private userApiService: UserApiService) {
  }

  searchUsersEmails(query: string): Observable<Array<EmailsSearchItem>> {
    return this.userApiService.searchUsersEmails(query).pipe(
      take(1),
      map((data: Array<any>) => data.map(x => this.createSearchResultItem(x)))
    );
  }

  private createSearchResultItem(data: any): EmailsSearchItem {
    return Object.assign(new EmailsSearchItem(), data);
  }
}


