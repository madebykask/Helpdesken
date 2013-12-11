using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region TEXT

    public interface ITextRepository : IRepository<Text>
    {
        int GetNextId();
        IEnumerable<Text> GetAllWithTranslation();
    }

    public class TextRepository : RepositoryBase<Text>, ITextRepository
    {
        public TextRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public int GetNextId()
        {
            var query = (from t in this.DataContext.Texts
                         select t.Id).Max();

            return query++;
        }

        public IEnumerable<Text> GetAllWithTranslation()
        {
            return this.DataContext.Texts.Include("TextTranslations");
        }
    }

    #endregion

    #region TEXTTRANSLATION

    public interface ITextTranslationRepository : IRepository<TextTranslation>
    {
        IEnumerable<TextTranslationLanguageList> ReturnTTsListForEdit(int textId);
        IEnumerable<TextTranslationLanguageList> ReturnTTsListForIndex();
        IEnumerable<TextTranslationList> ReturnTTsListForNew();
    }

    public class TextTranslationRepository : RepositoryBase<TextTranslation>, ITextTranslationRepository
    {
        public TextTranslationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<TextTranslationLanguageList> ReturnTTsListForEdit(int textId)
        {
            var query = from l in this.DataContext.Languages
                        join tt in this.DataContext.TextTranslations on l.Id equals tt.Language_Id
                        join t in this.DataContext.Texts on textId equals t.Id
                        where l.IsActive == 1 && tt.Text_Id == t.Id
                        group l by new { l.Id, l.Name, tt.TextTranslated, textId, tt.TextTranslation_Id } into g
                        select new TextTranslationLanguageList
                        {
                            Language_Id = g.Key.Id,
                            LanguageName = g.Key.Name,
                            Text_Id = g.Key.textId,
                            TextTranslation_Id = g.Key.TextTranslation_Id,
                            TranslationName = g.Key.TextTranslated,
                        };

            return query.OrderBy(x => x.Text_Id);
        }

        public IEnumerable<TextTranslationLanguageList> ReturnTTsListForIndex()
        {
            var query = from l in this.DataContext.Languages
                        join tt in this.DataContext.TextTranslations on l.Id equals tt.Language_Id
                        where l.IsActive == 1 && tt.Text_Id > 4999
                        group l by new { l.Id, l.Name, tt.TextTranslated, tt.Text_Id } into g
                        select new TextTranslationLanguageList
                        {
                            Language_Id = g.Key.Id,
                            LanguageName = g.Key.Name,
                            Text_Id = g.Key.Text_Id,
                            TranslationName = g.Key.TextTranslated,
                        };

            return query.OrderBy(x => x.Text_Id);
        }

        public IEnumerable<TextTranslationList> ReturnTTsListForNew()
        {
            var query = from l in this.DataContext.Languages
                        join tt in this.DataContext.TextTranslations on l.Id equals tt.Language_Id
                        where l.IsActive == 1 && tt.Text_Id > 4999
                        group l by new { l.Name, l.Id, } into g
                        select new TextTranslationList
                        {
                            LanguageName = g.Key.Name
                        };

            return query;
        }
    }

    #endregion
}
