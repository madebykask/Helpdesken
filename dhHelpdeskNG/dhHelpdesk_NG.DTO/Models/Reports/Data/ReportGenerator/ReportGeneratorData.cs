namespace DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview;
    using DH.Helpdesk.Common.ValidationAttributes;
    using System;
    using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;

    public sealed class ReportGeneratorFields
    {
        public ReportGeneratorFields()
        {
        }

        public int Id { get; set; }

        public string User { get; set; }

        public string Notifier { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Customer { get; set; }

        public string Region { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }

        public string Place { get; set; }

        public string OrdererCode { get; set; }

        public string CostCentre { get; set; }

        //--

        public string PcNumber { get; set; }

        public string ComputerType { get; set; }

        public string ComputerPlace { get; set; }

        //--

        public decimal? Case { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime ChangeDate { get; set; }

        public string RegistratedBy { get; set; }

        public int CaseType { get; set; }

        public string ProductArea { get; set; }

        public string System { get; set; }

        public string UrgentDegree { get; set; }

        public string Impact { get; set; }

        public string Category { get; set; }

        public string Supplier { get; set; }

        public string InvoiceNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public string Other { get; set; }

        public int PhoneContact { get; set; }

        public int Sms { get; set; }

        public DateTime? AgreedDate { get; set; }

        public string Available { get; set; }

        public int Cost { get; set; }

        public string AttachedFile { get; set; }

        public string RegistrationSource { get; set; }

        public int? SolvedInTime { get; set; }

        public DateTime? FinishingDate { get; set; }

        public string FinishingDescription { get; set; }

        //--

        public string WorkingGroup { get; set; }

        public string Responsible { get; set; }

        public string Administrator { get; set; }

        public string Priority { get; set; }

        public string State { get; set; }

        public string SubState { get; set; }

        public DateTime? PlannedActionDate { get; set; }

        public DateTime? WatchDate { get; set; }

        public int Verified { get; set; }

        public string VerifiedDescription { get; set; }

        public string SolutionRate { get; set; }

        public string CausingPart { get; set; }

        public string IsAbout_User { get; set; }

        public string IsAbout_Persons_Name { get; set; }

        public string IsAbout_Persons_Phone { get; set; }

        public string IsAbout_Persons_CellPhone { get; set; }

        public string IsAbout_Persons_Email { get; set; }

        public string IsAbout_Department { get; set; }

        public string IsAbout_Region { get; set; }

        public string IsAbout_OU { get; set; }

        public string IsAbout_CostCentre { get; set; }

        public string IsAbout_Place { get; set; }

        public string IsAbout_UserCode { get; set; }
        //--

        public Log LogData { get; set; }

        public List<string> AllInternalText { get; set; }

        public List<string> AllExtenalText { get; set; }

        public int TotalMaterial { get; set; }
        public int TotalOverTime { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalWork { get; set; }

    }

    public sealed class ReportGeneratorData
    {
        public ReportGeneratorData(
            FullCaseSettings settings, 
            List<FullCaseOverview> cases)
        {
            this.Cases = cases;
            this.Settings = settings;
        }

        [NotNull]
        public FullCaseSettings Settings { get; private set; }

        [NotNull]
        public List<FullCaseOverview> Cases { get; private set; }
    }
}