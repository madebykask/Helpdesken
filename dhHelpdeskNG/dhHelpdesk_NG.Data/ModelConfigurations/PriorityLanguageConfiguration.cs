using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class PriorityLanguageConfiguration : EntityTypeConfiguration<PriorityLanguage>
    {
        internal PriorityLanguageConfiguration()
        {
            HasKey(x => new { x.Language_Id, x.Priority_Id });

            HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Language_Id).IsRequired();
            Property(x => x.Priority_Id).IsRequired();
            //Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblPriority_tblLanguage");
        }
    }
}
