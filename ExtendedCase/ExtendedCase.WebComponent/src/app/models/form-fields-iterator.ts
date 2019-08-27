import { FieldModelBase, FormModel, SectionModel, SectionInstanceModel, TabModel } from '../models/form.model';
import { FormFieldPathModel } from '../models/form-field-path.model';


export class FormFieldsIterator {
    constructor(private formModel: FormModel) {
    }

    // iterate over 
    forEach (fieldAction: (formField: FieldModelBase, fieldPath: FormFieldPathModel) => void,
             sectionAction?: (sectionInstance: SectionInstanceModel, sectionIndex: number) => void,
             tabAction?: (tab: TabModel) => void) {
        //tabs
        for (let tabId of Object.keys(this.formModel.tabs)) {
            let tabModel = this.formModel.tabs[tabId];

            if (tabAction) {
                tabAction(tabModel);
            }

            //sections
            for (let sectionId of Object.keys(tabModel.sections)) {
                let sectionModel = tabModel.sections[sectionId];

                let sectionInstanceIndex = -1;

                //section instances
                for (let sectionInstance of sectionModel.instances) {
                    sectionInstanceIndex++;

                    if (sectionAction) {
                        sectionAction(sectionInstance, sectionInstanceIndex);
                    }

                    if (fieldAction && sectionInstance.fields) {
                        for (let fieldId of Object.keys(sectionInstance.fields)) {
                            let fieldModel = sectionInstance.fields[fieldId];

                            let fieldPath = new FormFieldPathModel(tabId, sectionId, sectionInstanceIndex, fieldId);
                            fieldAction(fieldModel, fieldPath);
                        }    
                    }
                }
            }
        }
    }
}
