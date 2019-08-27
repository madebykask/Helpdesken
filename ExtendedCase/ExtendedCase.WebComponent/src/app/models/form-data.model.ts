import { IKeyedCollection, KeyedCollection} from '../shared/keyed-collection'
import { FormStateModel } from './form-state.model';
import { FormStateItem } from './form-state.model';

export class FormDataModel {
    Id: number = 0;
    ExtendedCaseGuid: string = '';
    ExtendedCaseFormId: number = 0;

    CaseFieldsValues: IKeyedCollection<FieldValueModel> = new KeyedCollection<FieldValueModel>();
    ExtendedCaseFieldsValues: IKeyedCollection<FieldValueModel> = new KeyedCollection<FieldValueModel>();

    formState: FormStateModel = new FormStateModel();
}

export class FormDataSaveModel {
    UniqueId: string = '';
    HelpdeskCaseId: number = 0;
    FormId: number = 0;
    FieldsValues: FormFieldValueModel[] = [];
    CaseFieldsValues: FormFieldValueModel[] = [];
    FormState: FormStateItem[] = [];
};

export class FieldValueModel {

    constructor(
        public Value: string,
        public SecondaryValue?: string,
        public Pristine: boolean = true) {
    }
}

export class FormFieldValueModel {

    constructor(
        public FieldId: string,
        public Value: string,
        public SecondaryValue: string,
        public Properties: FieldProperties) {
    }
}

export class FieldProperties {
    constructor(public Pristine: boolean = true) {
    }
}

export class FormMetaDataResponse {
    Id: number;
    MetaData: string;
}

export class FormDataSaveResult  {
    Id:string = '';
    ExtendedCaseGuid:string = '';
    ExtendedCaseFormId:string = '';
}

export class FormListItem {
    Id: number;
    Name: string;
}
