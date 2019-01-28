import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { ProxyModel } from '../models/proxy.model';
import {
    FormTemplateModel, TabTemplateModel, SectionTemplateModel, BaseControlTemplateModel,
    CustomQueryDataSourceTemplateModel, CustomStaticDataSourceTemplateModel, OptionsDataSourceTemplateModel, ControlCustomDataSourceTemplateModel,
    DataSourceParameterTemplateModel, IWithDataSourceParameters, IDataSourceParameter
} from '../models/template.model';

import { FormFieldPathModel } from '../models/form-field-path.model';
import { FormModel, SectionModel, SingleControlFieldModel, MultiControlFieldModel, FieldModelBase, ItemModel, CustomDataSourceModel } from '../models/form.model';
import { DataSourceService } from './data/data-source.service';
import { LogService } from './log.service'
import { ErrorHandlingService} from './error-handling.service';

@Injectable()
export class DataSourcesLoaderService {
    constructor(
        private dataSourceService: DataSourceService,
        private errorHandlingService : ErrorHandlingService, 
        private logService: LogService) {
    }

    loadCustomQueryDataSourceData(proxyModel: ProxyModel, dataSourceId: string, parameters: IDataSourceParameter[]): Observable<any> {
        this.logService.debugFormatted('loadCustomQueryDataSourceData: loading {0}. Parameters: {1}', dataSourceId, parameters);
        let params = this.prepareDataSourceQueryParameters(proxyModel, dataSourceId, parameters);
        if (params === undefined)
            return Observable.throw({ id: dataSourceId });

        return this.dataSourceService.getCustomDataSource(dataSourceId, params)
            .catch((e: any) => {
                this.errorHandlingService.handleError(e, `Failed to load custom dataSource '${dataSourceId}'`);
                return Observable.throw({ id: dataSourceId });
            })
            .flatMap((data: any) => {
                this.logService.debugFormatted('loadCustomQueryDataSourceData: success! Data retreived for ds {0}',  dataSourceId);
                return Observable.of(data);
            });
    }

    loadOptionsDataSourceData(proxyModel: ProxyModel, dataSourceId: string, parameters: IDataSourceParameter[]): Observable<any> {

        this.logService.info('loadOptionsDataSourceData: start');
        let params = this.prepareDataSourceQueryParameters(proxyModel, dataSourceId, parameters);
        if (params === undefined)
            return Observable.throw({ id: dataSourceId });

        return this.dataSourceService.getOptionDataSource(dataSourceId, params)
            .catch((e: any) => {
                this.errorHandlingService.handleError(e, `Failed to load options datasource '${dataSourceId }'`);
                return Observable.throw({ id: dataSourceId });
            })
            .flatMap((data: any) => {
                this.logService.infoFormatted('loadOptionsDataSourceData: success! loaded options data from {0}.', dataSourceId);
                return Observable.of(data);
            });
    }

    private prepareDataSourceQueryParameters(proxyModel: ProxyModel, dataSourceId: string, dsParameters: Array<IDataSourceParameter>): { [id: string]: string } {

        let resolvedParameters: { [id: string]: string } = {};
        dsParameters.forEach((paramTemplate) => {
            //resolve param value   
            let fieldPathModel = FormFieldPathModel.parse(paramTemplate.field);
            let proxyControl = proxyModel.findProxyControl(fieldPathModel);
            if (!proxyControl) {
                this.logService.warningFormatted('Failed to resolve parameter value for DataSource ${dataSourceId} query. Parameter ({0}) has undefined value.', paramTemplate.field);
                return undefined;
            }

            let paramValue = proxyControl.value || '';
            resolvedParameters[paramTemplate.name] = paramValue;
        });

        return resolvedParameters;
    }
}
