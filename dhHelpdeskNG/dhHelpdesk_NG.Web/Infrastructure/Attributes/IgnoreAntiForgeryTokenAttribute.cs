using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class IgnoreAntiForgeryTokenAttribute : Attribute
	{
	}
}