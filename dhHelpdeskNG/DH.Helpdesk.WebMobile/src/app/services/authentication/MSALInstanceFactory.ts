import { BrowserCacheLocation, IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';
import { config } from '@env/environment';

const isIE = window.navigator.userAgent.indexOf('MSIED ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: config.microsoftClientId, 
      authority: config.microsoftAuthority + (config.microsoftAuthority.endsWith('/') ? '' : '/') + config.microsoftTenant,
      redirectUri: config.microsoftRedirectUri,
      postLogoutRedirectUri: config.microsoftRedirectUri },
      cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage,
        storeAuthStateInCookie: isIE, // set to true for IE 11
      },
  });
}
