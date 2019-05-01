import { IKeyValue } from '../models';

export class ResponseDataHelper {

  fromJSONKeyValue(json: any): IKeyValue {
    if (typeof json === 'string') {
      json = JSON.parse(json);
    }
    return <IKeyValue>{ ...json };
  }

}


