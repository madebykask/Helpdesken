using DH.Helpdesk.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class LogConfiguration : EntityTypeConfiguration<Log>
    {
        internal LogConfiguration()
        {
            HasKey(l => l.Id);
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(l => l.Case_Id).IsRequired();
            Property(l => l.LogGUID).IsRequired();
            Property(l => l.LogDate).IsRequired();
            Property(l => l.Text_External).IsRequired();
            Property(l => l.Text_Internal).IsRequired();
            Property(l => l.User_Id).IsOptional();
            Property(l => l.InformCustomer).IsRequired();
            Property(l => l.FinishingDate).IsOptional();
            Property(l => l.WorkingTime).IsRequired();
            Property(l => l.OverTime).IsRequired();
            Property(l => l.EquipmentPrice).IsRequired();
            Property(l => l.Price).IsRequired();
            Property(l => l.Charge).IsRequired();
            Property(l => l.Export).IsRequired();
            Property(l => l.ExportDate).IsOptional();
            Property(l => l.LogType).IsRequired();
            Property(l => l.RegTime).IsRequired();
            Property(l => l.ChangeTime).IsRequired();
            Property(l => l.RegUser).IsRequired();

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

            HasOptional(l => l.CaseHistory)
                .WithMany()
                .HasForeignKey(l => l.CaseHistory_Id)
                .WillCascadeOnDelete(false);

            HasOptional(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.User_Id)
                .WillCascadeOnDelete(false);

            HasOptional(l => l.FinishingTypeEntity)
                .WithMany()
                .HasForeignKey(l => l.FinishingType);

            HasMany(l => l.LogFiles)
                .WithRequired(f => f.Log);

            HasOptional(c => c.InvoiceRow)
                .WithMany()
                .HasForeignKey(c => c.InvoiceRow_Id)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Mail2Tickets)
                .WithOptional(x => x.Log)
                .HasForeignKey(x => x.Log_Id);

            ToTable("tblLog");
        }
    }
}