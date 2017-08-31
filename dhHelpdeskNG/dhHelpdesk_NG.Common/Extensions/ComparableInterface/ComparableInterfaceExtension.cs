using DH.Helpdesk.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Common.Extensions
{
	public static class ComparableInterfaceExtension
	{
		public static ValueCompare CompareValueWith(this IComparable me, IComparable with)
		{
			var comp = me.CompareTo(with);

			return comp > 0 ? ValueCompare.LargerThan :
				comp == 0 ? ValueCompare.Equal :
				ValueCompare.LessThan;
		}
	}
}
