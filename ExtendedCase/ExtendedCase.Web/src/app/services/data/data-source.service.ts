import { throwError,  Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../shared/app-config/app-config';
import { IAppConfig } from 'src/app/shared/app-config/app-config.interface';

@Injectable()
export class DataSourceService {
    constructor(private http: HttpClient,
                @Inject(AppConfig) private config: IAppConfig) {
    }

    getOptionDataSource(id: string, paramsObject: { [name: string]: any; }): Observable<any> {
        let url = this.config.apiHost + '/api/OptionDataSourceData/';
        return this.http
            .post(url, this.parseParams(id, paramsObject)).pipe(
            catchError((error: any) => throwError(error)));
    }

    getCustomDataSource(id: string, paramsObject: { [name: string]: any; }): Observable<any> {
        let url = this.config.apiHost + '/api/CustomDataSourceData/';
        return this.http
            .post(url, this.parseParams(id, paramsObject)).pipe(
            catchError((error: any) => throwError(error)));
    }

    private parseParams(id: string, paramsObject: { [name: string]: any; }): { [name: string]: any; } {
        let paramsJson = {
            Id: id,
            Params: new Array()
        };

        for (let paramName of Object.keys(paramsObject)) {
            paramsJson.Params.push({
                Name: paramName,
                Value: paramsObject[paramName]
            });
        }
        return paramsJson;
    }
}
