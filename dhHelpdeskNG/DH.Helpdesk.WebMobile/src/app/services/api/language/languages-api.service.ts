import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { OptionsHelper } from "src/app/helpers";
import { LocalStorageService } from "../../local-storage";
import { OptionItem } from "src/app/modules/shared-module/models";
import { HttpApiServiceBase } from "src/app/modules/shared-module/services/api/httpServiceBase";

@Injectable({ providedIn: 'root' })
export class LanguagesApiService extends HttpApiServiceBase {

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