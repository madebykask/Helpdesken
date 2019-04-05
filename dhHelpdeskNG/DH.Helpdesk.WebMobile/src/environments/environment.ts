
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
      // apiUrl: 'http://dhutvas3.datahalland.se:8049',
      apiUrl: 'http://192.168.1.182:8049',
      clientId: 'hd',
      version: VERSION.fullVersion,
      internalVersion: require('../../package.json').version,
      enableLog: true,
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI. 
