import { UuidGenerator } from 'src/app/modules/shared-module/utils/uuid-generator';

export class UserAuthenticationData {
  access_token: string;
  refresh_token: string;
  expires_in: number; // in seconds - not updated once recieved
  recievedAt: Date;
  sessionId: string;
}

export class UserData {
  id: number;
  name: string;
  selectedCustomerId: number;
  selectedLanguageId: number;
  userTimeZone?: string;
  ownCasesOnly: boolean;
  createCasePermission: boolean;
  canDeleteAttachedFiles: boolean;
}

export class CurrentUser {

    constructor() {
      this.authData = new UserAuthenticationData();
      this.currentData = new UserData();
    }

    // Properties
    get id(): number {
      return this.currentData.id;
    }

    get name(): string {
      return this.currentData.name;
    }

    version: string;
    authData: UserAuthenticationData;
    currentData: UserData;

    //create method
    static createAuthenticated(data: any): CurrentUser {
        const user = new CurrentUser();
        //set auth data
        const authData = new UserAuthenticationData();
        authData.access_token = data.access_token;
        authData.refresh_token = data.refresh_token;
        authData.expires_in = data.expires_in;
        authData.recievedAt = new Date();
        authData.sessionId = UuidGenerator.createUuid();
        user.authData = authData;
        return user;
    }
}
