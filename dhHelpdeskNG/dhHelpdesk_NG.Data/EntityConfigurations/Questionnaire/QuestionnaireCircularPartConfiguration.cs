namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;
    public class QuestionnaireCircularPartConfiguration: EntityTypeConfiguration<QuestionnaireCircularPartEntity>
    {
        internal QuestionnaireCircularPartConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(q => q.GUID).IsRequired();
            this.Property(q => q.QuestionnaireCircularId).IsRequired().HasColumnName("QuestionnaireCircularId");
            this.Property(c => c.CaseId).IsOptional().HasColumnName("Case_Id");            
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            
            this.Property(x => x.InputDate).IsOptional();
            this.Property(x => x.SendDate).IsOptional();

            this.HasOptional(c => c.QuestionnaireCircular).WithMany().HasForeignKey(c => c.QuestionnaireCircularId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.Case).WithMany().HasForeignKey(c => c.CaseId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireCircularPart");
        }

    }
}
