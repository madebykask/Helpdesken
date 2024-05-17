using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Feedback
{
    public static class FeedbackFieldMapper
    {
        public static Field MapToFields(this FeedbackField ffield)
        {
            return new Field
            {
                Id = int.MaxValue, //FeedbackField doesnot contain Id
                FieldType = FieldTypes.String,
                Key = ffield.Key,
                StringValue = ffield.StringValue,
                DateTimeValue = ffield.DateTimeValue 
            };
        }
    }
}
