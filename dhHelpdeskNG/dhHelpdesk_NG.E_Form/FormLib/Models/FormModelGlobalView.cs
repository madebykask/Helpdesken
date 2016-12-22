using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECT.Model.Entities;

namespace ECT.FormLib.Models
{
    public class FormModelGlobalView
    {
        public IEnumerable<FormField> FormFields { get; set; }
        public FormModel FormModel { get; set; }
    }
}