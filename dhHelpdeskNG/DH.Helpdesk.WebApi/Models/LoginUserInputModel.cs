using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi.Models
{
    public class LoginUserInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
    }
}