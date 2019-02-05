import { Input } from "@angular/core";
import { CaseAction, CaseActionDataType, CaseLogActionData, CaseHistoryActionData, CaseEventType } from "../../../models";

export class CaseActionBaseComponent<TActionData extends CaseActionDataType> {

  @Input() caseKey: string;
  @Input() caseAction: CaseAction<TActionData>;

  getActionIcon() {

    let iconClass = "action-icon mbsc-ic mbsc-ic-";

    switch (this.caseAction.type) {

      case CaseEventType.ExternalLogNote:
        return iconClass + "fa-comment-o";

      case CaseEventType.InternalLogNote:
        return iconClass + "fa-comment";

      case CaseEventType.ClosedCase:
        return iconClass + "fa-check-square-o";

      case CaseEventType.AssignedAdministrator:
        return iconClass + "fa-user";

      case CaseEventType.AssignedWorkingGroup:
        return iconClass + "fa-group";

      case CaseEventType.ChangeSubstatus:
        return iconClass + "fa-exchange";

      case CaseEventType.ChangePriority:
        return iconClass + "fa-exclamation";

      case CaseEventType.ChangeWatchDate:
        return iconClass + "fa-clock-o";

      case CaseEventType.UploadLogFile:
        return iconClass + "fa-paperclip";

      case CaseEventType.SentEmails:
        return iconClass + "fa-envelope-o";

      case CaseEventType.OtherChanges:
        return iconClass + "fa-edit";

      default:
        return iconClass + "fa-edit";
    }
  }
}