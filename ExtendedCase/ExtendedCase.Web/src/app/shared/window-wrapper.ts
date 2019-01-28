import { Injectable } from '@angular/core';


@Injectable()
export class WindowWrapper {

    set extendedCaseComponentRef(cmp: any) {
        this.nativeWindow.extendedCaseComponentRef = cmp;
    }

    get extendedCaseComponentRef():any {
        return this.nativeWindow.extendedCaseComponentRef;
    }
    
    get nativeWindow(): any {
        return window;
    }

    setGlobal(name: string, obj: any) {
        if (this.nativeWindow[name] === undefined) {
            this.nativeWindow[name] = obj;
        }
    }

}