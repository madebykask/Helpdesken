import { LoggerService } from '../logging';
import { AuthenticationService } from '../authentication/';
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";

export function initUserData(
  userSettingsService: UserSettingsApiService, 
  authService: AuthenticationService,
  logger: LoggerService
  ) : Function {
    return  () => {
      return userSettingsService.applyUserSettings().toPromise();
    };    
  }

