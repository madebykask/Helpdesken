import { CaseFileModel } from './case-file.model';

export class FormParametersModel {
    extendedCaseGuid: string;
    formId: number;
    languageId: number;
    assignmentParameters: FormAssignmentParameters;
    caseId: number;
    caseGuid: string;
    caseNumber: string;
    userGuid: string;
    currentUser: string;
    isCaseLocked: boolean;
    applicationType: string;
    useInitiatorAutocomplete = true;
    isMobile = false;

    caseFiles: CaseFileModel[];

    constructor() {
    }
}

export class FormAssignmentParameters {
    constructor(
        public userRole: string,
        public caseStatus: number,
        public customerId: number) {
    }
}
