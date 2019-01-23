import { CaseOptions } from "src/app/modules/shared-module/models/case/case-options.model";

export class OptionsDataSource {
    public options: CaseOptions;

    constructor(options: CaseOptions) {
        this.options = options;
    }
}