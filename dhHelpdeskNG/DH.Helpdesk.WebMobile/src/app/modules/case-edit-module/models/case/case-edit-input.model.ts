import { CaseFieldOptions } from 'src/app/modules/shared-module/constants';
import { CaseSectionType } from './case-section-input.model';

export interface IKeyValue {
  key: string;
  value: string;
}

export interface IFieldBase {
name: string;
label: string;
options: IKeyValue[];
isReadonly: boolean;
isRequired: boolean;
maxLength: number | null;
isHidden: boolean;
//JsonType: string;
}

export interface ICaseField<T> extends IFieldBase {
  value?: T;
  section?: CaseSectionType;
  setByTemplate: boolean;
}

export class CaseEditInputModel {
    id: number;
    customerId: number;
    caseNumber: number;
    caseGuid: string;
    editMode: CaseAccessMode;
    fields: ICaseField<any>[];
    caseSolution: CaseSolution;
    extendedCaseData: ExtendedCaseData;
    mailToTickets: MailToTicketInfo[];
    childCasesIds: number[] | null;
    parentCaseId: number | null;
}

export enum CaseAccessMode {
    NoAccess = 0,
    ReadOnly = 1,
    FullAccess = 2
}

export class CaseLockModel {
    isLocked: boolean;
    userId: number;
    lockGuid: string;
    browserSession: string;
    createdTime: Date;
    extendedTime: Date;
    extendValue: number;
    timerInterval: number;
    userFullName: string;
}

export class MailToTicketInfo {
    email: string;
    type: string;
    isHelpdesk: boolean;
}

export class CaseSolution {
    caseSolutionId: number;
    name: string;
    stateSecondaryId: number;
    defaultTab: string;
}

export class ExtendedCaseData {
    extendedCaseFormId: number;
    extendedCaseGuid: string;
    extendedCaseName: string;
}

// tslint:disable-next-line: max-classes-per-file
export class CaseFieldModel<T> implements ICaseField<T> {
    public name: string;
    public label: string;
    //public JsonType: string;
    public value?: T;
    public section?: CaseSectionType;
    public options: IKeyValue[];

    public get isReadonly(): boolean {
      return this.options != null && this.options.filter((o) => o.key === CaseFieldOptions.readonly).length > 0;
    }

    public get isRequired(): boolean {
      return this.options != null && this.options.filter((o) => o.key === CaseFieldOptions.required).length > 0;
    }

    public get maxLength(): number | null {
      if (this.options == null) { return null; }
      const maxLength = this.options.filter((o) => o.key === CaseFieldOptions.maxlength);
      return maxLength.length > 0 ? +maxLength[0].value : null;
    }

    public get setByTemplate(): boolean {
      return this.options != null && this.options.filter((o) => o.key === CaseFieldOptions.setByTemplate).length > 0;
    }

    public get isHidden(): boolean {
      return this.options != null && this.options.filter((o) => o.key === CaseFieldOptions.isHidden).length > 0;
    }
}

export class FieldModel implements IFieldBase {
  name: string;
  label: string;
  options: IKeyValue[];
  isReadonly: boolean;
  isRequired: boolean;
  maxLength: number;
  setByTemplate: boolean;
  isHidden: boolean;
}
