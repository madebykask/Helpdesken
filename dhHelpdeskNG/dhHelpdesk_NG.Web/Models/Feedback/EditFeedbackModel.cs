using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
using DH.Helpdesk.Web.Models.Questionnaire.Input;

namespace DH.Helpdesk.Web.Models.Feedback
{
	public class EditFeedbackModel
	{
		public EditFeedbackModel()
		{
			Options = new List<QuestionnaireQuesOptionModel>();
            CaseFilter = new CircularCaseFilter();
        }

		public int QuestionId { get; set; }

		[LocalizedRequired]
		[StringLength(100)]
		[LocalizedDisplay("Name")]
		public string Name { get; set; }

        public bool IsSent { get; set; }

        [LocalizedDisplay("Description")]
		public string Description { get; set; }

		[LocalizedDisplay("LanguageId")]
		public int LanguageId { get; set; }

		[LocalizedDisplay("QuestionnaireId")]
		public int QuestionnaireId { get; set; }

		[LocalizedRequired]
		[StringLength(100)]
		[LocalizedDisplay("Identifier")]
		public string Identifier { get; set; }

		[LocalizedDisplay("SelectedPercent")]
		public int SelectedPercent { get; set; }

		[LocalizedRequired]
		[StringLength(1000)]
		[LocalizedDisplay("Question")]
		public string Question { get; set; }

		[LocalizedDisplay("ShowNote")]
		public int ShowNote { get; set; }

		[StringLength(1000)]
		[LocalizedDisplay("NoteText")]
		public string NoteText { get; set; }

		[LocalizedDisplay("CreateDate")]
		public DateTime CreateDate { get; set; }

		public int? CircularId { get; set; }

        public CircularCaseFilter CaseFilter { get; set; }

		public bool ExcludeAdministrators { get; set; }
        public bool UseBase64Images { get; set; }

        public List<QuestionnaireQuesOptionModel> Options { get; set; }

		public bool IsNew
		{
			get { return QuestionId <= 0; }
		}


	}
}