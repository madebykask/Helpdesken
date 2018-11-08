import { HttpApiServiceBase } from "../api";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "../local-storage";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { OptionItem } from "../../models";
import { OptionsHelper } from "../../helpers/options-helper";

@Injectable({ providedIn: 'root' })
export class LanguagesService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService, 
        private optionsHelpder: OptionsHelper) {
            super(http, localStorageService);
    }

    getLanguages() {
        return this.getJson(this.buildResourseUrl('/api/languages/get', null, false, true))
        .pipe(
            map((jsItems: any) => {
                return this.optionsHelpder.toOptionItems(jsItems as Array<any>) || new Array<OptionItem>();
            })
        );//TODO: error handling
    }
}