import { Injectable } from '@angular/core';
import { OptionItem } from '../modules/shared-module/models';

@Injectable({ providedIn: 'root' })
export class OptionsHelper {
    toOptionItems(jsArr: Array<any>): OptionItem[] {
        return jsArr.map(jsItem => new OptionItem(jsItem.value, jsItem.name));
    }
}