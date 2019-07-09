import { Injectable } from '@angular/core';
import { LocalStorageService } from '../../../../services/local-storage';
import { HttpClient } from '@angular/common/http';
import { OptionsHelper } from '../../../../helpers/options-helper';
import { map, take } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { BundleOptionsFilter, BundledCaseOptions, OptionItem } from 'src/app/modules/shared-module/models';
import { HttpApiServiceBase } from 'src/app/modules/shared-module/services/api/httpServiceBase';

@Injectable({ providedIn: 'root' })
export class BundleCaseOptionsService extends HttpApiServiceBase {

    protected constructor(protected http: HttpClient, protected localStorageService: LocalStorageService,
        private caseHelper: OptionsHelper) {
            super(http, localStorageService);
    }

    getOptions(filter: BundleOptionsFilter) {
        return this.postJson(this.buildResourseUrl('/api/caseoptions/bundle', null, true, true), filter)
        .pipe(
            take(1),
            map((jsOptions: any) => {
                if (jsOptions == null) { throwError('No options received.'); }
                const mapArray = (arr: any) => this.caseHelper.toOptionItems(arr as Array<any>) || new Array<OptionItem>();

                const options = new BundledCaseOptions();
                if (jsOptions.customerRegistrationSources != null) {
                    options.customerRegistrationSources = mapArray(jsOptions.customerRegistrationSources);
                }
                if (jsOptions.systems != null) {
                    options.systems = mapArray(jsOptions.systems);
                }
                if (jsOptions.urgencies != null) {
                    options.urgencies = mapArray(jsOptions.urgencies);
                }
                if (jsOptions.impacts != null) {
                    options.impacts = mapArray(jsOptions.impacts);
                }
                if (jsOptions.suppliers != null) {
                    options.suppliers = mapArray(jsOptions.suppliers);
                }
                if (jsOptions.countries != null) {
                    options.countries = mapArray(jsOptions.countries);
                }
                if (jsOptions.currencies != null) {
                    options.currencies = mapArray(jsOptions.currencies);
                }
                if (jsOptions.responsibleUsers != null) {
                    options.responsibleUsers = mapArray(jsOptions.responsibleUsers);
                }
                if (jsOptions.priorities != null) {
                    options.priorities = mapArray(jsOptions.priorities);
                }
                if (jsOptions.statuses != null) {
                    options.statuses = mapArray(jsOptions.statuses);
                }
                if (jsOptions.projects != null) {
                    options.projects = mapArray(jsOptions.projects);
                }
                if (jsOptions.problems != null) {
                    options.problems = mapArray(jsOptions.problems);
                }
                if (jsOptions.changes != null) {
                    options.changes = mapArray(jsOptions.changes);
                }
                if (jsOptions.solutionsRates != null) {
                    options.solutionsRates = mapArray(jsOptions.solutionsRates);
                }
                if (jsOptions.causingParts != null) {
                    options.causingParts = mapArray(jsOptions.causingParts);
                }
                return options;
            })); // TODO: error handling
    }
}
