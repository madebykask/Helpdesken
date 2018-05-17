using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain.Orders
{
    public enum EMailTypes
    {
        [Display(Name ="Standard")]
        Standard = 1,
        [Display(Name = "Utökad")]
        Expanded = 2,
        [Display(Name = "Ingen e-post")]
        NoEmail = 3
    }
}