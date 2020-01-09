import { FormParametersModel } from './form-parameters.model';
import { FormFieldPathModel } from './form-field-path.model';
import * as cm from '../utils/common-methods';

export class ProxyModel {
    tabs: any;
    formInfo: FormInfo;
    localization: any;
    dataSources: any;
    globalFunctions: any;
    caseData: any;
    nextStep: string;

    constructor(formInfo: FormInfo, globalFunctions: {}, localization: {}) {
        this.formInfo = formInfo;
        this.tabs = {};
        this.dataSources = {};
        this.globalFunctions = globalFunctions;
        this.caseData = {};
        this.localization = localization;
    }

    findProxyControl(fieldPathModel: FormFieldPathModel): ProxyControl {
        let proxyTab = <ProxyTab>this.tabs[fieldPathModel.tabId];
        let proxySection = <ProxySection>proxyTab.sections[fieldPathModel.sectionId];

        let proxySectionInstance =
            <ProxySectionInstance>proxySection.instances[fieldPathModel.sectionInstanceIndex];

        return proxySectionInstance.controls[fieldPathModel.fieldId];
    }
}

export class ProxyTab {
    hidden: boolean;
    disabled: boolean;
    pristine: boolean;
    sections: any;

    constructor(public id: string, public name: string) {
        this.sections = {};
    }
}

export class ProxySection {
    hidden: boolean;
    disabled: boolean;
    pristine: boolean;
    instances: any[];

    constructor(public id: string, public name: string) {
        this.instances = [];
    }
}


export class ProxySingleInstanceSection extends ProxySection {
    controls: any;
    uniqueId: string;
    forceEnable: boolean;

    constructor(id: string, name: string) {
        super(id, name);
        this.controls = {};
    }
}

export class ProxySectionInstance {
    hidden: boolean;
    disabled: boolean;
    pristine: boolean;
    controls: any;
    uniqueId: string;
    forceEnable: boolean;

    dataSources: {[id: string]: any};

    constructor(uniqueId: string) {
        this.uniqueId = uniqueId;
        this.controls = {};
        this.dataSources = {};
    }
}

export class ProxyControl {
    id: string;
    hidden: boolean;
    disabled: boolean;
    pristine: boolean;
    selectedId: string; // selected option id of the searchbox
    value: any;
    secondaryValue: any;

    constructor(id: string) {
        this.id = id;
    }
}

export class FormInfo {
    caseId: number;
    userRole: number;
    caseStatus: number;
    customerId: number;
    languageId: number;
    formId: number;
    userGuid: string;
    currentUser: string;
    isCaseLocked: boolean;
    applicationType: string;
    useInitiatorAutocomplete: boolean;

    constructor(formParameters: FormParametersModel) {
        this.userRole = formParameters.assignmentParameters ? cm.parseIntOrDefault(formParameters.assignmentParameters.userRole, 0) : 0;
        this.caseStatus = formParameters.assignmentParameters ? cm.parseIntOrDefault(formParameters.assignmentParameters.caseStatus, 0) : 0;
        this.customerId = formParameters.assignmentParameters ? cm.parseIntOrDefault(formParameters.assignmentParameters.customerId, 0) : 0;
        this.languageId = cm.parseIntOrDefault(formParameters.languageId, 0);
        this.formId = cm.parseIntOrDefault(formParameters.formId, 0);
        this.caseId = cm.parseIntOrDefault(formParameters.caseId, 0);
        this.userGuid = formParameters.userGuid || '';
        this.currentUser = formParameters.currentUser || '';
        this.isCaseLocked = formParameters.isCaseLocked || false;
        this.applicationType = formParameters.applicationType || '';
        this.useInitiatorAutocomplete = formParameters.useInitiatorAutocomplete != null ? formParameters.useInitiatorAutocomplete : true;
    }
}
