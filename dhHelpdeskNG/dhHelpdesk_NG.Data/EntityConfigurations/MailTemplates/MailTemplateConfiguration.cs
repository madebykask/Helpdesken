namespace DH.Helpdesk.Dal.EntityConfigurations.MailTemplates
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.MailTemplates;

    public sealed class MailTemplateConfiguration : EntityTypeConfiguration<MailTemplateEntity>
    {
        internal MailTemplateConfiguration()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.MailTemplateGUID).IsRequired();
            this.Property(t => t.MailID).IsRequired();
            this.Property(t => t.Customer_Id).IsOptional();
            this.HasOptional(t => t.Customer).WithMany().HasForeignKey(t => t.Customer_Id).WillCascadeOnDelete(false);
            this.Property(t => t.IsStandard).IsRequired();
            this.Property(t => t.OrderType_Id).IsOptional();
            this.Property(t => t.SendMethod).IsRequired();
            this.HasOptional(t => t.OrderType).WithMany().HasForeignKey(t => t.OrderType_Id).WillCascadeOnDelete(false);
            this.Property(t => t.AccountActivity_Id).IsOptional();
            
            this.HasMany(t => t.MailTemplateLanguages)
		         .WithRequired(l => l.MailTemplate)
                 .HasForeignKey(t => t.MailTemplate_Id);

            this.HasOptional(t => t.AccountActivity)
                .WithMany()
                .HasForeignKey(t => t.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(t => t.IncludeLogText_External).IsRequired();

            this.ToTable("tblMailTemplate");
        }
    }
}