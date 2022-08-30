using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
	/// <summary>
	/// Attribute checks and validate antiforgery token.
	/// Looks for token in request header instead of request body.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class ValidateApiAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
	{
		public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{
			if (actionContext.Request.Method != HttpMethod.Post)
				return continuation();
			if (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreAntiForgeryTokenAttribute>().Any())
				return continuation();

			string currentToken = null;
			var cookie = actionContext.Request.Headers
				 .GetCookies()
				 .Select(c => c[AntiForgeryConfig.CookieName])
				 .FirstOrDefault();

			IEnumerable<string> xXsrfHeaders;

			if (actionContext.Request.Headers.TryGetValues("X-XSRF-Token", out xXsrfHeaders))
				currentToken = xXsrfHeaders.FirstOrDefault();

			if (cookie == null)
				return ErrorResponse(actionContext);

			try
			{
				if (string.IsNullOrWhiteSpace(currentToken))
					AntiForgery.Validate();
				else
					AntiForgery.Validate(cookie.Value, currentToken);
			}
			catch (Exception)
			{
				return ErrorResponse(actionContext);
			}
			return continuation();
		}

		private Task<HttpResponseMessage> ErrorResponse(HttpActionContext actionContext)
		{
			actionContext.Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Forbidden,
				RequestMessage = actionContext.ControllerContext.Request
			};

			return FromResult(actionContext.Response);
		}

		private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
		{
			var source = new TaskCompletionSource<HttpResponseMessage>();
			source.SetResult(result);
			return source.Task;
		}
	}
}