export class FormParametersModel {
    extendedCaseGuid: string;
    formId: number;
    languageId: number;
    assignmentParameters: FormAssignmentParameters;
    caseId: number;
    userGuid: string;
    currentUser: string;
    isCaseLocked: boolean;
    applicationType: string;
    useInitiatorAutocomplete = true;

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
