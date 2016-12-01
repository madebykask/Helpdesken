using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Feedback
{
	public class FeedbackFullItem
	{
		public FeedbackFullItem()
		{
			Options = new List<FeedbackQuestionOption>();
		}

		public FeedbackOverview Info { get; set; }
		public FeedbackQuestionOverview Question { get; set; }
		public List<FeedbackQuestionOption>  Options { get; set; }

	}

}
