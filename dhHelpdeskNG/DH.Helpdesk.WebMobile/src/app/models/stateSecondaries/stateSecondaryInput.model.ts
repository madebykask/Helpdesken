export class StateSecondaryInputModel {
  public id: number;
  public customerId: number;
  public isActive: boolean;
  public noMailToNotifier: boolean;
  public name: string;
  public changedDate: Date;
  public createdDate: Date;
  public workingGroupId?: number;
  public isDefault: boolean;
  public reminderDays?: number;
  public recalculateWatchDate: boolean;
  public alternativeStateSecondaryName: string;
}
