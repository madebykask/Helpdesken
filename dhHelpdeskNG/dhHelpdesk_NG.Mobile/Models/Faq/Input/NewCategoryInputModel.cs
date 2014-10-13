namespace DH.Helpdesk.Mobile.Models.Faq.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class NewCategoryInputModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}