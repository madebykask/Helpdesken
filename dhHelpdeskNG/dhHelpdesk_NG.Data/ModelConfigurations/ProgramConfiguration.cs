using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;
using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ProgramConfiguration : EntityTypeConfiguration<Program>
    {
        internal ProgramConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.List).IsOptional().HasMaxLength(200).HasColumnName("EMailList");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Program");
            Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("Show");           
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblprogram");
        }
    }
}
