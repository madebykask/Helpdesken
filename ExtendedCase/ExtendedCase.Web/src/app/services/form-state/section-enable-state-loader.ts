import { FormModel, SectionModel, SectionInstanceModel } from '../../models/form.model';
import { IFormStateLoader } from './form-state-loader';
import { FormStateModel, FormStateItem, FormItemPath, FormStateKeys } from '../../models/form-state.model';
import * as cm from '../../utils/common-methods';


export class SectionEnableStateLoader implements IFormStateLoader {

    private get StateKey() {
        return FormStateKeys.enableStateSelection;
    }

    apply(formStateItem: FormStateItem, formModel: FormModel) {
        if (formStateItem.key === this.StateKey) {
            let section = formModel.findSectionSafe(formStateItem.tabId, formStateItem.sectionId);
            if (section && section.template.enableAction && 
                section.instances && section.instances.length > formStateItem.sectionIndex) {
                let sectionInstance = section.instances[formStateItem.sectionIndex];
                sectionInstance.sectionEnableStateSelection = cm.anyToBoolean(formStateItem.value);
            }
        }
    }

    get(formModel: FormModel): FormStateItem[] {
        let items: FormStateItem[] = [];

        formModel.createFieldsIterator()
            .forEach(null, (sectionInstance, sectionIndex) => this.saveSectionState(sectionInstance, sectionIndex, items, formModel));

        return items;
    }

    private saveSectionState(sectionInstance: SectionInstanceModel, sectionIndex: number, items: FormStateItem[], formModel: FormModel): void {
        let sectionModel = sectionInstance.section;
        let sectionTemplate = sectionInstance.section.template;

        if (sectionTemplate.enableAction) {
            let tabId = sectionModel.tab.id;
            let value = formModel.tabs[tabId].sections[sectionModel.id].instances[sectionIndex].sectionEnableStateSelection.toString();
            let item = new FormStateItem(sectionModel.tab.id, sectionModel.id, sectionIndex, this.StateKey, value);
            items.push(item);
        }
    }
}