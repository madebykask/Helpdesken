using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.FormLib.Models
{
    public class FormModelGlobalView
    {
        public IEnumerable<FormField> FormFields { get; set; }
        public FormModel FormModel { get; set; }
    }
}