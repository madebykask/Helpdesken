// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ILanguageRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The LanguageRepository interface.
    /// </summary>
    public interface ILanguageRepository : IRepository<Language>
    {
        List<ItemOverview> FindActiveOverviewsByIds(List<int> languageIds);

        /// <summary>
        /// The get language text id by id.
        /// </summary>
        /// <param name="languageId">
        /// The language id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetLanguageTextIdById(int languageId);
        int GetLanguageIdByText(string languageName);



        /// <summary>
        /// The find active.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        List<ItemOverview> FindActiveOverviews();

        /// <summary>
        /// The get active languages.
        /// </summary>
        /// <returns>
        /// The languages.
        /// </returns>
        IEnumerable<LanguageOverview> GetActiveLanguages();
    }

    /// <summary>
    /// The language repository.
    /// </summary>
    public sealed class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public LanguageRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindActiveOverviewsByIds(List<int> languageIds)
        {
            return
                this.DataContext.Languages.Where(l => languageIds.Contains(l.Id) && l.IsActive != 0)
                    .Select(l => new { l.Id, l.Name })
                    .ToList()
                    .Select(l => new ItemOverview(l.Name, l.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        /// <summary>
        /// The get language text id by id.
        /// </summary>
        /// <param name="languageId">
        /// The language id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetLanguageTextIdById(int languageId)
        {
            return this.DataContext.Languages.Find(languageId).LanguageID;
        }


        
        /// <summary>
        /// The find active.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public List<ItemOverview> FindActiveOverviews()
        {
            var languageOverviews = this.DataContext.Languages
                .Where(l => l.IsActive != 0)
                .Select(l => new { l.Id, l.Name }).ToList();

            return languageOverviews
                .Select(l => new ItemOverview(l.Name, l.Id.ToString(CultureInfo.InvariantCulture)))
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
            var entities = this.Table
                    .Where(l => l.IsActive > 0)
                    .OrderBy(l => l.Name)
                    .ToList();
                
            return entities
                .Select(l => new LanguageOverview
                        {
                            Id = l.Id,
                            IsActive = l.IsActive,
                            LanguageId = l.LanguageID,
                            Name = l.Name
                        });
        }

        public int GetLanguageIdByText(string languageName)
        {
            return this.DataContext.Languages.Find(languageName).Id;
        }
    }
}
