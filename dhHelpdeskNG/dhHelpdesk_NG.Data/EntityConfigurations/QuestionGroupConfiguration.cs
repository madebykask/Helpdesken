namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class QuestionGroupConfiguration : EntityTypeConfiguration<QuestionGroup>
    {
        internal QuestionGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("QuestionGroup");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblquestiongroup");
        }
    }
}
