namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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

    #region TEXTTYPE

    public interface ITextTypeRepository : IRepository<TextType>
    {
        TextType GetTextTypeById(int id);  
    }

    public class TextTypeRepository : RepositoryBase<TextType>, ITextTypeRepository
    {
        public TextTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public TextType GetTextTypeById(int id)
        {
           
            return this.DataContext.TextTypes.Where(x => x.Id == id).FirstOrDefault();
           
        }
    }

    #endregion

    #region TEXTTRANSLATION

    public interface ITextTranslationRepository : IRepository<TextTranslation>
    {
        List<Translation2> FindTranslations();
        IEnumerable<TextTranslationLanguageList> ReturnTTsListForEdit(int textId);
        IEnumerable<TextTranslationLanguageList> ReturnTTsListForIndex(int languageId);
        IEnumerable<TextTranslationList> ReturnTTsListForNew();
        string GetTTByLanguageId(int textid, int languageId);
    }

    public class TextTranslationRepository : RepositoryBase<TextTranslation>, ITextTranslationRepository
    {
        public TextTranslationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public string GetTTByLanguageId(int textid, int languageId)
        {
            return this.DataContext.TextTranslations.Where(tt => tt.Text_Id == textid && tt.Language_Id == languageId).Select(tt => tt.TextTranslated).SingleOrDefault();
           

        }

        public List<Translation2> FindTranslations()
        {
            var texts = this.DataContext.Texts.Select(t => new { t.Id, t.TextToTranslate });

            var textTranslations =
                this.DataContext.TextTranslations.Select(t => new { t.Text_Id, t.Language_Id, t.TextTranslated });

            var translations =
                texts.Join(
                    textTranslations,
                    t => t.Id,
                    t => t.Text_Id,
                    (text, textTranslation) =>
                        new { text.TextToTranslate, textTranslation.Language_Id, textTranslation.TextTranslated })
                    .ToList();



            return
                translations.Select(
                    t =>
                        new Translation2(
                            t.TextToTranslate,
                            t.Language_Id == 1 ? "sv-SE" : t.Language_Id == 2 ? "en-US" : "de-DE",
                            t.TextTranslated)).ToList();
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

        public IEnumerable<TextTranslationLanguageList> ReturnTTsListForIndex(int languageId)
        {
            var query = from l in this.DataContext.Languages
                        join tt in this.DataContext.TextTranslations on l.Id equals tt.Language_Id
                        where l.IsActive == 1 && tt.Text_Id > 4999 && tt.Language_Id == languageId
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
                            Language_Id = g.Key.Id,
                            LanguageName = g.Key.Name
                        };

            return query;
        }
    }

    #endregion
}
