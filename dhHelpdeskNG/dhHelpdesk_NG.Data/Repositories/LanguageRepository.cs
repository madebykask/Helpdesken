using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public interface ILanguageRepository : IRepository<Language>
    {
        string FindLanguageIdById(int languageId);

        List<ItemOverviewDto> FindActive();
    }

	public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
	{
		public LanguageRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public string FindLanguageIdById(int languageId)
	    {
	        return this.DataContext.Languages.Find(languageId).LanguageID;
	    }

	    public List<ItemOverviewDto> FindActive()
	    {
	        var languageOverviews =
	            this.DataContext.Languages.Where(l => l.IsActive != 0).Select(l => new { l.Id, l.Name }).ToList();

	        return
	            languageOverviews.Select(
	                l => new ItemOverviewDto { Name = l.Name, Value = l.Id.ToString(CultureInfo.InvariantCulture) })
	                             .ToList();
	    }
	}
}
