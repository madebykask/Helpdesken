import { OptionItem } from '../models';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class OptionsHelper {
    toOptionItems(jsArr: Array<any>): OptionItem[] {
        return jsArr.map(jsItem => new OptionItem(jsItem.value, jsItem.name));
    }
}