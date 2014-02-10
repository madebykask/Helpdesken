namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnaireConfiguration : EntityTypeConfiguration<QuestionnaireEntity>
    {
        internal QuestionnaireConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(q => q.QuestionnaireName).IsRequired().HasMaxLength(100);
            this.Property(q => q.QuestionnaireDescription).IsRequired();
            this.Property(c => c.CustomerId).IsOptional().HasColumnName("Customer_Id");            
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasOptional(c => c.Customer).WithMany().HasForeignKey(c => c.CustomerId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaire");
        }
    }
}

