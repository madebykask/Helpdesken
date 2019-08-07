import { Pipe, PipeTransform } from '@angular/core';
import { IOption } from '../../modules/ng-select/option.interface';
import { ItemModel, } from '../models/form.model';

@Pipe({ name: 'toNGSelectOptions', pure: true })
export class ToNGSelectOptions implements PipeTransform {
    transform(values: Array<ItemModel>): Array<IOption> {
        if(!values) return new Array<IOption>();
        let result = values.map((item: any) => {
            return <IOption>{
                value: item.value,
                label: item.text
            }
        });
        return result;
    }
}