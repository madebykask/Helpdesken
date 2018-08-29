using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi.Models
{
    public class RefreshTokenInputModel
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string ClientId { get; set; }
    }
}