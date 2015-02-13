namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LogConfiguration : EntityTypeConfiguration<Log>
    {
        internal LogConfiguration()
        {
            this.HasKey(l => l.Id);

            //HasRequired(l => l.Case)
            //            .WithMany(l => l.Cases)
            //            .HasForeignKey(l => l.Case_Id)
            //            .WillCascadeOnDelete(false);

            //HasOptional(l => l.FinishingCause)
            //            .HasForeignKey(l => l.FinishingType)
            //            .WithOptionalDependent()
            //            .WillCascadeOnDelete(false);

            //HasOptional(l => l.User)
            //            .WithMany()
            //            .HasForeignKey(l => l.User_Id)
            //            .WillCascadeOnDelete(false);

            this.HasOptional(l => l.CaseHistory)
                        .WithMany()
                        .HasForeignKey(l => l.CaseHistory_Id)
                        .WillCascadeOnDelete(false);

            this.HasOptional(l => l.User)
                        .WithMany()
                        .HasForeignKey(l => l.User_Id)
                        .WillCascadeOnDelete(false);

            this.Property(l => l.Case_Id).IsRequired();
            this.Property(l => l.LogGUID).IsRequired();
            this.Property(l => l.LogDate).IsRequired();
            this.Property(l => l.Text_External).IsRequired().HasMaxLength(3000);
            this.Property(l => l.Text_Internal).IsRequired().HasMaxLength(3000);
            this.Property(l => l.User_Id).IsOptional();
            this.Property(l => l.InformCustomer).IsRequired();
            this.Property(l => l.FinishingDate).IsOptional();
            this.Property(l => l.WorkingTime).IsRequired();
            this.Property(l => l.EquipmentPrice).IsRequired();
            this.Property(l => l.Price).IsRequired();
            this.Property(l => l.Charge).IsRequired();
            this.Property(l => l.Export).IsRequired();
            this.Property(l => l.ExportDate).IsOptional();
            this.Property(l => l.LogType).IsRequired();
            this.Property(l => l.RegTime).IsRequired();
            this.Property(l => l.ChangeTime).IsRequired();
            this.Property(l => l.RegUser).IsRequired();

            this.HasOptional(l => l.FinishingTypeEntity)
                .WithMany()
                .HasForeignKey(l => l.FinishingType);

            this.HasMany(l => l.LogFiles)
                .WithRequired(f => f.Log);

            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblLog");
        }
    }
}