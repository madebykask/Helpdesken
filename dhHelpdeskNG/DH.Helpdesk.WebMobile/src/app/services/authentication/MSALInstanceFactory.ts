import { IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';



export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: 'f263307c-2182-44c0-9c28-1b8e88c00a7b',
      redirectUri: 'http://localhost:4200'
    }
  });
}
