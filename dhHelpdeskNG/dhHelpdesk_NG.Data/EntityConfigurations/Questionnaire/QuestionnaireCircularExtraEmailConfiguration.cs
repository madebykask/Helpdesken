using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    internal sealed class QuestionnaireCircularExtraEmailConfiguration : EntityTypeConfiguration<QuestionnaireCircularExtraEmailEntity>
    {
        internal QuestionnaireCircularExtraEmailConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.QuestionnaireCircular_Id).IsRequired();
            this.Property(c => c.Email).IsRequired().HasMaxLength(100);

            this.ToTable("tblQuestionnaireCircularExtraEmails");
        }
    }
}
