import { FormModel } from '../../models/form.model';
import { FormStateItem } from '../../models/form-state.model';

export interface IFormStateLoader {
    apply(formStateItem: FormStateItem, formModel: FormModel): void;
    get(formModel: FormModel): FormStateItem[];
}
