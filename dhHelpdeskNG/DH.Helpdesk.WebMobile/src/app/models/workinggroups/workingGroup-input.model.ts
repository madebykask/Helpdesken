export class WorkingGroupInputModel {
  public id: number;
  public isActive: number;
  public isDefault: boolean;
  public customerId: string;
  public code: string;
  public workingGroupName: string;
  public changedDate: Date;
  public createdDate: Date;
  public stateSecondaryId?: number;
}