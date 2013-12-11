using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseQuestionHeaderConfiguration : EntityTypeConfiguration<CaseQuestionHeader>
    {
        internal CaseQuestionHeaderConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Case)
                .WithMany(c => c.CaseQuestionHeaders)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            HasOptional(u => u.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Alternative).IsRequired();
            Property(x => x.AlternativeDescription).IsOptional().HasMaxLength(60);
            Property(x => x.FinishingDate).IsOptional();
            Property(x => x.SelectedAlternative).IsRequired();
            Property(x => x.VerificationAlternative).IsRequired();
            Property(x => x.Version).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasequestionheader");
        }
    }
}
