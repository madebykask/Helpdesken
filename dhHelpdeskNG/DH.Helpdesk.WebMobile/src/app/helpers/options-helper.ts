import { Injectable } from '@angular/core';
import { OptionItem, MultiLevelOptionItem } from '../modules/shared-module/models';

@Injectable({ providedIn: 'root' })
export class OptionsHelper {
    toOptionItems(jsArr: Array<any>): OptionItem[] {
        return jsArr.map(jsItem => new OptionItem(jsItem.value, jsItem.name));
    }

    getFromOptions(value: any, list: OptionItem[]) {
      const item = list.find(o => o.value == value);
      return item ? item.text : null;
    }

    getFromMultiLevelOptions(id: any, list: MultiLevelOptionItem[]) {
      const text = this.getTextChain(list, id);
      return text ? text : null;
    }

    public getTextChain(dataSource: MultiLevelOptionItem[], id: number | string) {
      if (dataSource == null || dataSource.length === 0) { return ''; }

      const chain = this.getOptionsChain(dataSource, id);
      return chain.length > 0 ? chain.map((elem) => elem.text).join(' > ') : '';
    }

    public  getOptionsChain(dataSource: MultiLevelOptionItem[], id: number | string) {
      if (dataSource == null || dataSource.length === 0) { return []; }

      let elems = new Array<MultiLevelOptionItem>();
      const searchNode = (elem: MultiLevelOptionItem, targetId: any): string => {
        const isText = typeof targetId === 'string';
        if ((isText && (elem.text === targetId)) || (elem.value === targetId)) {
          elems.push(elem);
           return elem.text;
          }
        if (elem.childs != null) {
          for (let i = 0; i < elem.childs.length; i++) {
            const text = searchNode(elem.childs[i], targetId);
            if (text != null) {
              elems.push(elem);
              return text;
            }
          }
        }
        return null;
      };
      dataSource.forEach((elem: MultiLevelOptionItem) => searchNode(elem, id));
      elems = elems.reverse();

      return  elems || [];
    }
}

