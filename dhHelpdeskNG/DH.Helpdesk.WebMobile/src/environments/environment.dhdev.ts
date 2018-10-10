declare const require: any;

export const environment = {
    production: true
  };
  
  export const config = {
    apiUrl: 'http://dhutvas3.datahalland.se:8049',
    clientId: 'hd',
    version: require('../../package.json').version,
    enableLog: true,
  }