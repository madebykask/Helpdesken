namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain;

    public class ProblemLogConfiguration : EntityTypeConfiguration<ProblemLog>
    {
        internal ProblemLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Problem)
                .WithMany()
                .HasForeignKey(x => x.Problem_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FinishingCause)
                .WithMany()
                .HasForeignKey(x => x.FinishingCause_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(x => x.Problem_Id).IsRequired();
            this.Property(x => x.ChangedByUser_Id).IsRequired();

            this.Property(x => x.FinishingCause_Id).IsOptional();
            this.Property(x => x.FinishingDate).IsOptional();

            this.Property(x => x.ProblemLogGUID).IsRequired();
            this.Property(x => x.ShowOnCase).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(x => x.FinishConnectedCases).IsRequired();

            this.ToTable("tblproblemlog");
        }
    }
}