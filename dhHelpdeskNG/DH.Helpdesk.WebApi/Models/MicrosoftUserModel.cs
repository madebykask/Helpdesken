using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.WebApi.Models
{
    public class MicrosoftUserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string IdToken { get; set; }
    }
}