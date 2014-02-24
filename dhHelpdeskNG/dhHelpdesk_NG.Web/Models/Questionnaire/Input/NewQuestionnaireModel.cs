using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class NewQuestionnaireModel
    {
        [IsId]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [LocalizedDisplay("Name")]
        public string Name { get; set; }

        [Required]
        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("CustomerId")]
        public int CustomerId { get; set; }

        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

    }
}