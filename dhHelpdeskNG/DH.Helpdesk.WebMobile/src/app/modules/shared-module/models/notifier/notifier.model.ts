
export class NotifierModel {  
  constructor(){    
  }

  id:number;
  userId: string;
  name: string; 
  email: string; 
  place: string; 
  phone: string; 
  userCode : string; 
  cellphone: string; 
  regionId?: number; 
  regionName: string; 
  departmentId?: number; 
  departmentName: string; 
  ouId?: number; 
  ouName : string; 
  costCentre: string; 
}


export class NotifierSearchItem {
    constructor() {
    }
    
    id:string;
    userId:string;
    name: string;
    email: string;
}