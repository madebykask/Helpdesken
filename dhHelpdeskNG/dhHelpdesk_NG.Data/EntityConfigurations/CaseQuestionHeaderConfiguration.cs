namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseQuestionHeaderConfiguration : EntityTypeConfiguration<CaseQuestionHeader>
    {
        internal CaseQuestionHeaderConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Case)
                .WithMany(c => c.CaseQuestionHeaders)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(u => u.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Alternative).IsRequired();
            this.Property(x => x.AlternativeDescription).IsOptional().HasMaxLength(60);
            this.Property(x => x.FinishingDate).IsOptional();
            this.Property(x => x.SelectedAlternative).IsRequired();
            this.Property(x => x.VerificationAlternative).IsRequired();
            this.Property(x => x.Version).IsRequired();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcasequestionheader");
        }
    }
}
