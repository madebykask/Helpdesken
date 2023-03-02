
import 'zone.js/dist/zone-error';
import { VERSION } from './version';
declare const require: any;

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false
};

export const config = {
    apiUrl: 'https://localhost:449/api',
    clientId: 'hd',
    version: '#{AssemblyInfo.ProductVersion}',
    internalVersion: '#{AssemblyInfo.FileVersion}',
    enableLog: true,
    microsoftShowLogin: true,
    microsoftClientId: "f263307c-2182-44c0-9c28-1b8e88c00a7b",
    microsoftTenant: "a1f945cf-f91f-4b88-a250-a58e3dd50140",
    microsoftAuthority: "https://login.microsoftonline.com",
    microsoftRedirectUri: "https://localhost:449/mobile/",
    microsoftDefaultAuthority : "https://login.microsoftonline.com/common/login"
};
 
/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.