import { OptionItem } from "../shared/optionItem.model";

export class BundledCaseOptions {
    customerRegistrationSources?: OptionItem[];
    systems?: OptionItem[];
    urgencies?: OptionItem[];
    impacts?: OptionItem[];
    suppliers?: OptionItem[];
    countries?: OptionItem[];
    currencies?: OptionItem[];
    workingGroups: OptionItem[];
    responsibleUsers: OptionItem[];
    performers: OptionItem[];
    priorities: OptionItem[];
    statuses: OptionItem[];
    stateSecondaries: OptionItem[];
    projects: OptionItem[];
    problems: OptionItem[];
    changes: OptionItem[];
    solutionsRates: OptionItem[];
}

export class CaseOptions extends BundledCaseOptions {
    regions?: OptionItem[];
    departments?: OptionItem[];
    oUs?: OptionItem[];
    isAboutDepartments?: OptionItem[];
    isAboutOUs?: OptionItem[];
    caseTypes?: MultiLevelOptionItem[];
    productAreas?: MultiLevelOptionItem[];
    categories?: MultiLevelOptionItem[];
    closingReasons?: MultiLevelOptionItem[];
    
}


export class MultiLevelOptionItem extends OptionItem {
    constructor(value: any, text: string, parentValue?: any, group:string = null, html: string = null, disabled: boolean = null) {
        super(value, text, group, html, disabled);
        this.parentValue = parentValue;
    }

    public parentValue?: any;
    public childs?: MultiLevelOptionItem[];
}