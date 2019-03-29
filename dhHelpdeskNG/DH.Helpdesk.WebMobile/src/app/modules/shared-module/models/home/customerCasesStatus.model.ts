export class CustomerCasesStatusModel {
  constructor() {
  }
  
  customerId: number;
  customerName: string;
  myCases: number;
  inProgress: number;
  newToday: number;
  closedToday: number;
}