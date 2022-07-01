namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FinishingCauseConfiguration : EntityTypeConfiguration<FinishingCause>
    {
        internal FinishingCauseConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FinishingCauseCategory)
                .WithMany()
                .HasForeignKey(x => x.FinishingCauseCategory_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ParentFinishingCause)
                .WithMany(x => x.SubFinishingCauses)
                .HasForeignKey(x => x.Parent_FinishingCause_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.FinishingCauseCategory_Id).IsOptional();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("FinishingCause");
            this.Property(x => x.Parent_FinishingCause_Id).IsOptional();
            this.Property(x => x.PromptUser).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.FinishingCauseGUID).IsOptional();
            this.Property(x => x.Merged).IsOptional();

            this.ToTable("tblfinishingcause");
        }
    }
}
