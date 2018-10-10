using System;
using System.Collections.Generic;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Infrastructure.Case
{
    public static class CaseHistoryBuilder
    {
        const string bs = "<th>";
        const string be = "</th>";
        const string ey = "";
        const string from = " &rarr; ";
        const string tdOpenMarkup = "<td style=\"width:70%\">";
        const string tdCloseMarkup = "</td>";

        public static string GetCaseHistoryInfo(
            CaseHistoryOverview cur,
            CaseHistoryOverview old,
            int customerId,
            int departmentFilterFormat,
            IList<CaseFieldSetting> cfs,
            OutputFormatter outFormatter)
        {
            var sb = new StringBuilder();

            var prev = (old != null ? old : new CaseHistoryOverview());

            // Reported by
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ReportedBy.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ReportedBy != prev.ReportedBy)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ReportedBy.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.ReportedBy.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.ReportedBy.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons name
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_Name.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsName != prev.PersonsName)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_Name.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.PersonsName.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsName.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons e-mail
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsEmail != prev.PersonsEmail)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.PersonsEmail.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsEmail.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons_phone 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_Phone.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsPhone != prev.PersonsPhone)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.PersonsPhone.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsPhone.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons mobile no 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsCellphone != prev.PersonsCellphone)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.PersonsCellphone.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsCellphone.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Region
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Region_Id != prev.Region_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Region_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.Region != null)
                        sb.Append(prev.Region.Name.RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Region != null)
                        sb.Append(cur.Region.Name.RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // OU
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.OU_Id != prev.OU_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.OU_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.OU != null)
                        sb.Append(prev.OU.Name.RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.OU != null)
                        sb.Append(cur.OU.Name.RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Department
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Department_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Department_Id != prev.Department_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Department_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.Department != null)
                        sb.Append(prev.Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Department != null)
                        sb.Append(cur.Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // CostCentre 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CostCentre.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CostCentre != prev.CostCentre)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.CostCentre.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.CostCentre.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.CostCentre.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Placement 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Place.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Place != prev.Place)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Place.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.Place.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.Place.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // UserCode
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.UserCode.ToString()).ShowOnStartPage == 1)
            {
                if (cur.UserCode != prev.UserCode)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.UserCode.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.UserCode.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.UserCode.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }

            // Registration Source
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()).ShowOnStartPage == 1)
            {
                if (cur.RegistrationSourceCustomer_Id != prev.RegistrationSourceCustomer_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.RegistrationSourceCustomer != null)
                        sb.Append(Translation.Get(prev.RegistrationSourceCustomer.SourceName.RemoveHTMLTags(), Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.RegistrationSourceCustomer != null)
                        sb.Append(Translation.Get(cur.RegistrationSourceCustomer.SourceName.RemoveHTMLTags(), Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // CaseType
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseType_Id != prev.CaseType_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);

                    if (prev.CaseType != null)
                        sb.Append(Translation.Get(prev.CaseType.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);

                    sb.Append(from);

                    if (cur.CaseType != null)
                        sb.Append(Translation.Get(cur.CaseType.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            //Project
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Project.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Project_Id != prev.Project_Id)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.Project.ToString(), customerId);
                    var prevVal = prev.Project != null ? prev.Project.Name : null;
                    var curVal = cur.Project != null ? cur.Project.Name : null;
                    var s = FormatChanges(field,  prevVal, curVal);
                    sb.Append(s);
                }
            }

            //Problem
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Problem.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Problem_Id != prev.Problem_Id)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.Problem.ToString(), customerId);
                    var prevVal = prev.Problem?.Name;
                    var curVal = cur.Problem?.Name;
                    var s = FormatChanges(field, prevVal, curVal);
                    sb.Append(s);
                }
            }

            // Causing Part
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CausingPart.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CausingPartId != prev.CausingPartId)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.CausingPart.ToString(), customerId);
                    var prevVal = prev.CausingPart?.Name;
                    var curVal = cur.CausingPart?.Name;
                    var s = FormatChanges(field, Translation.GetCoreTextTranslation(prevVal), Translation.GetCoreTextTranslation(curVal));
                    sb.Append(s);
                }
            }

            // Planned action date (Planerad åtgärdsdatum)
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.PlanDate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PlanDate != prev.PlanDate)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.PlanDate.ToString(), customerId);
                    var s = FormatNullableDate(field, prev.PlanDate, cur.PlanDate, outFormatter);
                    sb.Append(s);
                }
            }

            // Verified (Verifierad)
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Verified.ToString()).ShowOnStartPage == 1)
            {
                var oldValue = prev.Verified.HasValue ? prev.Verified.ToBool().GetYesNoText() : "";
                var newValue = cur.Verified.HasValue ? cur.Verified.ToBool().GetYesNoText() : "";

                if (oldValue != newValue)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.Verified.ToString(), customerId);
                    var s = FormatChanges(field, oldValue, newValue);
                    sb.Append(s);
                }
            }

            // Verified description (Verifierad beskrivning)
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString()).ShowOnStartPage == 1)
            {
                if (cur.VerifiedDescription != prev.VerifiedDescription)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(), customerId);
                    var prevValue = prev.VerifiedDescription.RemoveHTMLTags();
                    var newValue = cur.VerifiedDescription.RemoveHTMLTags();
                    var s = FormatChanges(field, prevValue, newValue);
                    sb.Append(s);
                }
            }

            // Resolution rate (Lösningsgrad)
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.SolutionRate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.SolutionRate != prev.SolutionRate)
                {
                    var field = Translation.GetForCase(GlobalEnums.TranslationCaseFields.SolutionRate.ToString(), customerId);
                    var prevValue = prev.SolutionRate.RemoveHTMLTags();
                    var newValue = cur.SolutionRate.RemoveHTMLTags();
                    var s = FormatChanges(field, prevValue, newValue);
                    sb.Append(s);
                }
            }

            // ProductArea
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ProductArea_Id != prev.ProductArea_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.ProductArea != null)
                        sb.Append(Translation.Get(prev.ProductArea.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.ProductArea != null)
                        sb.Append(Translation.Get(cur.ProductArea.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Category
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Category_Id != prev.Category_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Category_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.Category != null)
                        sb.Append(Translation.Get(prev.Category.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Category != null)
                        sb.Append(Translation.Get(cur.Category.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // ReferenceNumber
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ReferenceNumber != prev.ReferenceNumber)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.ReferenceNumber.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.ReferenceNumber.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }
            // Caption
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Caption.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Caption != prev.Caption)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Caption.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.Caption.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.Caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup); //
                    sb.Append("</tr>");
                }
            }
            // Responsible User
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseResponsibleUser_Id != prev.CaseResponsibleUser_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.UserResponsible != null)
                        sb.Append(prev.UserResponsible.FirstName + " " + prev.UserResponsible.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.UserResponsible != null)
                        sb.Append(cur.UserResponsible.FirstName + " " + cur.UserResponsible.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Performer User
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Performer_User_Id != prev.Performer_User_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.UserPerformer != null)
                        sb.Append(prev.UserPerformer.FirstName + " " + prev.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.UserPerformer != null)
                        sb.Append(cur.UserPerformer.FirstName + " " + cur.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Priority
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Priority_Id != prev.Priority_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.Priority != null)
                        sb.Append(Translation.Get(prev.Priority.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Priority != null)
                        sb.Append(Translation.Get(cur.Priority.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // WorkingGroup
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.WorkingGroup_Id != prev.WorkingGroup_Id)
                {
                    //string value = cur.WorkingGroup != null ? cur.WorkingGroup.WorkingGroupName : ey + from + cur.WorkingGroup != null ? cur.WorkingGroup.WorkingGroupName : ey;
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.WorkingGroup != null)
                        sb.Append(Translation.Get(prev.WorkingGroup.WorkingGroupName, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.WorkingGroup != null)
                        sb.Append(Translation.Get(cur.WorkingGroup.WorkingGroupName, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // StateSecondary
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.StateSecondary_Id != prev.StateSecondary_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.StateSecondary != null)
                        sb.Append(Translation.Get(prev.StateSecondary.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.StateSecondary != null)
                        sb.Append(Translation.Get(cur.StateSecondary.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Status
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Status_Id != prev.Status_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.Status != null)
                        sb.Append(Translation.Get(prev.Status.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Status != null)
                        sb.Append(Translation.Get(cur.Status.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Watchdate
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WatchDate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.WatchDate != prev.WatchDate)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    if (prev.WatchDate == null)
                    {
                        sb.Append(tdOpenMarkup);
                        sb.Append(from);
                        sb.Append(outFormatter.FormatDate(cur.WatchDate.Value));
                    }
                    else
                    {
                        if (cur.WatchDate == null)
                        {
                            sb.Append(tdOpenMarkup);
                            sb.Append(outFormatter.FormatDate(prev.WatchDate.Value));
                            sb.Append(from);
                            sb.Append("");
                        }
                        else
                        {
                            sb.Append(tdOpenMarkup);
                            sb.Append(outFormatter.FormatDate(prev.WatchDate.Value));
                            sb.Append(from);
                            sb.Append(outFormatter.FormatDate(cur.WatchDate.Value));
                        }

                        sb.Append(tdCloseMarkup);
                        sb.Append("</tr>");
                    }
                }
            }
            // IsAbout user
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_ReportedBy != prev.IsAbout_ReportedBy)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.IsAbout_ReportedBy.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_ReportedBy.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            //IsAbout Persons name
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Persons_Name != prev.IsAbout_Persons_Name)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.IsAbout_Persons_Name.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_Persons_Name.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout Persons_phone 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Persons_Phone != prev.IsAbout_Persons_Phone)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.IsAbout_Persons_Phone.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_Persons_Phone.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout Department
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Department_Id != prev.IsAbout_Department_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.IsAbout_Department != null)
                        sb.Append(prev.IsAbout_Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.IsAbout_Department != null)
                        sb.Append(cur.IsAbout_Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout UserCode
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_UserCode != prev.IsAbout_UserCode)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.IsAbout_UserCode.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_UserCode.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }
            // CaseFile
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Filename.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseFile != prev.CaseFile && !string.IsNullOrEmpty(cur.CaseFile))
                {
                    sb.Append("<tr>");
                    var caption = string.Empty;
                    if (!string.IsNullOrEmpty(cur.CaseFile))
                    {
                        if (cur.CaseFile.StartsWith(StringTags.Add))
                        {
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            caption = cur.CaseFile.Substring(StringTags.Add.Length);
                        }
                        else
                        if (cur.CaseFile.StartsWith(StringTags.Delete))
                        {
                            sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            caption = cur.CaseFile.Substring(StringTags.Delete.Length);
                        }
                        else
                            caption = cur.CaseFile;
                    }
                    else
                        caption = ey;

                    sb.Append(tdOpenMarkup);
                    sb.Append(caption);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // LogFile
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString()).ShowOnStartPage == 1)
            {
                if (cur.LogFile != prev.LogFile && !string.IsNullOrEmpty(cur.LogFile))
                {
                    sb.Append("<tr>");
                    var caption = string.Empty;
                    if (!string.IsNullOrEmpty(cur.LogFile))
                    {
                        if (cur.LogFile.StartsWith(StringTags.Add))
                        {
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            caption = cur.LogFile.Substring(StringTags.Add.Length);
                        }
                        else
                        if (cur.LogFile.StartsWith(StringTags.Delete))
                        {
                            sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);

                            caption = cur.LogFile.Substring(StringTags.Delete.Length);
                        }
                        else
                        {
                            sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            caption = cur.LogFile;
                        }
                    }
                    else
                        caption = ey;

                    sb.Append(tdOpenMarkup);
                    sb.Append(caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }


            // CaseLog
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString()).ShowOnStartPage == 1 ||
                cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseLog != prev.CaseLog && !string.IsNullOrEmpty(cur.CaseLog))
                {
                    var caption = string.Empty;
                    sb.Append("<tr>");
                    if (!string.IsNullOrEmpty(cur.CaseLog))
                    {
                        if (cur.CaseLog.StartsWith(StringTags.Add))
                        {
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get("Ärendelogg") + be);
                            caption = cur.CaseLog.Substring(StringTags.Add.Length)
                                .Replace(StringTags.ExternalLog,
                                    Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                .Replace(StringTags.InternalLog, "<br />" +
                                                                 Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ");
                        }
                        else
                        if (cur.CaseLog.StartsWith(StringTags.Delete))
                        {
                            sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get("Ärendelogg") + be);
                            caption = cur.CaseLog.Substring(StringTags.Delete.Length)
                                .Replace(StringTags.ExternalLog,
                                    Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                .Replace(StringTags.InternalLog, "<br />" +
                                                                 Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                .Replace(StringTags.LogFile, "<br />" +
                                                             Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": <br /> &nbsp; - ")
                                .Replace(StringTags.Seperator, "<br /> &nbsp; - ");
                        }
                        else
                        {
                            sb.Append(bs + Translation.Get("Ärendelogg") + be);
                            caption = cur.CaseLog.Replace(StringTags.ExternalLog,
                                    Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                .Replace(StringTags.InternalLog, "<br />" +
                                                                 Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ");
                        }
                    }
                    else
                        caption = ey;

                    sb.Append(tdOpenMarkup);
                    sb.Append(caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // Closing Reason
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ClosingReason != prev.ClosingReason && cur.ClosingReason != null)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ClosingReason.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (prev.ClosingReason != null)
                        sb.Append(prev.ClosingReason);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.ClosingReason != null)
                        sb.Append(cur.ClosingReason);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // Closing Date
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.FinishingDate != prev.FinishingDate)
                {
                    sb.Append("<tr>");
                    if (prev.FinishingDate == null)
                    {
                        sb.Append(bs + Translation.Get("Ärendet avslutat") + be);
                        sb.Append(tdOpenMarkup);
                        sb.Append(from);
                        sb.Append(outFormatter.FormatDate(cur.FinishingDate.Value));
                    }
                    else
                    {
                        if (cur.FinishingDate == null)
                        {
                            sb.Append(bs + Translation.Get("Ärendet aktiverat") + be);
                            sb.Append(tdOpenMarkup);
                            sb.Append(ey);
                        }
                        else
                        {
                            sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.FinishingDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            sb.Append(tdOpenMarkup);
                            sb.Append(outFormatter.FormatDate(prev.FinishingDate.Value));
                            sb.Append(from);
                            sb.Append(outFormatter.FormatDate(cur.FinishingDate.Value));
                        }
                    }

                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // Case extra followers
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString()).ShowOnStartPage == 1)
            {
                if (!cur.CaseExtraFollowers.Equals(prev.CaseExtraFollowers))
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.GetCoreTextTranslation("Följare") + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(prev.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "));
                    sb.Append(from);
                    sb.Append(cur.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "));
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        private static string FormatNullableDate(string field, DateTime? oldValue, DateTime? newValue, OutputFormatter outFormatter)
        {
            var sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append(bs + field + be);

            if (oldValue == null)
            {
                sb.Append(tdOpenMarkup);
                sb.Append(from);
                sb.Append(outFormatter.FormatDate(newValue.Value));
            }
            else // oldValue != null
            {
                if (newValue == null)
                {
                    sb.Append(tdOpenMarkup);
                    sb.Append(outFormatter.FormatDate(oldValue.Value));
                    sb.Append(from);
                    sb.Append("");
                }
                else
                {
                    sb.Append(tdOpenMarkup);
                    sb.Append(outFormatter.FormatDate(oldValue.Value));
                    sb.Append(from);
                    sb.Append(outFormatter.FormatDate(newValue.Value));
                }

                sb.Append(tdCloseMarkup);
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        private static string FormatChanges(string field,  string oldValue, string newValue)
        {
            var sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append(bs + field + be);
            sb.Append(tdOpenMarkup);

            if (!string.IsNullOrEmpty(oldValue))
                sb.Append(oldValue);
            else
                sb.Append(ey);

            sb.Append(from);

            if (!string.IsNullOrEmpty(newValue))
                sb.Append(newValue);
            else
                sb.Append(ey);

            sb.Append(tdCloseMarkup);
            sb.Append("</tr>");

            return sb.ToString();
        }
    }
}