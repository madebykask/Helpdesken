import { MailToTicketInfo, CaseSolution, CaseAccessMode, CaseEditInputModel, CaseFieldModel, IKeyValue } from '..';
import { DateUtil } from 'src/app/modules/shared-module/utils/date-util';
import { CaseHistoryChangeModel, CaseLogModel, LogFile, CaseHistoryModel, Mail2Ticket } from './case-actions-api.model';

export class CaseModelBuilder {

  // TODO: review - not all cases covered
  createCaseEditInputModel(json: any): CaseEditInputModel {
    if (typeof json === 'string') {
      json = JSON.parse(json);
    }

    const fields = json.fields as any[] || new Array();
    const caseSolution = json.caseSolution ? <CaseSolution>json.caseSolution : null;
    const mailToTickets: MailToTicketInfo = json.mailToTickets ? <MailToTicketInfo>json.mailToTickets : null;
    const editMode = <CaseAccessMode>json.editMode;

    return Object.assign(new CaseEditInputModel(), json, {
      editMode: editMode,
      caseSolution: caseSolution,
      mailToTickets: mailToTickets,
      fields: fields.map(v => {
        let field = null;
        switch (v.JsonType) {
          case 'string':
            field = this.createBaseCaseField<string>(v);
            break;
          case 'date':
            field = this.createBaseCaseField<string>(v);
            break;
          case 'number':
            field = this.createBaseCaseField<number>(v);
            break;
          case 'array':
            field = this.createBaseCaseField<Array<any>>(v);
            break;
          default:
            field = this.createBaseCaseField<any>(v);
            break;
        }
        return field;
      })
    });
  }

  createCaseHistoryModel(json): CaseHistoryModel {
    if (json === null) { return null; }
    const model = Object.assign(new CaseHistoryModel(), {
      emailLogs: json.emailLog || [],
      changes: json.changes && json.changes.length
        ? json.changes.map(x => this.createCaseHistoryChangeModel(x))
        : []
    });
    return model;
  }

  createCaseLogModel(json: any): CaseLogModel {
    if (json === null) { return null; }
    const model = Object.assign(new CaseLogModel(), json, {
      createdAt: new Date(json.createdAt),
      files: json.files && json.files.length
        ? json.files.map(f => Object.assign(new LogFile(), f))
        : [],
      mail2Tickets: json.mail2Tickets && json.mail2Tickets.length
      ? json.mail2Tickets.map(m => Object.assign(new Mail2Ticket(), m))
      : [],
    });
    return model;
  }

  createCaseHistoryChangeModel(json) {
    return Object.assign(new CaseHistoryChangeModel(), json, {
      // todo:review
      createdAt: new Date(json.createdAt),
      previousValue: this.tryConvertToDate(json.previousValue),
      currentValue: this.tryConvertToDate(json.currentValue)
    });
  }

  private tryConvertToDate(value: any) {
    // try convert field value to date
    const val = DateUtil.tryConvertToDate(value);
    return val;
  }

  private createBaseCaseField<T>(json: any): CaseFieldModel<T> {
    if (typeof json === 'string') {
      json = JSON.parse(json);
    }
    const options = json.options as any[] || new Array();
    return Object.assign(new CaseFieldModel<T>(), json, {
      value: json.value,
      options: options.map(v => this.createKeyValue(v))
    });
  }

  private createKeyValue(json: any): IKeyValue {
    if (typeof json === 'string') { json = JSON.parse(json); }
    return <IKeyValue>{ ...json };
  }

}
