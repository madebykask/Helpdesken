using DH.Helpdesk.Web.Common.Constants.Case;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DH.Helpdesk.BusinessData.OldComponents.GlobalEnums;

namespace DH.Helpdesk.Web.Common.Extensions
{
    public static class CaseFieldsNamesApiExtensions
    {
        public static string MapToCaseFieldsNamesApi(this TranslationCaseFields it)
        {
            const string defaultValue = null;
            switch (it)
            {
                case TranslationCaseFields.Performer_User_Id:
                    return CaseFieldsNamesApi.PerformerUserId;

                case TranslationCaseFields.Available:
                    return CaseFieldsNamesApi.Available;

                case TranslationCaseFields.AgreedDate:
                    return CaseFieldsNamesApi.AgreedDate;

                case TranslationCaseFields.Caption:
                    return CaseFieldsNamesApi.Caption;

                case TranslationCaseFields.CaseType_Id:
                    return CaseFieldsNamesApi.CaseTypeId;

                case TranslationCaseFields.Category_Id:
                    return CaseFieldsNamesApi.CategoryId;

                case TranslationCaseFields.CausingPart:
                    return CaseFieldsNamesApi.CausingPart;

                case TranslationCaseFields.Change:
                    return CaseFieldsNamesApi.Change;

                case TranslationCaseFields.ContactBeforeAction:
                    return CaseFieldsNamesApi.ContactBeforeAction;

                case TranslationCaseFields.Cost:
                    return CaseFieldsNamesApi.Cost;

                case TranslationCaseFields.CostCentre:
                    return CaseFieldsNamesApi.CostCentre;

                case TranslationCaseFields.Department_Id:
                    return CaseFieldsNamesApi.DepartmentId;

                case TranslationCaseFields.Description:
                    return CaseFieldsNamesApi.Description;

                case TranslationCaseFields.tblLog_Text_External:
                    return CaseFieldsNamesApi.Log_ExternalText;

                case TranslationCaseFields.Filename:
                    return CaseFieldsNamesApi.Filename;

                case TranslationCaseFields.ClosingReason:
                    return CaseFieldsNamesApi.ClosingReason;

                case TranslationCaseFields.FinishingDate:
                    return CaseFieldsNamesApi.FinishingDate;

                case TranslationCaseFields.FinishingDescription:
                    return CaseFieldsNamesApi.FinishingDescription;

                case TranslationCaseFields.Impact_Id:
                    return CaseFieldsNamesApi.ImpactId;

                case TranslationCaseFields.tblLog_Text_Internal:
                    return CaseFieldsNamesApi.Log_InternalText;

                case TranslationCaseFields.InventoryLocation:
                    return CaseFieldsNamesApi.InventoryLocation;

                case TranslationCaseFields.InventoryNumber:
                    return CaseFieldsNamesApi.InventoryNumber;

                case TranslationCaseFields.ComputerType_Id:
                    return CaseFieldsNamesApi.ComputerTypeId;

                case TranslationCaseFields.InvoiceNumber:
                    return CaseFieldsNamesApi.InvoiceNumber;

                case TranslationCaseFields.IsAbout_CostCentre:
                    return CaseFieldsNamesApi.IsAbout_CostCentre;

                case TranslationCaseFields.IsAbout_Department_Id:
                    return CaseFieldsNamesApi.IsAbout_DepartmentId;

                case TranslationCaseFields.IsAbout_OU_Id:
                    return CaseFieldsNamesApi.IsAbout_OrganizationUnitId;

                case TranslationCaseFields.IsAbout_Persons_CellPhone:
                    return CaseFieldsNamesApi.IsAbout_PersonCellPhone;

                case TranslationCaseFields.IsAbout_Persons_EMail:
                    return CaseFieldsNamesApi.IsAbout_PersonEmail;

                case TranslationCaseFields.IsAbout_Persons_Name:
                    return CaseFieldsNamesApi.IsAbout_PersonName;

                case TranslationCaseFields.IsAbout_Persons_Phone:
                    return CaseFieldsNamesApi.IsAbout_PersonPhone;

                case TranslationCaseFields.IsAbout_Place:
                    return CaseFieldsNamesApi.IsAbout_Place;

                case TranslationCaseFields.IsAbout_Region_Id:
                    return CaseFieldsNamesApi.IsAbout_RegionId;

                case TranslationCaseFields.IsAbout_ReportedBy:
                    return CaseFieldsNamesApi.IsAbout_ReportedBy;

                case TranslationCaseFields.IsAbout_UserCode:
                    return CaseFieldsNamesApi.IsAbout_UserCode;

                case TranslationCaseFields.tblLog_Filename:
                    return CaseFieldsNamesApi.Log_Filename;

                case TranslationCaseFields.tblLog_Filename_Internal:
                    return CaseFieldsNamesApi.Log_Filename_Internal;

                case TranslationCaseFields.Miscellaneous:
                    return CaseFieldsNamesApi.Miscellaneous;

                case TranslationCaseFields.OU_Id:
                    return CaseFieldsNamesApi.OrganizationUnitId;

                case TranslationCaseFields.Persons_CellPhone:
                    return CaseFieldsNamesApi.PersonCellPhone;

                case TranslationCaseFields.Persons_EMail:
                    return CaseFieldsNamesApi.PersonEmail;

                case TranslationCaseFields.Persons_Name:
                    return CaseFieldsNamesApi.PersonName;

                case TranslationCaseFields.Persons_Phone:
                    return CaseFieldsNamesApi.PersonPhone;

                case TranslationCaseFields.Place:
                    return CaseFieldsNamesApi.Place;

                case TranslationCaseFields.PlanDate:
                    return CaseFieldsNamesApi.PlanDate;

                case TranslationCaseFields.Priority_Id:
                    return CaseFieldsNamesApi.PriorityId;

                case TranslationCaseFields.Problem:
                    return CaseFieldsNamesApi.Problem;

                case TranslationCaseFields.ProductArea_Id:
                    return CaseFieldsNamesApi.ProductAreaId;

                case TranslationCaseFields.Project:
                    return CaseFieldsNamesApi.Project;

                case TranslationCaseFields.ReferenceNumber:
                    return CaseFieldsNamesApi.ReferenceNumber;

                case TranslationCaseFields.Region_Id:
                    return CaseFieldsNamesApi.RegionId;

                case TranslationCaseFields.RegistrationSourceCustomer:
                    return CaseFieldsNamesApi.RegistrationSourceCustomer;

                case TranslationCaseFields.SMS:
                    return CaseFieldsNamesApi.Sms;

                case TranslationCaseFields.SolutionRate:
                    return CaseFieldsNamesApi.SolutionRate;

                case TranslationCaseFields.StateSecondary_Id:
                    return CaseFieldsNamesApi.StateSecondaryId;

                case TranslationCaseFields.Status_Id:
                    return CaseFieldsNamesApi.StatusId;

                case TranslationCaseFields.Supplier_Id:
                    return CaseFieldsNamesApi.SupplierId;

                case TranslationCaseFields.System_Id:
                    return CaseFieldsNamesApi.SystemId;

                case TranslationCaseFields.Urgency_Id:
                    return CaseFieldsNamesApi.UrgencyId;

                case TranslationCaseFields.UserCode:
                    return CaseFieldsNamesApi.UserCode;

                case TranslationCaseFields.ReportedBy:
                    return CaseFieldsNamesApi.ReportedBy;

                case TranslationCaseFields.Verified:
                    return CaseFieldsNamesApi.Verified;

                case TranslationCaseFields.VerifiedDescription:
                    return CaseFieldsNamesApi.VerifiedDescription;

                case TranslationCaseFields.WatchDate:
                    return CaseFieldsNamesApi.WatchDate;

                case TranslationCaseFields.WorkingGroup_Id:
                    return CaseFieldsNamesApi.WorkingGroupId;

                case TranslationCaseFields.CaseNumber:
                    return CaseFieldsNamesApi.CaseNumber;

                case TranslationCaseFields.CaseResponsibleUser_Id:
                    return CaseFieldsNamesApi.CaseResponsibleUserId;

                case TranslationCaseFields.ChangeTime:
                    return CaseFieldsNamesApi.ChangeTime;

                case TranslationCaseFields.Customer_Id:
                    return CaseFieldsNamesApi.CustomerId;

                case TranslationCaseFields.RegTime:
                    return CaseFieldsNamesApi.RegTime;

                case TranslationCaseFields.User_Id:
                    return CaseFieldsNamesApi.UserId;

                case TranslationCaseFields.UserSearchCategory_Id:
                    return CaseFieldsNamesApi.UserSearchCategoryId;

                case TranslationCaseFields.IsAbout_UserSearchCategory_Id:
                    return CaseFieldsNamesApi.IsAbout_UserSearchCategoryId;
            }

            return defaultValue;
        }
    }
}
