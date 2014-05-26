namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class RegistratedCasesCaseTypeReportModel
    {
        public RegistratedCasesCaseTypeReportModel(string objectId, string fileName)
        {
            this.FileName = fileName;
            this.ObjectId = objectId;
            this.WorkingGroups = new ItemOverview[] { };
            this.CaseTypes = new ItemOverview[] { };
        }

        public RegistratedCasesCaseTypeReportModel(
                        IEnumerable<ItemOverview> workingGroups, 
                        IEnumerable<ItemOverview> caseTypes, 
                        ProductArea productArea, 
                        DateTime periodFrom, 
                        DateTime periodUntil, 
                        string objectId, 
                        string fileName)
        {
            this.FileName = fileName;
            this.ObjectId = objectId;
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.ProductArea = productArea;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
        }

        public string ObjectId { get; private set; }

        [NotNullAndEmpty]
        public string FileName { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ProductArea ProductArea { get; private set; }

        public DateTime PeriodFrom { get; private set; }

        public DateTime PeriodUntil { get; private set; }
    }
}