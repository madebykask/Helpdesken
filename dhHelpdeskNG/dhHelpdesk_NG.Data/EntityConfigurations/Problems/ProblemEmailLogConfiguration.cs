namespace DH.Helpdesk.Dal.EntityConfigurations.Problems
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Problems;

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