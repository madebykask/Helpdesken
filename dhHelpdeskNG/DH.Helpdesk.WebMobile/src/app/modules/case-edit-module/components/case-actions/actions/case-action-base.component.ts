import { Input } from "@angular/core";
import { CaseAction, CaseActionDataType, CaseLogActionData, CaseHistoryActionData } from "../../../models";

export class CaseActionBaseComponent<TActionData extends CaseActionDataType> {

  @Input() caseAction: CaseAction<TActionData>;

  getActionIcon() {
    const actionData = this.caseAction != null ? this.caseAction.Data : null;
    let iconClass = "action-icon mbsc-ic mbsc-ic-";
    if (actionData) {
        if (actionData instanceof CaseLogActionData) {
          return iconClass + "fa-comment-o";
        } else if (actionData instanceof CaseHistoryActionData) {
          return iconClass +"fa-history";
        } else {
          return iconClass + "fa-user-secret";
        }
    }
  }
}