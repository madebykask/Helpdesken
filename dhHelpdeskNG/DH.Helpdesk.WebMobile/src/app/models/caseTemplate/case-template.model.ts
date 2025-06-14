
export class CustomerCaseTemplateModel {
  public customerId: number;
  public customerName: string;
  items: CaseTemplateModel[];
}

export class CaseTemplateModel {
  public id: number;
  public name: string;
  public categoryId: number;
  public categoryName: string;
}

export class CaseTemplateNode {
  constructor(public id: number,
              public name: string,
              public customerId: number,
              public disabled = false) {
  }
}

export class CaseTemplateCategoryNode {
  public items: CaseTemplateNode[] = [];

  constructor(public id: number,
              public name: string,
              public customerId: number,
              public disabled = false) {
  }
}
