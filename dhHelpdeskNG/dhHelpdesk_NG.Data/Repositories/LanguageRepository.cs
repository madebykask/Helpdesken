namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILanguageRepository : IRepository<Language>
    {
        string GetLanguageTextIdById(int languageId);

        List<ItemOverview> FindActive();
    }

	public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
	{
		public LanguageRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public string GetLanguageTextIdById(int languageId)
	    {
	        return this.DataContext.Languages.Find(languageId).LanguageID;
	    }

	    public List<ItemOverview> FindActive()
	    {
	        var languageOverviews =
	            this.DataContext.Languages.Where(l => l.IsActive != 0).Select(l => new { l.Id, l.Name }).ToList();

	        return
	            languageOverviews.Select(l => new ItemOverview(l.Name, l.Id.ToString(CultureInfo.InvariantCulture)))
	                .ToList();
	    }
	}
}
