import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, finalize} from 'rxjs/operators';
import { UserData } from '../../models'
import { LocalStorageService } from '../local-storage'
import { HttpApiServiceBase } from '../api'

@Injectable({ providedIn: 'root' })
export class UserSettingsService extends HttpApiServiceBase {
    isLoadingUserSettings = false; 
    
    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService){
        super(http, localStorageService);
    }

    loadUserSettings() {
        this.isLoadingUserSettings = true;
        return this.getJson(this.buildResourseUrl('/api/currentuser/settings', undefined, false))//TODO: error handling
            .pipe(
                map((data: any) => {
                    if(data) {
                        let user = this.localStorageService.getCurrentUser();
                        if(data.customerId)
                            user.currentData.selectedCustomerId = data.customerId;//TODO: if no customer;
                        if(data.languageId)
                            user.currentData.selectedLanguageId = data.languageId;//TODO: if no language
                        // Other settings
                        this.localStorageService.setCurrentUser(user);

                        return user.currentData;
                    }
                }),
                finalize(() => this.isLoadingUserSettings = false )
            )
    };
    
    getUserData(): UserData {
        let user = this.localStorageService.getCurrentUser();
        if (!user) return null;

        return user.currentData
    }

    getCurrentLanguage(): number {
        return this.getUserData().selectedLanguageId;
    }

    setCurrentLanguage(languageId: number) {
        let user = this.localStorageService.getCurrentUser();
        user.currentData.selectedLanguageId = languageId;
        this.localStorageService.setCurrentUser(user);
    }
}