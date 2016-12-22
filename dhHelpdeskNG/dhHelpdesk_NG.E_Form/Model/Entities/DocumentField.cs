using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class DocumentField
    {
        public string DocumentFieldName { get; set; } 
        public string DocumentFieldValue { get; set ; }

        public bool IsMandatory { get; set; }

    }
}
