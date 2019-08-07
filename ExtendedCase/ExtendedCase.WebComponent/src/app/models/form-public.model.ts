
export class Form {
    tabs: Tab[] = [];
}

export class Tab {
    name: string;
    sections: Section[] = [];
}

export class Section {
    name: string;
    instances: SectionInstance[] = [];
}

export class SectionInstance {
    index: number;
    fields: Field[] = [];
}

export class Field {
    label: string;
    value: string;
    secondaryValue: string;
    caseBinding: string;
}