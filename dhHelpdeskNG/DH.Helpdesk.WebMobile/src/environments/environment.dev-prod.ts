import { VERSION } from './version';
declare const require: any;

export const environment = {
    production: true
  };

  export const config = {
    apiUrl: 'http://localhost:8049',
    clientId: 'hd',
    version: '#{ProductVersion}',
    internalVersion: '#{FileVersion}',
    enableLog: true,
    microsoftShowLogin: false,
    microsoftClientId: "",
    microsoftTenant: "",
    microsoftAuthority: "",
    microsoftRedirectUri: ""
  };
