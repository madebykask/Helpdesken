namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class PriorityLanguageConfiguration : EntityTypeConfiguration<PriorityLanguage>
    {
        internal PriorityLanguageConfiguration()
        {
            this.HasKey(x => new { x.Language_Id, x.Priority_Id });

            this.HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Language_Id).IsRequired();
            this.Property(x => x.Priority_Id).IsRequired();
            //Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblPriority_tblLanguage");
        }
    }
}
