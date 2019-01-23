using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    public class WorkstationTabSettingLanguageConfiguration: EntityTypeConfiguration<WorkstationTabSettingLanguage>
    {
        internal WorkstationTabSettingLanguageConfiguration()
        {
            HasKey(x => new { x.WorkstationTabSetting_Id, x.Language_Id });

            HasRequired(x => x.WorkstationTabSetting)
                .WithMany(x => x.WorkstationTabSettingLanguages)
                .HasForeignKey(x => x.WorkstationTabSetting_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.FieldHelp).IsOptional().HasMaxLength(200);
            Property(x => x.Label).IsOptional().HasMaxLength(50);

            ToTable("tblWorkstationTabSetting_tblLang");
        }
    }
}