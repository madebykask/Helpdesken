namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportModelWrapper
    {
        public ReportModelWrapper(ReportModel reportModel, ReportOwnerTypes reportOwnerType)
        {
            this.ReportModel = reportModel;
            this.ReportOwnerType = reportOwnerType;
        }

        public enum ReportOwnerTypes
        {
            Workstation,
            Server
        }

        [NotNull]
        public ReportModel ReportModel { get; set; }

        public ReportOwnerTypes ReportOwnerType { get; set; }

        public static List<ReportModelWrapper> Wrap(List<ReportModel> models, ReportOwnerTypes reportOwnerType)
        {
            var wrapReportModels =
                models.Select(x => new ReportModelWrapper(x, reportOwnerType)).ToList();

            return wrapReportModels;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ReportModelWrapper)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.ReportModel != null ? this.ReportModel.GetHashCode() : 0) * 397) ^ (int)this.ReportOwnerType;
            }
        }

        protected bool Equals(ReportModelWrapper other)
        {
            return Equals(this.ReportModel, other.ReportModel) && this.ReportOwnerType == other.ReportOwnerType;
        }
    }
}