namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseQuestionConfiguration : EntityTypeConfiguration<CaseQuestion>
    {
        internal CaseQuestionConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.CaseQuestionCategory)
                .WithMany(c => c.CaseQuestions)
                .HasForeignKey(c => c.CaseQuestionCategory_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Answer).IsRequired();
            this.Property(x => x.Note).IsOptional().HasMaxLength(100);
            this.Property(x => x.Question).IsOptional().HasMaxLength(2000);
            this.Property(x => x.QuestionHelp).IsOptional().HasMaxLength(500);
            this.Property(x => x.Weight).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcasequestion");
        }
    }
}
