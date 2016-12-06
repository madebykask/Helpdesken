using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers
{
	public class WebApiUrlNameBase
	{
		public abstract class BaseNameHelper<T> where T : ApiController
		{
			public static string Controller
			{
				get { return GetController<T>(); }
			}

			protected static string GetAction(Expression<Func<T, object>> action)
			{
				return GetAction<T>(action);
			}

			protected static string GetController<TController>()
				where TController : IHttpController
			{
				return GetController(typeof(TController));
			}

			protected static string GetController(Type controller)
			{
				return controller.Name.Replace("Controller", string.Empty);
			}

			private static string GetAction<TController>(Expression<Func<TController, object>> action)
				where TController : ApiController
			{
				var methodExpression = (action.Body as MethodCallExpression);
				if (methodExpression == null) throw new ArgumentException("method expression");

				return methodExpression.Method.Name;
			}
		}
	}

}