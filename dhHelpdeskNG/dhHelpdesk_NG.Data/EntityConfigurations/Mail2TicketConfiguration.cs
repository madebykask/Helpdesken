using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    internal sealed class Mail2TicketConfiguration : EntityTypeConfiguration<Mail2Ticket>
    {
        internal Mail2TicketConfiguration()
        {                        
            HasKey(f => f.Id);
            Property(f => f.Case_Id).IsOptional();
            Property(f => f.Log_Id).IsOptional();
            Property(f => f.EMailAddress).IsOptional().HasMaxLength(100);
            Property(f => f.EMailSubject).IsOptional().HasMaxLength(512);
            Property(f => f.Type).IsOptional().HasMaxLength(10);
            Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);  

            ToTable("tblMail2Ticket");
        }
    }
}