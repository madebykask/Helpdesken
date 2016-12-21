using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using DH.Helpdesk.Web.Infrastructure.Utilities;

namespace DH.Helpdesk.Web.Infrastructure
{
	public static class RouteValueDictionaryHelper
	{
		public static RouteValueDictionary ObjectToDictionary(this object value)
		{
			var dictionary = new RouteValueDictionary();

			if (value == null) return dictionary;

			foreach (var helper in PropertyHelper.GetProperties(value))
			{
				dictionary.Add(helper.Name, helper.GetValue(value));
			}

			return dictionary;
		}

	}
}
