import { Injectable } from '@angular/core';
import { FormParametersModel, FormAssignmentParameters } from '../models/form-parameters.model';
import { ActivatedRoute } from '@angular/router'
import * as cm from '../utils/common-methods';

@Injectable()
export class QueryParamsService {
    constructor(private activatedRoute: ActivatedRoute) {  }

    getAutoloadValue(): boolean {
        const queryParams = this.activatedRoute.snapshot.queryParams;
        const autoload = queryParams['autoLoad'];
        return autoload ? true : false;
    }

    getFormParamenters(): FormParametersModel {

        let queryParams = this.activatedRoute.snapshot.queryParams;

        let parametersModel = new FormParametersModel();
        parametersModel.extendedCaseGuid = queryParams['extendedCaseGuid'] || '';
        parametersModel.formId = parseInt(queryParams['formId']);
        parametersModel.languageId = parseInt(queryParams['languageId']);
        parametersModel.userGuid = queryParams['userGuid'];
        parametersModel.currentUser = queryParams['currentUser'];
        parametersModel.isCaseLocked = cm.anyToBoolean(queryParams['isCaseLocked']);

        let userRole = queryParams['userRole'];
        let caseStatus = parseInt(queryParams['caseStatus']);
        let customerId = parseInt(queryParams['customerId']);

        if (userRole || caseStatus || customerId) {
            parametersModel.assignmentParameters = new FormAssignmentParameters(userRole, caseStatus, customerId);
        }

        return parametersModel;
    }
}