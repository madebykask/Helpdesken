export class FilterMenuItemModel {
  constructor (
    public id: string,
    public text: string,
    public selected: boolean,
    public disabled: boolean = false) {
  }
}
