using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
	public class InvoiceHeaderConfiguration : EntityTypeConfiguration<InvoiceHeader>
	{
		internal InvoiceHeaderConfiguration()
		{
			this.HasKey(x => x.Id);

			this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			//this.Property(x => x.).IsRequired();
			//this.Property(x => x.).IsRequired().HasMaxLength(10);
			//this.Property(x => x.).IsRequired().HasMaxLength(1000);

			this.HasMany(x => x.InvoiceRows)
			.WithOptional(x => x.InvoiceHeader)
			.HasForeignKey(x => x.InvoiceHeader_Id);

			this.HasRequired(c => c.CreatedByUser)
				.WithMany()
				.HasForeignKey(c => c.User_Id)
				.WillCascadeOnDelete(false);

			this.ToTable("tblInvoiceHeader");
		}
	}
}
