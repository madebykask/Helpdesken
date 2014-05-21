namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class RegistratedCasesCaseTypeReportModel
    {
        public RegistratedCasesCaseTypeReportModel()
        {
            this.WorkingGroups = new ItemOverview[] { };
            this.CaseTypes = new ItemOverview[] { };
        }

        public RegistratedCasesCaseTypeReportModel(
                        string key, 
                        IEnumerable<ItemOverview> workingGroups, 
                        IEnumerable<ItemOverview> caseTypes, 
                        ProductArea productArea, 
                        DateTime periodFrom, 
                        DateTime periodUntil)
        {
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.ProductArea = productArea;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Key = key;
        }

        [NotNullAndEmpty]
        public string Key { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ProductArea ProductArea { get; private set; }

        public DateTime PeriodFrom { get; private set; }

        public DateTime PeriodUntil { get; private set; }
    }
}