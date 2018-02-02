using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Common.Extensions.Object
{
	public static class ObjectExtension
	{
		public static T Use<T>(this T t, Func<T, T> use)
		{
			return use(t);
		}
	}
}
