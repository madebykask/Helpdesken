import { CaseOptions } from "./case-options.model";

export class OptionsDataSource {
    public options: CaseOptions;

    constructor(options: CaseOptions) {
        this.options = options;
    }
}