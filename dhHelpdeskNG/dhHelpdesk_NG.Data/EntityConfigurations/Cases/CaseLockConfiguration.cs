namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    internal sealed class CaseLockConfiguration : EntityTypeConfiguration<CaseLockEntity>
    {
        internal CaseLockConfiguration()
        {
            
            this.HasKey(l => l.Id);

            this.HasRequired(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.User_Id)
                 .WillCascadeOnDelete(false);

            this.Property(l => l.Case_Id).IsRequired();
            this.Property(l => l.User_Id).IsRequired();
            this.Property(l => l.LockGUID).IsRequired();
            this.Property(l => l.BrowserSession).IsRequired();
            this.Property(l => l.CreatedTime).IsRequired(); 
            this.Property(l => l.ExtendedTime).IsRequired();
            this.Property(l => l.ActiveTab).IsRequired().HasMaxLength(100);

            this.ToTable("tblCaseLock");
        }
    }
}