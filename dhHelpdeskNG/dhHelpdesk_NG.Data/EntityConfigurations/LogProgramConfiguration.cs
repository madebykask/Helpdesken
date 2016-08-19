namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LogProgramConfiguration : EntityTypeConfiguration<LogProgramEntity>
    {
        internal LogProgramConfiguration()
        {
            this.HasKey(l => l.Id);

            this.Property(l => l.Log_Type).IsRequired();
            this.Property(l => l.Case_Id).IsRequired();
            this.Property(l => l.Old_Performer_User_Id).IsRequired();
            this.Property(l => l.New_Performer_user_Id).IsRequired();
            this.Property(l => l.User_Id).IsOptional();
            this.Property(l => l.Customer_Id).IsOptional();
            this.Property(l => l.LogText).IsRequired().HasMaxLength(2000);
            this.Property(l => l.RegTime).IsRequired();

            this.HasOptional(l => l.User)
                        .WithMany()
                        .HasForeignKey(l => l.User_Id)
                        .WillCascadeOnDelete(false);

            this.ToTable("tblLogProgram");
        }
    }
}