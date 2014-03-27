namespace DH.Helpdesk.Dal.EntityConfigurations.Faq
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Faq;

    public sealed class FaqConfiguration : EntityTypeConfiguration<FaqEntity>
    {
        #region Constructors and Destructors

        internal FaqConfiguration()
        {
            this.HasKey(f => f.Id);
            
            this.Property(f => f.FAQCategory_Id).IsRequired().HasColumnName("Id_FAQCategory");

            this.HasRequired(f => f.FAQCategory)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.FAQCategory_Id)
                .WillCascadeOnDelete(false);

            this.Property(f => f.FAQQuery).IsRequired().HasMaxLength(100);
            this.Property(f => f.Answer).IsRequired().HasMaxLength(2000);
            this.Property(f => f.Answer_Internal).IsRequired().HasMaxLength(1000);
            this.Property(f => f.InformationIsAvailableForNotifiers).HasColumnName("PublicFAQ").IsRequired();
            this.Property(f => f.URL1).IsRequired().HasMaxLength(200);
            this.Property(f => f.URL2).IsRequired().HasMaxLength(200);
            this.Property(f => f.Customer_Id).IsOptional();

            this.HasOptional(f => f.Customer)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(f => f.WorkingGroup_Id).IsOptional();

            this.HasOptional(f => f.WorkingGroup)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(f => f.ShowOnStartPage).IsRequired();
            this.Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(f => f.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
           
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.ToTable("tblFAQ");
        }

        #endregion
    }
}