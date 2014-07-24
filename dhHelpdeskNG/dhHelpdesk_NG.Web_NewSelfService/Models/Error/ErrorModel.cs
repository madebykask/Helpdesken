using DH.Helpdesk.NewSelfService.Models;
using DH.Helpdesk.NewSelfService.Models.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.NewSelfService.Models.Error
{
    public class ErrorModel
    {
        public ErrorModel()
        {            
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }        

    }
}