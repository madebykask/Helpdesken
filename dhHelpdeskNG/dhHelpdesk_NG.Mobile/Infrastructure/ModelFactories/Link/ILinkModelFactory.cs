using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.Web.Models.Link;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Link
{
    public interface ILinkModelFactory
    {
        LinksInfoViewModel GetLinksViewModel(IEnumerable<LinkOverview> linkOverviews);
    }
}