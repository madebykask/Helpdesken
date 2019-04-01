import { Injectable } from '@angular/core';
import { IFormStateLoader } from './form-state-loader';
import { FormStateModel, FormStateItem } from '../../models/form-state.model';
import { FormModel } from '../../models/form.model';
import { SectionEnableStateLoader } from './section-enable-state-loader';

@Injectable()
export class FormStateService {
    private stateLoaders: IFormStateLoader[] = [];

    constructor() {

        //add state loaders
        this.stateLoaders.push(new SectionEnableStateLoader());
    }

    // applies form state from db to form model
    applyFormState(formModel: FormModel, formState:FormStateModel) {
        if (!formState.Items)
            return;

        for (let stateItem of formState.Items) {
            for (let stateLoader of this.stateLoaders) {
                stateLoader.apply(stateItem, formModel);
            }
        }
    }

    // saves state from model to formState model
    updateFormState(formModel: FormModel, formState: FormStateModel): void {
        
        let items: FormStateItem[] = [];

        for (let stateLoader of this.stateLoaders) {
            // extract state with state loaders
            let loaderStateItems = stateLoader.get(formModel);
            if (loaderStateItems && loaderStateItems.length) {
                items.push(...loaderStateItems);
            }
        }

        //update form state with modified state values
        for (let item of items) {
            formState.updateStateItem(item);
        }
    }
}
