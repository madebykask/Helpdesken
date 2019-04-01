import { Injectable } from "@angular/core";
import { ClientLogApiService } from '../api';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { ClientLogEntryModel, ClientLogLevel } from "src/app/models/shared/client-log.model";
import { EnvironmentService } from "../environment/environment.service";
import { AuthenticationStateService } from "../authentication/authentication-state.service";
import { UuidGenerator } from "src/app/modules/shared-module/utils/uuid-generator";

@Injectable({ providedIn: 'root' })
export class InfoLoggerService { 
  constructor(private _apiLogger: ClientLogApiService,
     private _localStorageService: LocalStorageService,
     private _envService: EnvironmentService,
     private _authStateService: AuthenticationStateService) {}

  log(value: string) {
    let envInfo = this._envService.getEnvInfo();
    const user = this._localStorageService.getCurrentUser();

    let log = new ClientLogEntryModel();
    log.level = ClientLogLevel.Info;
    log.isAuthenticated = this._authStateService.isAuthenticated();
    log.url = this._envService.getUrl();
    log.message = value;
    log.param1 = JSON.stringify(envInfo);
    log.sessionId = user != null ? user.authData.sessionId : 'null';
    log.uniqueId =  UuidGenerator.createUuid();

    this._apiLogger.saveLogEntry(log).subscribe(); 
  }
}
