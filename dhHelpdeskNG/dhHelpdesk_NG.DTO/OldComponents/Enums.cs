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
        }

        public enum RegistrationSource
        {
            Empty = 0,
            Case = 1,
            SelfService = 2,
            Mail = 3,
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
            Source = 60
        }
    }
}
