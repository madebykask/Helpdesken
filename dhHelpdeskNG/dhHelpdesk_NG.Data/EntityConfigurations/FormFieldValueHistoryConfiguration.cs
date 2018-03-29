using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class FormFieldValueHistoryConfiguration : EntityTypeConfiguration<FormFieldValueHistory>
    {
        internal FormFieldValueHistoryConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Case_Id).IsRequired();
            Property(x => x.FormField_Id).IsRequired();
            Property(x => x.CaseHistory_Id).IsRequired();
            Property(x => x.FormFieldValue).IsRequired().HasMaxLength(2000);
            Property(x => x.FormFieldText).IsOptional().HasMaxLength(2000);
            Property(x => x.InitialFormFieldText).IsRequired().HasMaxLength(2000);
            Property(x => x.InitialFormFieldValue).IsRequired().HasMaxLength(2000);
            ToTable("tblFormFieldValueHistory");
        }
    }
}
