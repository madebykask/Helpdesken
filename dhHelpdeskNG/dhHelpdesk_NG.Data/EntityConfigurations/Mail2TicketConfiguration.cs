namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain;
    using System.ComponentModel.DataAnnotations.Schema;

    internal sealed class Mail2TicketConfiguration : EntityTypeConfiguration<Mail2Ticket>
    {
        internal Mail2TicketConfiguration()
        {                        
            this.HasKey(f => f.Id);
            this.Property(f => f.Case_Id).IsOptional();
            this.Property(f => f.Log_Id).IsOptional();
            this.Property(f => f.EMailAddress).IsOptional().HasMaxLength(100);            
            this.Property(f => f.Type).IsOptional().HasMaxLength(10);
            this.Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);  

            this.ToTable("tblMail2Ticket");
        }
    }
}