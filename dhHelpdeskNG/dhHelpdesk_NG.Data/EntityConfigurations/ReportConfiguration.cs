namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        internal ReportConfiguration()
        {
            this.HasKey(x => x.Id);

            this.ToTable("tblreport");
        }
    }

    public class ReportCustomerConfiguration : EntityTypeConfiguration<ReportCustomer>
    {
        internal ReportCustomerConfiguration()
        {
            this.HasKey(x => new { x.Customer_Id, x.Report_Id });

            this.Property(x => x.ShowOnPage).IsRequired().HasColumnName("Show");

            this.ToTable("tblReport_tblCustomer");
        }
    }
}
