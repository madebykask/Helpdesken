import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map, filter } from 'rxjs/operators';
import { UserData } from '../../models'
import { LocalStorageService } from '../localStorage'
import { HttpApiServiceBase } from '../api'
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserSettingsService extends HttpApiServiceBase {
    
    protected constructor(http: HttpClient, private localStorageService: LocalStorageService){
        super(http);
    }

    loadUserSettings() {
        let isLoaded: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
        this.getJson(this.buildResourseUrl('/api/currentuser/settings'))//TODO: error handling
            .pipe(
                map((data: any) => {
                    if(data) {
                        var settings = new UserData();
                        settings.selectedCustomerId = Number(data.CustomerId);
                        return settings;
                    }
                }) 
            )
            .subscribe(
                data => {
                    var user = this.localStorageService.getCurrentUser();
                    if(data.selectedCustomerId)
                        user.currentData.selectedCustomerId = Number(data.selectedCustomerId);
                    // Other settings
                    this.localStorageService.setCurrentUser(user);
                    isLoaded.next(true);
                    return this
                }
            );
            return isLoaded.pipe(
                filter(success => success)
            );
    };
    
    getUserData(): UserData {
        var user = this.localStorageService.getCurrentUser();
        if (!user) return null;

        return user.currentData
    }

}