using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.BusinessData.Models.Feedback
{

    public class FeedbackField
    {
        public string Key { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public FieldTypes FieldType { get; set; }
        public int FeedbackId { get; set; }
        public int CircularId { get; set; }
        public Guid CircularPartGuid { get; set; }
        public bool ExcludeAdministrators { get; set; }
        public bool UseBase64Images { get; set; }
    }
}
