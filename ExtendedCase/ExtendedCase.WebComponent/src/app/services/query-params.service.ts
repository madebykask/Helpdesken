import { Injectable } from '@angular/core';
import { FormParametersModel, FormAssignmentParameters } from '../models/form-parameters.model';
import { ActivatedRoute, Router } from '@angular/router'
import * as cm from '../utils/common-methods';
import { WindowWrapper } from '../shared/window-wrapper';

@Injectable()
export class QueryParamsService {
    constructor(private activatedRoute: ActivatedRoute, private window: WindowWrapper) {  }

    getAutoloadValue(): boolean {
        const queryParams = this.activatedRoute.snapshot.queryParams;
        let params = this.getUrlParameters();
        const autoload = queryParams['autoLoad'] || params.get('autoLoad');
        return autoload ? true : false;
    }

    getFormParamenters(): FormParametersModel {

        let queryParams = this.activatedRoute.snapshot.queryParams;
        let params = this.getUrlParameters();

        let parametersModel = new FormParametersModel();
        parametersModel.extendedCaseGuid = (queryParams['extendedCaseGuid'] || params.get('extendedCaseGuid')) || '';
        parametersModel.formId = parseInt(queryParams['formId'] || params.get('formId'), 10);
        parametersModel.languageId = parseInt(queryParams['languageId'] || params.get('languageId'), 10);
        parametersModel.userGuid = queryParams['userGuid'] || params.get('userGuid');
        parametersModel.currentUser = queryParams['currentUser'] || params.get('currentUser');
        parametersModel.isCaseLocked = cm.anyToBoolean(queryParams['isCaseLocked'] || params.get('isCaseLocked'));
        parametersModel.useInitiatorAutocomplete =
                                        cm.anyToBoolean(queryParams['useInitiatorAutocomplete'] || params.get('useInitiatorAutocomplete'));

        let userRole = queryParams['userRole'] || params.get('userRole');
        let caseStatus = parseInt(queryParams['caseStatus'] || params.get('caseStatus'), 10);
        let customerId = parseInt(queryParams['customerId'] || params.get('customerId'), 10);

        if (userRole || caseStatus || customerId) {
            parametersModel.assignmentParameters = new FormAssignmentParameters(userRole, caseStatus, customerId);
        }

        return parametersModel;
    }

    private getUrlParameters(): URLSearchParams {
      return new URLSearchParams(this.window.nativeWindow.location.search);
    }
}
