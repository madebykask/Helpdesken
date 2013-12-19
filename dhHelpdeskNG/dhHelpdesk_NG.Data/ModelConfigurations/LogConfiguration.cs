using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class LogConfiguration : EntityTypeConfiguration<Log>
    {
        internal LogConfiguration()
        {
            HasKey(l => l.Id);

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

            //HasOptional(l => l.CaseHistory)
            //            .WithMany()
            //            .HasForeignKey(l => l.CaseHistory_Id)
            //            .WillCascadeOnDelete(false);

            Property(l => l.Case_Id).IsRequired();
            Property(l => l.LogGUID).IsRequired();
            Property(l => l.LogDate).IsRequired();
            Property(l => l.Text_External).IsRequired().HasMaxLength(3000);
            Property(l => l.Text_Internal).IsRequired().HasMaxLength(3000);
            Property(l => l.User_Id).IsOptional();
            Property(l => l.InformCustomer).IsRequired();
            Property(l => l.FinishingDate).IsOptional();
            Property(l => l.WorkingTime).IsRequired();
            Property(l => l.EquipmentPrice).IsRequired();
            Property(l => l.Price).IsRequired();
            Property(l => l.Charge).IsRequired();
            Property(l => l.Export).IsRequired();
            Property(l => l.ExportDate).IsOptional();
            Property(l => l.LogType).IsRequired();
            Property(l => l.RegTime).IsRequired();
            Property(l => l.ChangeTime).IsRequired();
            Property(l => l.RegUser).IsRequired();

            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblLog");
        }
    }
}