import { Injectable } from '@angular/core';
import { ProxyModelBuilder } from './proxy-model-builder';
import { TabTemplateModel, SectionTemplateModel, BaseControlTemplateModel } from '../models/template.model';
import { FieldModelBase, TabModel, SectionModel, SectionInstanceModel, CustomDataSourceModel } from '../models/form.model';
import { ProxyModel, ProxySection, ProxySectionInstance } from '../models/proxy.model';
import { FieldValueModel } from '../models/form-data.model'
import { IKeyedCollection } from '../shared/keyed-collection'

@Injectable()
export class ProxyModelService {

    constructor(private pmb: ProxyModelBuilder) {
    }

    createDataSourceProperty(
        targetElem: any,
        dataSourceId: string,
        dataSourceModel: CustomDataSourceModel
    ): void {
        if (!targetElem.dataSources.hasOwnProperty(dataSourceId)) {
            Object.defineProperty(targetElem.dataSources,
                dataSourceId,
                {
                    get: () => { return dataSourceModel.getData().slice(); },
                    enumerable: true
                });
        }
    }

    createCaseData(
        proxyModel: ProxyModel,
        fieldsValues: IKeyedCollection<FieldValueModel>): void {

        // reset previous values
        proxyModel.caseData = {};
        if (!fieldsValues) {
            return;
        }

        fieldsValues.getKeys().forEach((key: string) => {
            let fieldValue: FieldValueModel = fieldsValues.getItem(key);
            if (!proxyModel.caseData.hasOwnProperty(key)) {
                Object.defineProperty(proxyModel.caseData,
                    key,
                    {
                        get: () => { return fieldValue.Value; },
                        enumerable: true
                    });
            }
        });
    }

    createProxyModelProperty(
        proxyModel: ProxyModel,
        tab: TabTemplateModel,
        section: SectionTemplateModel,
        control: BaseControlTemplateModel,
        tabModel: TabModel,
        sectionModel: SectionModel,
        sectionInstance: SectionInstanceModel,
        fieldModel: FieldModelBase): void {

        // create or get tab proxy
        let proxyTab: any;
        if (!proxyModel.tabs.hasOwnProperty(tab.id)) {
            proxyTab = this.pmb.createProxyTab(proxyModel, tabModel, tab);
        } else {
            proxyTab = proxyModel.tabs[tab.id];
        }

        // create or get section proxy
        let proxySection: any;
        if (!proxyTab.sections.hasOwnProperty(section.id)) {

            proxySection = section.multiSectionAction && section.multiSectionAction.allowMultipleSections
                ? this.pmb.createMultiSectionModelProxy(proxyModel, section, sectionModel)
                : this.pmb.createSingleSectionModelProxy(proxyModel, section, sectionModel);

            proxyTab.sections[section.id] = proxySection;
        } else {
            proxySection = proxyModel.tabs[tab.id].sections[section.id];
        }

        // find or create section instance proxy
        let proxySectionInstance = proxySection.instances.find((inst: any) => inst.uniqueId === sectionInstance.id);
        if (!proxySectionInstance) {
            proxySectionInstance = this.pmb.createSectionInstanceProxy(proxyModel, control, fieldModel, sectionInstance);

            // add new instance to proxy collection
            proxySection.instances.push(proxySectionInstance);
        }

        // create control proxy
        proxySectionInstance.controls[control.id] = this.pmb.createFieldModelProxy(control, fieldModel, proxySectionInstance);
    }

    removeSectionInstance(sectionInstance: SectionInstanceModel, proxyModel: ProxyModel) {
        let section = sectionInstance.section;
        let tab = section.tab;

        let sectionModelProxy = <ProxySection>proxyModel.tabs[tab.id].sections[section.id];
        if (sectionModelProxy) {
            // remove from instances collection
            sectionModelProxy.instances =
                sectionModelProxy.instances.filter((item: ProxySectionInstance) => item.uniqueId !== sectionInstance.id);
        }
    }
}
