import { Input } from "@angular/core";
import { CaseAction, CaseActionDataType, CaseLogActionData, CaseHistoryActionData, CaseActionEvents } from "../../../models";

export class CaseActionBaseComponent<TActionData extends CaseActionDataType> {

  @Input() caseAction: CaseAction<TActionData>;

  getActionIcon() {

    let iconClass = "action-icon mbsc-ic mbsc-ic-";

    switch (this.caseAction.Type) {

      case CaseActionEvents.ExternalLogNote:
        return iconClass + "fa-comment-o";

      case CaseActionEvents.InternalLogNote:
        return iconClass + "fa-comment";

      case CaseActionEvents.ClosedCase:
        return iconClass + "fa-check-square-o";

      case CaseActionEvents.AssignedAdministrator:
        return iconClass + "fa-user";

      case CaseActionEvents.AssignedWorkingGroup:
        return iconClass + "fa-group";

      case CaseActionEvents.ChangeSubstatus:
        return iconClass + "fa-exchange";

      case CaseActionEvents.ChangePriority:
        return iconClass + "fa-exclamation";

      case CaseActionEvents.ChangeWatchDate:
        return iconClass + "fa-clock-o";

      case CaseActionEvents.UploadLogFile:
        return iconClass + "fa-paperclip";

      case CaseActionEvents.SentEmails:
        return iconClass + "fa-envelope-o";

      case CaseActionEvents.OtherChanges:
        return iconClass + "fa-edit";

      default:
        return iconClass + "fa-edit";      
    }
  }
}