namespace DH.Helpdesk.Dal.EntityConfigurations.Problems
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Problems;

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

            this.Property(x => x.ProblemLogGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ShowOnCase).IsRequired();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(x => x.FinishConnectedCases).IsRequired();

            this.ToTable("tblproblemlog");
        }
    }
}