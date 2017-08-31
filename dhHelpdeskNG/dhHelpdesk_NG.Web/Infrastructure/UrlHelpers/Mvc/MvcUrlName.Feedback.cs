using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc
{
	public partial class MvcUrlName
	{
		public class Feedback : BaseNameHelper<FeedbackController>
		{
			public static string New
			{
				get { return GetAction(e => e.NewFeedback(null)); }
			}

			public static string Edit
			{
				//get { return GetAction(e => e.EditFeedback(null)); }
				get { return "EditFeedback"; }
			}

			public static string Delete
			{
				get { return GetAction(e => e.DeleteFeedback(null)); }
			}

			public static string DeleteQuestionOption 
			{
				get { return GetAction(e => e.DeleteQuestionOption(null)); }	
			}

			public static string AddQuestionOption
			{
				get { return GetAction(e => e.AddQuestionOption(null)); }
			}

            public static string UpdateOptionIcon
            {
                get { return GetAction(e => e.UpdateOptionIcon(null)); }
            }
        }
	}
}