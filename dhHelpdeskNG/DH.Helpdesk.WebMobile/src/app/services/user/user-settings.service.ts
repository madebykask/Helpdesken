import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map, filter } from 'rxjs/operators';
import { UserData } from '../../models'
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserSettingsService extends HttpApiServiceBase {
    
    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService){
        super(http, localStorageService);
    }

    loadUserSettings() {
        let isLoaded: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
        this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))//TODO: error handling
            .pipe(
                map((data: any) => {
                    if(data) {
                        let settings = new UserData();
                        settings.selectedCustomerId = data.customerId;//TODO: if no customer;
                        settings.selectedLanguageId = data.languageId;//TODO: if no language
                        return settings;
                    }
                }) 
            )
            .subscribe(
                data => {
                    var user = this.localStorageService.getCurrentUser();
                    if(data.selectedCustomerId)
                        user.currentData.selectedCustomerId = data.selectedCustomerId;
                    if(data.selectedLanguageId)
                        user.currentData.selectedLanguageId = data.selectedLanguageId;
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