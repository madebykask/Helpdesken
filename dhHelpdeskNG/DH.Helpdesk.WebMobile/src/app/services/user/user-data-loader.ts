import { UserSettingsService } from '.';
import { LoggerService } from '../logging';
import { AuthenticationService } from '../authentication';

export function initUserData(
  userSettingsService: UserSettingsService, 
  authService: AuthenticationService,
  logger: LoggerService
  ) : Function {
    return  () => {      
      return userSettingsService.applyUserSettings().toPromise();        
    };    
  }

