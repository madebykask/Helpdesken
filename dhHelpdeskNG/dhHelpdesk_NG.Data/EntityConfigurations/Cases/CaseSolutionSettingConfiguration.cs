namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    public class CaseSolutionSettingConfiguration : EntityTypeConfiguration<CaseSolutionSetting>
    {
        public CaseSolutionSettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.CaseSolution)
                .WithMany()
                .HasForeignKey(x => x.CaseSolution_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.Readonly).IsRequired();
            this.Property(c => c.Show).IsRequired();
            this.Property(c => c.CaseSolutionField).IsRequired().HasColumnName("FieldName_Id");

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblCaseSolutionFieldSettings");
        }
    }
}
