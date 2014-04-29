namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILanguageRepository : IRepository<Language>
    {
        string GetLanguageTextIdById(int languageId);

        List<ItemOverview> FindActive();

        /// <summary>
        /// The get active languages.
        /// </summary>
        /// <returns>
        /// The languages.
        /// </returns>
        IEnumerable<LanguageOverview> GetActiveLanguages();
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

	    /// <summary>
	    /// The get active languages.
	    /// </summary>
	    /// <returns>
	    /// The result.
	    /// </returns>
	    public IEnumerable<LanguageOverview> GetActiveLanguages()
	    {
	        return this.GetAll()
                .ToList()
                .Select(l => new LanguageOverview()
                                 {
                                     Id = l.Id,
                                     IsActive = l.IsActive.ToBool(),
                                     LanguageId = l.LanguageID,
                                     Name = l.Name
                                 })
                .Where(l => l.IsActive)
                .OrderBy(l => l.Name);
	    }
	}
}
