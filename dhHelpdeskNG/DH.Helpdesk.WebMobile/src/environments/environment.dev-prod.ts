import { VERSION } from './version';
declare const require: any;

export const environment = {
    production: true
  };

  export const config = {
    apiUrl: 'http://localhost:8080',
    clientId: 'hd',
    version: '#{AssemblyInfo.ProductVersion}',
    internalVersion: '#{AssemblyInfo.FileVersion}',
    enableLog: true,
    microsoftShowLogin: false,
    microsoftClientId: "",
    microsoftTenant: "",
    microsoftAuthority: "",
    microsoftRedirectUri: ""
  };
