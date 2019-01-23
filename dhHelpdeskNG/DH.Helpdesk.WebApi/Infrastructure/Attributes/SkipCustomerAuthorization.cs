using System;

namespace DH.Helpdesk.WebApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipCustomerAuthorization : Attribute
    {
    }
}