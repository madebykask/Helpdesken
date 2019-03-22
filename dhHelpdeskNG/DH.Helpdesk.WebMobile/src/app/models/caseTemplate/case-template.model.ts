export class CaseTemplateModel {
  public id:number;
  public name:string;  
  public categoryId:number;
  public categoryName:string;
}

export class CaseTemplateNode {
  constructor(public id: number,
              public name: string) {
  }
}

export class CaseTemplateCategoryNode {
  public items:CaseTemplateNode[] = [];

  constructor(public id: number,
              public name: string) {
  } 
}