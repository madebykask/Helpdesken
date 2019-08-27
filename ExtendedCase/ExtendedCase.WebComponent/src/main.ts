import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';
import {enableProdMode} from '@angular/core';
import { AppModule } from './app.module';

declare let ENV: any; // // to avoid compiler error. Using global variable from js.

if (ENV !== 'dev') {
    enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
    .catch((err: any) => console.error(err));
