﻿using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace DH.Helpdesk.WebApi.Infrastructure.Authentication
{
    public static class ClaimsAccessHelpder
    {
        public static ClaimsIdentity GetClaims(this IIdentity identity)
        {
            return (ClaimsIdentity)identity;
        }

        /// <summary>
        /// Gets db id of current user
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        //public static int GetUserId(this IIdentity identity)
        //{
        //    var userIdStr = identity.GetClaims().GetUserId();

        //    if(string.IsNullOrWhiteSpace(userIdStr)) throw new Exception("No UserID claim found.");

        //    return int.Parse(userIdStr);
        //}

        //public static string GetUserName(this IIdentity identity)
        //{
        //    var userNameStr = identity.GetUserName();

        //    if(string.IsNullOrWhiteSpace(userNameStr)) throw new Exception("No UserName claim found.");

        //    return userNameStr;
        //}

        public static int GetGroupId(this IIdentity identity)
        {
            var userGroupIdStr = identity.GetClaims().FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(userGroupIdStr))
                throw new Exception("No UserGroupId/Role claim found.");

            return int.Parse(userGroupIdStr);
        }

        public static string GetTimezoneId(this IIdentity identity)
        {
            var timezoneId = identity.GetClaims().FindFirstValue(CustomClaimTypes.TimezoneId);

            if (string.IsNullOrWhiteSpace(timezoneId))
                throw new Exception("No timezoneId claim found.");

            return timezoneId;
        }
    }
}