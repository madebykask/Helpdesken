import { SortOrder } from 'src/app/modules/shared-module/constants';

export class CasesSortMenuItemModel {
  constructor(
    public id: number,
    public text: string,
    public fieldId: string,
    public sortOrder: string = SortOrder.SortAsc,
    public selected = false) {
    }
}
