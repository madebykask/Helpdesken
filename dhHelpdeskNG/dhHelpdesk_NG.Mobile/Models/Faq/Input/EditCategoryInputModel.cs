namespace DH.Helpdesk.Mobile.Models.Faq.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class EditCategoryInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}