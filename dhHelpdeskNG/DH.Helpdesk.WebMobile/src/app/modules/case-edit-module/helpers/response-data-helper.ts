import { KeyValue } from '../models';

export class ResponseDataHelper {
  fromJSONKeyValue(json: any): KeyValue {
    if (typeof json === 'string') {
      json = JSON.parse(json);
    }
    return Object.assign(new KeyValue(), json, {});
  }
}
