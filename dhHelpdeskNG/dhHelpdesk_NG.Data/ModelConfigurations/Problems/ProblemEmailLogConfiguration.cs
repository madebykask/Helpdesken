namespace dhHelpdesk_NG.Data.ModelConfigurations.Problems
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Problems;

    public class ProblemEmailLogConfiguration : EntityTypeConfiguration<ProblemEMailLog>
    {
        internal ProblemEmailLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.ProblemLog)
                .WithMany()
                .HasForeignKey(x => x.ProblemLog_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.ToTable("tblproblememaillog");
        }
    }
}