import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class WindowWrapper {    
   
    get nativeWindow(): any {
        return getWindow();
    }

    setGlobal(name: string, obj: any) {
        if (this.nativeWindow[name] === undefined) {
            this.nativeWindow[name] = obj;
        }
    }
}

function getWindow(): any {
    return window;
}