using DH.Helpdesk.SelfService.Models;
using DH.Helpdesk.SelfService.Models.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.SelfService.Models.Error
{
    public class Error
    {
        public Error()
        {            
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string BackURL { get; set; }        

    }
}