
export enum NotifierType {
  Initiator = 1,
  Regarding
}

export class NotifierModel {
  constructor() {
  }

  id: number;
  userId: string;
  name: string;
  email: string;
  place: string;
  phone: string;
  userCode: string;
  cellphone: string;
  regionId?: number;
  regionName: string;
  departmentId?: number;
  departmentName: string;
  ouId?: number;
  ouName: string;
  costCentre: string;
}

export class NotifierSearchItem {
    constructor() {
    }
    id: number;
    userId: string;
    firstName: string;
    surName: string;
    email: string;
    phone: string;
    departmentId: number;
    departmentName: string;
    userCode: string;

    get fullName () {
      return `${this.firstName} ${this.surName}`.trim();
    }
}
