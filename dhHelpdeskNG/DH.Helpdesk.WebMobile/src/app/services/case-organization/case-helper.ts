import { OptionItem } from "../../models";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class CaseHelper {
    toOptionItems(jsArr: Array<any>): OptionItem[] {
        return jsArr.map(jsItem => new OptionItem(jsItem.name, jsItem.value));
    }
}