import { VERSION } from './version';
declare const require: any;

export const environment = {
  production: true
};

export const config = {
  apiUrl: '#{APIURL}',
  clientId: 'hd',
  version: '#{AssemblyInfo.ProductVersion}',
  internalVersion: '#{AssemblyInfo.FileVersion}',
  enableLog: false,
  microsoftShowLogin: '#{MicrosoftShowLogin}',
  microsoftClientId: '#{MicrosoftClientId}',
  microsoftTenant: '#{MicrosoftTenant}',
  microsoftAuthority: '#{MicrosoftAuthority}',
  microsoftRedirectUri: '#{MicrosoftRedirectUri}',
  microsoftDefaultAuthority : "https://login.microsoftonline.com/common/login"
};
