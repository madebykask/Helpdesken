using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class OrderFieldTypeConfiguration : EntityTypeConfiguration<OrderFieldType>
    {
        internal OrderFieldTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.OrderType)
                .WithMany()
                .HasForeignKey(a => a.OrderType_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).HasColumnName("OrderFieldType").HasMaxLength(50).IsRequired();
            this.Property(x => x.OrderField).IsRequired();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblOrderFieldTypes");
        }
    }
}
