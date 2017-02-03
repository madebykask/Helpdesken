using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
	public class InvoiceRowConfiguration : EntityTypeConfiguration<InvoiceRow>
	{
		internal InvoiceRowConfiguration()
		{
			this.HasKey(x => x.Id);

			this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			//this.Property(x => x.).IsRequired();
			//this.Property(x => x.).IsRequired().HasMaxLength(10);
			//this.Property(x => x.).IsRequired().HasMaxLength(1000);

			this.HasOptional(c => c.InvoiceHeader)
				.WithMany()
				.HasForeignKey(c => c.InvoiceHeader_Id)
				.WillCascadeOnDelete(false);

			this.HasOptional(c => c.Case)
				.WithMany()
				.HasForeignKey(c => c.Case_Id)
				.WillCascadeOnDelete(false);

			this.HasMany(x => x.Logs)
				.WithOptional(x => x.InvoiceRow)
				.HasForeignKey(x => x.InvoiceRow_Id);

			this.HasMany(x => x.CaseInvoiceRows)
				.WithOptional(x => x.InvoiceRow)
				.HasForeignKey(x => x.InvoiceRow_Id);

			this.HasOptional(c => c.CreatedByUser)
				.WithMany()
				.HasForeignKey(c => c.CreatedByUser_Id)
				.WillCascadeOnDelete(false);

			this.ToTable("tblInvoiceRow");
		}
	}
}