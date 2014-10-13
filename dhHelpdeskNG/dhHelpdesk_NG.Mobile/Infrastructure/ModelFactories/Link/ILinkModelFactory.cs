using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.Mobile.Models.Link;

namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Link
{
    public interface ILinkModelFactory
    {
        LinksInfoViewModel GetLinksViewModel(IEnumerable<LinkOverview> linkOverviews);
    }
}