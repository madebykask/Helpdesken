namespace DH.Helpdesk.Dal.EntityConfigurations.Problems
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Problems;

    public class ProblemConfiguration : EntityTypeConfiguration<Problem>
    {
        internal ProblemConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.ChangedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.ChangedByUser_Id)
                 .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ResponsibleUser)
                 .WithMany()
                 .HasForeignKey(x => x.ResponsibleUser_Id)
                 .WillCascadeOnDelete(false);

            this.HasMany(s => s.Cases)
                .WithOptional(s => s.Problem)
                .HasForeignKey(s => s.Problem_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.ChangedByUser_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(20);
            this.Property(x => x.Description).IsOptional().HasColumnName("ProblemDescription");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ProblemName");
            this.Property(x => x.ProblemNumber).IsRequired();
            this.Property(x => x.ProblemNumberPrefix).IsOptional().HasMaxLength(10);
            this.Property(x => x.ResponsibleUser_Id).IsOptional();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.FinishingDate).IsOptional();
            this.Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("ShowOnStartPage");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblproblem");
        }
    }
}
