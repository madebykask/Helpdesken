using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Authentication
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        private const string IsRefreshTokenExpiredName = "IsRefreshTokenExpired";
        //private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string,AuthenticationTicket>(); // Can be used to store tickets on server and pass only hashed key to client.

        public async override Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            if (!context.OwinContext.Environment.ContainsKey(IsRefreshTokenExpiredName) ||
                (bool) context.OwinContext.Environment[IsRefreshTokenExpiredName])
            {
                var dt = DateTime.Now;
                context.Ticket.Properties.IssuedUtc = dt;
                context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(dt.AddHours(8));//TODO:Move to config
                context.SetToken(context.SerializeTicket());
            }
        }

        public async override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            context.DeserializeTicket(context.Token);
            if (context.Ticket?.Properties.ExpiresUtc != null && context.Ticket.Properties.ExpiresUtc.Value.LocalDateTime > DateTime.Now)
            {
                context.OwinContext.Environment[IsRefreshTokenExpiredName] = false;
                //context.OwinContext.Set("custom.ExpriredToken", true); // if customized error message is required read this value in request pipeline and place your message to responce
            }
        }

    }
}