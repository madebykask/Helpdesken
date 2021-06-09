namespace DH.Helpdesk.Web.Models.Faq.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class NewFaqInputModel
    {
        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }

        [StringLength(1000)]
        public string InternalAnswer { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool InformationIsAvailableForNotifiers { get; set; }

        [StringLength(2000)]
        public string UrlOne { get; set; }

        [StringLength(2000)]
        public string UrlTwo { get; set; }

        [Required]
        [StringLength(100)]
        public string Question { get; set; }

        public int? WorkingGroupId { get; set; }

        public int CategoryId { get; set; }

        public string Id { get; set; }
    }
}