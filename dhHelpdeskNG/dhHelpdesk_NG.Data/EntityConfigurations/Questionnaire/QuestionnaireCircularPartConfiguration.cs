namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireCircularPartConfiguration : EntityTypeConfiguration<QuestionnaireCircularPartEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularPartConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Guid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed).IsRequired();
            this.Property(c => c.QuestionnaireCircular_Id).IsRequired();
            
            this.HasRequired(c => c.QuestionnaireCircular)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireCircular_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.Case_Id).IsOptional();
            
            this.HasRequired(c => c.Case)
                .WithMany()
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(c => c.InputDate).IsOptional();
            this.Property(c => c.SendDate).IsOptional();
            this.Property(c => c.CreatedDate);            

            this.ToTable("tblQuestionnaireCircularPart");
        }

        #endregion
    }
}