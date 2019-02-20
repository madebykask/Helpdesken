import { VERSION } from './version'
declare const require: any;

export const environment = {
    production: true
  };
  
  export const config = {
    apiUrl: 'http://localhost:8049',
    clientId: 'hd',
    version: VERSION.fullVersion,
    internalVersion: require('../../package.json').version,
    enableLog: true,
  }