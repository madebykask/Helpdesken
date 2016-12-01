using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc
{
	public partial class MvcUrlName
	{
		public class Questionnaire : BaseNameHelper<QuestionnaireController>
		{
			public static string Index
			{
				get { return GetAction(e => e.Index()); }
			}

		}
	}
}