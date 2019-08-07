import { Injectable } from '@angular/core';
import { TabTemplateModel, SectionTemplateModel, BaseControlTemplateModel } from '../models/template.model';
import { FieldModelBase, TabModel, SectionModel, SectionInstanceModel, SingleControlFieldModel, MultiValueSingleControlFieldModel, MultiControlFieldModel, CustomDataSourceModel, ItemModel, FormControlType } from '../models/form.model';
import { ProxyModel, ProxyTab, ProxySection, ProxySingleInstanceSection, ProxySectionInstance,  ProxyControl } from '../models/proxy.model';
import * as commonMethods from '../utils/common-methods';

@Injectable()
export class ProxyModelBuilder {
    createProxyTab(proxyModel: ProxyModel, tabModel: TabModel, tab: TabTemplateModel): ProxyTab {

        let proxyTab = new ProxyTab(tab.id, tab.name);
        proxyModel.tabs[tab.id] = proxyTab;

        Object.defineProperty(proxyModel.tabs[tab.id],
            'hidden',
            {
                get: () => { return tabModel.hidden; },
                enumerable: true
            });

        Object.defineProperty(proxyModel.tabs[tab.id],
            'disabled',
            {
                get: () => { return tabModel.disabled; },
                enumerable: true
            });

        Object.defineProperty(proxyModel.tabs[tab.id],
            'pristine',
            {
                get: () => { return tabModel.group.pristine; },
                enumerable: true
            });

        return proxyTab;
    }

    createSingleSectionModelProxy(proxyModel: ProxyModel,
        sectionTpl: SectionTemplateModel,
        sectionModel: SectionModel): ProxySection {
        let proxySection = new ProxySingleInstanceSection(sectionTpl.id, sectionTpl.name);

        Object.defineProperty(proxySection,
            'hidden',
            {
                get: () => { return sectionModel.instances[0].hidden; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'disabled',
            {
                get: () => { return sectionModel.instances[0].disabled; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'pristine',
            {
                get: () => { return sectionModel.instances[0].group.pristine; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'controls',
            {
                get: () => { return proxySection.instances && proxySection.instances.length > 0 ? proxySection.instances[0].controls : null;; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'forceEnable',
            {
                get: () => {
                    return proxySection.instances && proxySection.instances.length > 0 ? proxySection.instances[0].forceEnable : false;
                },
                enumerable: true
            });

        return proxySection;
    }

    createMultiSectionModelProxy(proxyModel: ProxyModel,
        sectionTpl: SectionTemplateModel,
        sectionModel: SectionModel): ProxySection {

        let proxySection = new ProxySection(sectionTpl.id, sectionTpl.name);

        Object.defineProperty(proxySection,
            'hidden',
            {
                get: () => { return sectionModel.hidden; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'disabled',
            {
                get: () => { return sectionModel.disabled; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'pristine',
            {
                get: () => { return sectionModel.group.pristine; },
                enumerable: true
            });

        Object.defineProperty(proxySection,
            'first',
            {
                get: () => {
                    return proxySection.instances && proxySection.instances.length > 0 ? proxySection.instances[0] : null;
                },
                enumerable: true
            });

        return proxySection;
    }

    createSectionInstanceProxy(proxyModel: ProxyModel,
        control: BaseControlTemplateModel,
        fieldModel: FieldModelBase,
        sectionInstance: SectionInstanceModel): ProxySectionInstance {

        let proxySectionInstance = new ProxySectionInstance(sectionInstance.id);

        Object.defineProperty(proxySectionInstance,
            'hidden',
            {
                get: () => { return sectionInstance.hidden; },
                enumerable: true
            });

        Object.defineProperty(proxySectionInstance,
            'disabled',
            {
                //get: () => { return sectionInstance.group.disabled; },//todo: check why group is checked instead of model?!
                get: () => { return sectionInstance.disabled; },
                enumerable: true
            });

        Object.defineProperty(proxySectionInstance,
            'pristine',
            {
                get: () => { return sectionInstance.group.pristine; },
                enumerable: true
            });

        Object.defineProperty(proxySectionInstance,
            'forceEnable',
            {
                get: () => { return sectionInstance.sectionEnableStateSelection; },
                enumerable: true
            });

        //crrate proxy accessor for each section instance datasource item
        if (sectionInstance.dataSources) {
            proxySectionInstance.dataSources = {};
            for (let dataSourceId of Object.keys(sectionInstance.dataSources)) {
                if (!proxySectionInstance.dataSources.hasOwnProperty(dataSourceId)) {
                    Object.defineProperty(proxySectionInstance.dataSources,
                        dataSourceId,
                        {
                            get: () => {
                                var data = sectionInstance.dataSources[dataSourceId].getData() || [];
                                 return data.slice();
                            },
                            enumerable: true
                        });
                }
            }
        }
        
        return proxySectionInstance;
    }

    createFieldModelProxy(controlTpl: BaseControlTemplateModel, fieldModel: FieldModelBase, sectionInstance: ProxySectionInstance): ProxyControl {

        if (fieldModel instanceof SingleControlFieldModel) {
            return this.createSingleControlFieldProxy(controlTpl, fieldModel, sectionInstance);
        } else if (fieldModel instanceof MultiControlFieldModel) {
            return this.createMultiControlFieldProxy(fieldModel, sectionInstance);
        } else {
            throw `Not supported fieldModel type (${fieldModel})`;
        }
    }

    private createSingleControlFieldProxy(controlTpl: BaseControlTemplateModel,
        fieldModel: SingleControlFieldModel,
        sectionInstance: ProxySectionInstance)
        : ProxyControl {

        let proxyControl = new ProxyControl(fieldModel.id);

        Object.defineProperty(proxyControl,
            'multiId',
            {
                get: () => { return fieldModel.getFieldPath().buildFormFieldPath(false); },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'value',
            {
                get: () => { return fieldModel.control.value; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'secondaryValue',
            {
                get: () => {
                    if (fieldModel instanceof MultiValueSingleControlFieldModel) {
                        return this.getMultiValueSingleControlSecondaryValue(fieldModel);
                    } else if (controlTpl.controlType === FormControlType.Search) {
                        return fieldModel.additionalData; //search only
                    }

                    let selectedItem = fieldModel.items ? fieldModel.items.find((x: ItemModel) => x.value === fieldModel.control.value) : null;
                    return selectedItem ? selectedItem.text : null;
                },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'hidden',
            {
                get: () => { return fieldModel.hidden; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'disabled',
            {
                get: () => { return fieldModel.control.disabled; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'pristine',
            {
                get: () => { return fieldModel.control.pristine; },
                enumerable: true
            });

        let parent = sectionInstance;
        Object.defineProperty(proxyControl,
            'parent',
            {
                get: () => { return parent },
                enumerable: false // to avoid circular ref error on json serialisation
            });

        return proxyControl;
    }

    private createMultiControlFieldProxy(fieldModel: MultiControlFieldModel, sectionInstance: ProxySectionInstance): ProxyControl {

        let proxyControl = new ProxyControl(fieldModel.id);

        Object.defineProperty(proxyControl,
            'multiId',
            {
                get: () => { return fieldModel.getFieldPath().buildFormFieldPath(false); },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'value',
            {
                get: () => { return fieldModel.controls.value; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'secondaryValue',
            {
                get: () => {
                    return this.getMultiControlSecondaryValues(fieldModel);
                },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'hidden',
            {
                get: () => { return fieldModel.hidden; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'disabled',
            {
                get: () => { return fieldModel.controls.disabled; },
                enumerable: true
            });

        Object.defineProperty(proxyControl,
            'pristine',
            {
                get: () => { return fieldModel.controls.pristine; },
                enumerable: true
            });

        let parent = sectionInstance;
        Object.defineProperty(proxyControl,
            'parent',
            {
                get: () => { return parent },
                enumerable: false // to avoid circular ref error on json serialisation
            });

        return proxyControl;
    }

    private getMultiValueSingleControlSecondaryValue(fieldModel: MultiValueSingleControlFieldModel): string[] {
        let value = commonMethods.isUndefinedNullOrEmpty(fieldModel.control.value)
            ? []
            : fieldModel.control.value;

        let selectedTexts: string[] = [];

        if (!commonMethods.isArray(value)) {
            value = Array.of(value);}

        value.forEach((v: any) => {
            let selectedItem = fieldModel.items ? fieldModel.items.find((x: ItemModel) => x.value === v) : null;
            if (selectedItem)
                selectedTexts.push(selectedItem.text);
        });

        return selectedTexts;
    }

    private getMultiControlSecondaryValues(fieldModel: MultiControlFieldModel): string[] {
        //"Value": { "val1": "true", "val2": "true", "val3": true }        
        let selectedTexts: string[] = [];

        for (let valKey of Object.keys(fieldModel.controls.value)) {
            let val = fieldModel.controls.value[valKey];

            val = commonMethods.anyToBoolean(val);
            if (val === true) {
                fieldModel.items.forEach((item: ItemModel) => {
                    if (item.value === valKey)
                        selectedTexts.push(item.text);
                });
            }
        }

        return selectedTexts;
    }
}