export class CaseFileModel {
  constructor(public fileId: number, public fileName: string) {
  }
}

export class ExCaseFileModel {
  constructor(public Id: number, public FileName: string) { // Use in CamelCase for legacy compablity with helpdesk and selfservice
  }
}
