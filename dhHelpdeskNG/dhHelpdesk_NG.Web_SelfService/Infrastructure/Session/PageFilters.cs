using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.SelfService.Infrastructure.Session
{
    public sealed class PageFilters
    {
        public PageFilters(string pageName, object filters)
        {
            PageName = pageName;
            Filters = filters;
        }

        [NotNullAndEmpty]
        public string PageName { get; private set; }

        [NotNull]
        public object Filters { get; private set; }
    }
}