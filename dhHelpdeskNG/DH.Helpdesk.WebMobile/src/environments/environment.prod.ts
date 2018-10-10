declare const require: any;

export const environment = {
  production: true
};

export const config = {
  apiUrl: 'http://localhost:8049',
  clientId: 'hd',
  version: require('../../package.json').version,
  enableLog: false,
}