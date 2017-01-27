using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class ReportFavoriteConfiguration : EntityTypeConfiguration<ReportFavorite>
    {
        internal ReportFavoriteConfiguration()
        {
            this.HasKey(l => l.Id);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(l => l.Customer)
                .WithMany(l => l.ReportFavorites)
                .HasForeignKey(l => l.Customer_Id)
                .WillCascadeOnDelete(false);

			this.HasRequired(l => l.User)
				.WithMany(l => l.ReportFavorites)
				.HasForeignKey(l => l.User_Id)
				.WillCascadeOnDelete(false);

			this.Property(l => l.Name).IsRequired().HasMaxLength(200);
            this.Property(l => l.Type).IsRequired();

            this.ToTable("tblReportFavorites");
        }
    }
}
