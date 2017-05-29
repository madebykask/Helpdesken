using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Feedback
{
    public class FeedbackThankYouModel
    {
        public string Html { get; set; }
        public string NoteLabel { get; set; }

        public bool IsShowNote { get; set; }

        [StringLength(500)]
        public string NoteText { get; set; }

        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public int QuestionId { get; set; }
    }
}