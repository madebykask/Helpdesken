export class OptionItem {
  constructor(value: any, text: string, group: string = null, html: string = null, disabled: boolean = null) {
      this.value = value;
      this.text = text;
      this.group = group;
      this.html = html;
      this.disabled = disabled;
  }
  public value: any;
  public text: string;
  public group?: string;
  public html?: string;
  public disabled?: boolean;
}

export class BundledCaseOptions {
    customerRegistrationSources?: OptionItem[];
    systems?: OptionItem[];
    urgencies?: OptionItem[];
    impacts?: OptionItem[];
    suppliers?: OptionItem[];
    countries?: OptionItem[];
    currencies?: OptionItem[];
    responsibleUsers?: OptionItem[];
    priorities?: OptionItem[];
    statuses?: OptionItem[];
    stateSecondaries?: OptionItem[];
    projects?: OptionItem[];
    problems?: OptionItem[];
    changes?: OptionItem[];
    solutionsRates?: OptionItem[];
    causingParts?: OptionItem[];
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
    workingGroups?: OptionItem[];
    performers?: OptionItem[];
    //options: unknown;
}

export class MultiLevelOptionItem extends OptionItem {
  constructor(value: any, text: string, parentValue?: any, group: string = null, html: string = null, disabled: boolean = null) {
      super(value, text, group, html, disabled);
      this.parentValue = parentValue;
  }

  public parentValue?: any;
  public childs?: MultiLevelOptionItem[];
}

