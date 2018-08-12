using DH.Helpdesk.Common.Enums.Settings;
using static DH.Helpdesk.BusinessData.OldComponents.GlobalEnums;

namespace DH.Helpdesk.BusinessData.OldComponents
{
    public class GlobalEnums
    {

        public enum CaseIcon
        {
            Normal = 0,
            Urgent = 1,
            Finished = 2,
            FinishedNotApproved = 3,
			Locked = 4
        }

        public enum MailTemplates
        {
            NewCase = 1,
            AssignedCaseToUser = 2,
            ClosedCase = 3, 
            InformNotifier = 4,
            InternalLogNote = 5,
            Survey = 6,
            AssignedCaseToWorkinggroup = 7,
            Order = 8,
            WatchdateOccurs = 9,
            CaseIsUpdated = 10,
            SmsAssignedCaseToUser = 11,
            PlannedDateOccurs = 12,
            AssignedCaseToPriority = 13,
            SmsClosedCase = 14,
            CaseIsActivated = 15,
            OperationLog = 60,
        }

        public enum TranslationCaseFields
        {
            None = -1,
            AgreedDate = 0,
            Available = 1,
            Caption = 2,
            CaseNumber = 3,
            CaseResponsibleUser_Id = 4,
            CaseType_Id = 5,
            Category_Id = 6,
            ChangeTime = 7,
            ComputerType_Id = 8,
            ContactBeforeAction = 9,
            Cost = 10,
            Customer_Id = 11,
            Department_Id = 12,
            Description = 13,
            Filename = 14,
            FinishingDate = 15,
            FinishingDescription = 16,
            Impact_Id = 17,
            InventoryLocation = 18,
            InventoryNumber = 19,
            InvoiceNumber = 20,
            Miscellaneous = 21,
            OU_Id = 22,
            Performer_User_Id = 23,
            Persons_CellPhone = 24,
            Persons_EMail = 25,
            Persons_Name = 26,
            Persons_Phone = 27,
            Place = 28,
            PlanDate = 29,
            Priority_Id = 30,
            ProductArea_Id = 31,
            ReferenceNumber = 32,
            Region_Id = 33,
            RegTime = 34,
            ReportedBy = 35,
            SMS = 36,
            SolutionRate = 37,
            StateSecondary_Id = 38,
            Status_Id = 39,
            Supplier_Id = 40,
            System_Id = 41,
            tblLog_Charge = 42,
            tblLog_Filename = 43,
            tblLog_Text_External = 44,
            tblLog_Text_Internal = 45,
            Urgency_Id = 46,
            User_Id = 47,
            UserCode = 48,
            WatchDate = 49,
            Verified = 50,
            VerifiedDescription = 51,
            WorkingGroup_Id = 52,
            _temporary_LeadTime = 53,
            CausingPart = 54,
            ClosingReason = 55,
            UpdateNotifierInformation = 56,
            Change = 57, 
            Project = 58,
            Problem = 59,
            RegistrationSourceCustomer = 60,
            AddUserBtn = 61,
            CostCentre = 62,
            IsAbout_ReportedBy = 63,
            IsAbout_Persons_Name = 64,
            IsAbout_Persons_EMail = 65,
            IsAbout_Persons_Phone = 66,
            IsAbout_Persons_CellPhone = 67,
            //IsAbout_Customer_Id = 68,
            IsAbout_Region_Id = 69,
            IsAbout_Department_Id = 70,
            IsAbout_OU_Id = 71,
            IsAbout_CostCentre = 72,
            IsAbout_Place = 73,
            IsAbout_UserCode = 74,
            MailToNotifier = 75,
            AddFollowersBtn = 76,
            UserSearchCategory_Id = 77,
            IsAbout_UserSearchCategory_Id = 78
        }
    }

    public static class TemplateExtenstion
    {

        public static TranslationCaseFields MapToCaseField(this CaseSolutionFields it)
        {
            switch (it)
            {
                case CaseSolutionFields.AddFollowersBtn:
                    return TranslationCaseFields.AddFollowersBtn;

                case CaseSolutionFields.AddUserBtn:
                    return TranslationCaseFields.AddUserBtn;

                case CaseSolutionFields.Administrator:
                    return TranslationCaseFields.Performer_User_Id;

                case CaseSolutionFields.Available:
                    return TranslationCaseFields.Available;

                case CaseSolutionFields.AgreedDate:
                    return TranslationCaseFields.AgreedDate;

                case CaseSolutionFields.Caption:
                    return TranslationCaseFields.Caption;

                case CaseSolutionFields.CaseType:
                    return TranslationCaseFields.CaseType_Id;

                case CaseSolutionFields.Category:
                    return TranslationCaseFields.Category_Id;

                case CaseSolutionFields.CausingPart:
                    return TranslationCaseFields.CausingPart;

                case CaseSolutionFields.Change:
                    return TranslationCaseFields.Change;

                case CaseSolutionFields.ContactBeforeAction:
                    return TranslationCaseFields.ContactBeforeAction;

                case CaseSolutionFields.Cost:
                    return TranslationCaseFields.Cost;

                case CaseSolutionFields.CostCentre:
                    return TranslationCaseFields.CostCentre;

                case CaseSolutionFields.Department:
                    return TranslationCaseFields.Department_Id;

                case CaseSolutionFields.Description:
                    return TranslationCaseFields.Description;

                case CaseSolutionFields.Email:
                    return TranslationCaseFields.MailToNotifier;

                case CaseSolutionFields.ExternalLogNote:
                    return TranslationCaseFields.tblLog_Text_External;

                case CaseSolutionFields.FileName:
                    return TranslationCaseFields.Filename;

                case CaseSolutionFields.FinishingCause:
                    return TranslationCaseFields.ClosingReason;

                case CaseSolutionFields.FinishingDate:
                    return TranslationCaseFields.FinishingDate;

                case CaseSolutionFields.FinishingDescription:
                    return TranslationCaseFields.FinishingDescription;

                case CaseSolutionFields.Impact:
                    return TranslationCaseFields.Impact_Id;

                case CaseSolutionFields.InternalLogNote:
                    return TranslationCaseFields.tblLog_Text_Internal;

                case CaseSolutionFields.InventoryLocation:
                    return TranslationCaseFields.InventoryLocation;

                case CaseSolutionFields.InventoryNumber:
                    return TranslationCaseFields.InventoryNumber;

                case CaseSolutionFields.InventoryType:
                    return TranslationCaseFields.ComputerType_Id;

                case CaseSolutionFields.InvoiceNumber:
                    return TranslationCaseFields.InvoiceNumber;

                case CaseSolutionFields.IsAbout_CostCentre:
                    return TranslationCaseFields.IsAbout_CostCentre;

                case CaseSolutionFields.IsAbout_Department_Id:
                    return TranslationCaseFields.IsAbout_Department_Id;

                case CaseSolutionFields.IsAbout_OU_Id:
                    return TranslationCaseFields.IsAbout_OU_Id;

                case CaseSolutionFields.IsAbout_PersonsCellPhone:
                    return TranslationCaseFields.IsAbout_Persons_CellPhone;

                case CaseSolutionFields.IsAbout_PersonsEmail:
                    return TranslationCaseFields.IsAbout_Persons_EMail;

                case CaseSolutionFields.IsAbout_PersonsName:
                    return TranslationCaseFields.IsAbout_Persons_Name;

                case CaseSolutionFields.IsAbout_PersonsPhone:
                    return TranslationCaseFields.IsAbout_Persons_Phone;

                case CaseSolutionFields.IsAbout_Place:
                    return TranslationCaseFields.IsAbout_Place;

                case CaseSolutionFields.IsAbout_Region_Id:
                    return TranslationCaseFields.IsAbout_Region_Id;

                case CaseSolutionFields.IsAbout_ReportedBy:
                    return TranslationCaseFields.IsAbout_ReportedBy;

                case CaseSolutionFields.IsAbout_UserCode:
                    return TranslationCaseFields.IsAbout_UserCode;

                case CaseSolutionFields.LogFileName:
                    return TranslationCaseFields.tblLog_Filename;

                case CaseSolutionFields.Miscellaneous:
                    return TranslationCaseFields.Miscellaneous;

                case CaseSolutionFields.OU:
                    return TranslationCaseFields.OU_Id;

                case CaseSolutionFields.PersonsCellPhone:
                    return TranslationCaseFields.Persons_CellPhone;

                case CaseSolutionFields.PersonsEmail:
                    return TranslationCaseFields.Persons_EMail;

                case CaseSolutionFields.PersonsName:
                    return TranslationCaseFields.Persons_Name;

                case CaseSolutionFields.PersonsPhone:
                    return TranslationCaseFields.Persons_Phone;

                case CaseSolutionFields.Place:
                    return TranslationCaseFields.Place;

                case CaseSolutionFields.PlanDate:
                    return TranslationCaseFields.PlanDate;

                case CaseSolutionFields.Priority:
                    return TranslationCaseFields.Priority_Id;

                case CaseSolutionFields.Problem:
                    return TranslationCaseFields.Problem;

                case CaseSolutionFields.ProductArea:
                    return TranslationCaseFields.ProductArea_Id;

                case CaseSolutionFields.Project:
                    return TranslationCaseFields.Project;

                case CaseSolutionFields.ReferenceNumber:
                    return TranslationCaseFields.ReferenceNumber;

                case CaseSolutionFields.Region:
                    return TranslationCaseFields.Region_Id;

                case CaseSolutionFields.RegistrationSourceCustomer:
                    return TranslationCaseFields.RegistrationSourceCustomer;

                case CaseSolutionFields.SMS:
                    return TranslationCaseFields.SMS;

                case CaseSolutionFields.SolutionRate:
                    return TranslationCaseFields.SolutionRate;

                case CaseSolutionFields.StateSecondary:
                    return TranslationCaseFields.StateSecondary_Id;

                case CaseSolutionFields.Status:
                    return TranslationCaseFields.Status_Id;

                case CaseSolutionFields.Supplier:
                    return TranslationCaseFields.Supplier_Id;

                case CaseSolutionFields.System:
                    return TranslationCaseFields.System_Id;

                case CaseSolutionFields.UpdateNotifierInformation:
                    return TranslationCaseFields.UpdateNotifierInformation;

                case CaseSolutionFields.Urgency:
                    return TranslationCaseFields.Urgency_Id;

                case CaseSolutionFields.Usercode:
                    return TranslationCaseFields.UserCode;

                case CaseSolutionFields.UserNumber:
                    return TranslationCaseFields.ReportedBy;

                case CaseSolutionFields.Verified:
                    return TranslationCaseFields.Verified;

                case CaseSolutionFields.VerifiedDescription:
                    return TranslationCaseFields.VerifiedDescription;

                case CaseSolutionFields.WatchDate:
                    return TranslationCaseFields.WatchDate;

                case CaseSolutionFields.WorkingGroup:
                    return TranslationCaseFields.WorkingGroup_Id;
            }

            return TranslationCaseFields.None;
        }
    }
}
