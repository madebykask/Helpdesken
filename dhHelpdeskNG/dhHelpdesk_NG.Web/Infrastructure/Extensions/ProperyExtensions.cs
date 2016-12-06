using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
	public static class ProperyExtensions
	{
		public static string NameOf<T, TProp>(this T o, Expression<Func<T, TProp>> propertySelector)
		{
			var body = (MemberExpression)propertySelector.Body;
			return body.Member.Name;
		}
	}
}