using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class OU
    {
        public int Id { get; set; }
        public string OUId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Code { get; set; } //ADD by TAN 2015-11-17
    }
}
