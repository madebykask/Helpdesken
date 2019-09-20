namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FileViewLogConfiguration : EntityTypeConfiguration<FileViewLogEntity>
    {
        internal FileViewLogConfiguration()
        {
            this.HasKey(f => f.Id);            

            this.Property(f => f.Case_Id).IsRequired();
            this.Property(f => f.User_Id).IsRequired();
            this.Property(f => f.FileName).IsRequired().HasMaxLength(200);
            this.Property(f => f.FilePath).IsRequired().HasMaxLength(200);
            this.Property(f => f.FileSource).IsRequired();
			this.Property(f => f.Operation).IsRequired();
			this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            this.ToTable("tblFileViewLog");
        }
    }
}
