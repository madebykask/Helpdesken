import { CaseFieldsNames } from '../../constants';
import { CaseFormGroup } from './case-form-group';

export class NotifierFormFieldsSetter {
  constructor(private isRegarding: boolean, private form: CaseFormGroup) {
  }
  setRegion(val) {
    this.setSafe(CaseFieldsNames.RegionId, val);
  }
  setDepartment(val) {
    this.setSafe(CaseFieldsNames.DepartmentId, val);
  }
  setOU(val) {
    this.setSafe(CaseFieldsNames.OrganizationUnitId, val);
  }
  setReportedBy(val: string) {
    this.setSafe(CaseFieldsNames.ReportedBy, val || '');
  }
  setPersonName(val: string) {
    this.setSafe(CaseFieldsNames.PersonName, val || '');
  }
  setPersonEmail(val: string) {
    this.setSafe(CaseFieldsNames.PersonEmail, val || '');
  }
  setPersonPhone(val: string) {
    this.setSafe(CaseFieldsNames.PersonPhone, val || '');
  }
  setPersonCellPhone(val: string) {
    this.setSafe(CaseFieldsNames.PersonCellPhone, val || '');
  }
  setPlace(val: string) {
    this.setSafe(CaseFieldsNames.Place, val || '');
  }
  setUserCode(val: string) {
    this.setSafe(CaseFieldsNames.UserCode, val || '');
  }
  setCostCenter(val: string) {
    this.setSafe(CaseFieldsNames.CostCentre, val || '');
  }
  private setSafe(fieldNameBase: string, val: any) {
    const fieldName = this.isRegarding ? 'IsAbout_' + fieldNameBase : fieldNameBase;
    this.form.setSafe(fieldName, val);
  }
}
