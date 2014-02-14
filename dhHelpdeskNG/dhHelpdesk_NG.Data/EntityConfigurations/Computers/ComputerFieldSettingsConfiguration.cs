namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerFieldSettingsConfiguration : EntityTypeConfiguration<ComputerFieldSettings>
    {
        public ComputerFieldSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ComputerField).IsRequired().HasMaxLength(50);
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.Label).IsRequired().HasMaxLength(50);
            this.Property(x => x.Label_ENG).IsRequired().HasMaxLength(50);
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.FieldHelp).IsRequired().HasMaxLength(200);
            this.Property(x => x.ShowInList).IsRequired();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblComputerFieldSettings");
        }
    }
}