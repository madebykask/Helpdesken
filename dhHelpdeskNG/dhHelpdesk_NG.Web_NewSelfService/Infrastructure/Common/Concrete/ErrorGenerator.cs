using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.NewSelfService.Infrastructure.Common.Concrete
{            
    using DH.Helpdesk.BusinessData.Models.Error;

    public static class ErrorGenerator
    {
        public static ErrorModel MakeError(string message, int? code = null)
        {
            var err = new ErrorModel((code!=null? code.Value:0), message);
            SessionFacade.LastError = err;
            return err;            
        }
    }
}