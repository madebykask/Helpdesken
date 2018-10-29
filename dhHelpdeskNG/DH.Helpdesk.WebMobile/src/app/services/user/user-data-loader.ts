import { UserSettingsService } from '.';
import { take, tap, catchError, retry } from 'rxjs/operators';
import { LoggerService } from '../logging';
import { AuthenticationService } from '../authentication';

export function initUserData(
  userSettingsService: UserSettingsService, 
  authService: AuthenticationService,
  logger: LoggerService
  ) : Function {
    return  () => {
     //check if user is authenticated
      if (authService.isAuthenticated()) {
        userSettingsService.applyUserSettings();
        //console.log('>> user-data-loader: response completed.');                        
      } else {
        //console.log('>>> user-data-load:  token is not valid. skipping loading user data');
        return Promise.resolve(null)
      }
      
      return Promise.resolve(null);
    };    
  }

