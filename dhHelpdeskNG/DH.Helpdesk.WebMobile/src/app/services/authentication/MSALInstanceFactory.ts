import { BrowserCacheLocation, IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';

const isIE = window.navigator.userAgent.indexOf('MSIED ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: 'f263307c-2182-44c0-9c28-1b8e88c00a7b',
      authority: 'https://login.microsoftonline.com/a1f945cf-f91f-4b88-a250-a58e3dd50140',
      redirectUri: window.location.origin,
      postLogoutRedirectUri: window.location.origin },
      cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage,
        storeAuthStateInCookie: isIE, // set to true for IE 11
      },
  });
}
