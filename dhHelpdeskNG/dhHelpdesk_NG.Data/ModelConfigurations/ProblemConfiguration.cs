namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain;

    public class ProblemConfiguration : EntityTypeConfiguration<Problem>
    {
        internal ProblemConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.ChangedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.ChangedByUser_Id)
                 .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ResponsibleUser)
                 .WithMany()
                 .HasForeignKey(x => x.ResponsibleUser_Id)
                 .WillCascadeOnDelete(false);

            Property(x => x.ChangedByUser_Id).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.InventoryNumber).IsRequired().HasMaxLength(20);
            Property(x => x.Description).IsOptional().HasColumnName("ProblemDescription");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ProblemName");
            Property(x => x.ProblemNumber).IsRequired();
            Property(x => x.ProblemNumberPrefix).IsOptional().HasMaxLength(10);
            Property(x => x.ResponsibleUser_Id).IsOptional();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.FinishingDate).IsOptional();
            Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("ShowOnStartPage");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblproblem");
        }
    }
}
