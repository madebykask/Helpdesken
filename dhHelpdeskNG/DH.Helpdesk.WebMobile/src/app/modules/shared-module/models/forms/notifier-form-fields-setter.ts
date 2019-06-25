import { CaseFieldsNames } from '../../constants';
import { CaseFormGroup } from './case-form-group';

export class NotifierFormFieldsSetter {
  constructor(
    private isRegarding: boolean, private form: CaseFormGroup) {
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
    this.setSafe(CaseFieldsNames.ReportedBy, val || '', false);
  }
  setPersonName(val: string) {
    this.setSafe(CaseFieldsNames.PersonName, val || '', false);
  }
  setPersonEmail(val: string) {
    this.setSafe(CaseFieldsNames.PersonEmail, val || '', false);
  }
  setPersonPhone(val: string) {
    this.setSafe(CaseFieldsNames.PersonPhone, val || '', false);
  }
  setPersonCellPhone(val: string) {
    this.setSafe(CaseFieldsNames.PersonCellPhone, val || '', false);
  }
  setPlace(val: string) {
    this.setSafe(CaseFieldsNames.Place, val || '', false);
  }
  setUserCode(val: string) {
    this.setSafe(CaseFieldsNames.UserCode, val || '', false);
  }
  setCostCenter(val: string) {
    this.setSafe(CaseFieldsNames.CostCentre, val || '', false);
  }
  private setSafe(fieldNameBase: string, val: any, emitNotification: boolean = true) {
    const fieldName = this.isRegarding ? 'IsAbout_' + fieldNameBase : fieldNameBase;
    emitNotification ? this.form.setValueWithNotification(fieldName, val) : this.form.setSafe(fieldName, val);
  }
}
