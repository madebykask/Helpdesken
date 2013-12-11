using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class QuestionGroupConfiguration : EntityTypeConfiguration<QuestionGroup>
    {
        internal QuestionGroupConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("QuestionGroup");
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblquestiongroup");
        }
    }
}
