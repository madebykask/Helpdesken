﻿namespace dhHelpdesk_NG.Web.Models.Faq.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public sealed class FaqEditInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }
        
        [StringLength(1000)]
        public string InternalAnswer { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool InformationIsAvailableForNotifiers { get; set; }

        [StringLength(200)]
        public string UrlOne { get; set; }

        [StringLength(200)]
        public string UrlTwo { get; set; }

        [Required]
        [StringLength(100)]
        public string Question { get; set; }

        public int CategoryId { get; set; }

        public int? WorkingGroupId { get; set; }
    }
}