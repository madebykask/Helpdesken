using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        internal ReportConfiguration()
        {
            HasKey(x => x.Id);

            ToTable("tblreport");
        }
    }

    public class ReportCustomerConfiguration : EntityTypeConfiguration<ReportCustomer>
    {
        internal ReportCustomerConfiguration()
        {
            HasKey(x => new { x.Customer_Id, x.Report_Id });

            Property(x => x.ShowOnPage).IsRequired().HasColumnName("Show");

            ToTable("tblReport_tblCustomer");
        }
    }
}
