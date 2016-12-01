using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers
{
	public class MvcUrlNameBase
	{
		public abstract class BaseNameHelper<T> where T : Controller
		{
			public static string Controller
			{
				get { return GetController<T>(); }
			}

			protected static string GetAction(Expression<Func<T, object>> action)
			{
				return GetAction<T>(action);
			}

			#region Private Methods

			protected static string GetAction<TController>(Expression<Func<TController, object>> action)
				where TController : Controller
			{
				var methodExpression = (action.Body as MethodCallExpression);
				if (methodExpression == null) throw new ArgumentException("method expression");

				return methodExpression.Method.Name;
			}


			protected static string GetController<TController>()
				where TController : IController
			{
				return GetController(typeof(TController));
			}

			protected static string GetController(Type controller)
			{
				return controller.Name.Replace("Controller", string.Empty);
			}

			#endregion
		}
	}
}