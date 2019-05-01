import { CaseUserSearchGroup } from '../../logic/constants/case-user-search-group';

export class EmailsSearchItem {
  constructor() { }
  userId: string;
  firstName: string;
  surName: string;
  emails: string[];
  departmentName: string;
  groupType: CaseUserSearchGroup;
}
