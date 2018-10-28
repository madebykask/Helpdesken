using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region TEXT

    public interface ITextRepository : IRepository<Text>
    {
        int GetNextId();
        IEnumerable<Text> GetAllWithTranslation();
        IEnumerable<TextList> GetAllTexts(int texttypeId, int? defaultLanguage);
        List<TextList> GetAllTextsAndTranslations(int texttypeId);
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

        public List<TextList> GetAllTextsAndTranslations(int texttypeId)
        {
            var textEntity = this.DataContext.Texts.Where(t => t.Type == texttypeId)
                        .Select(t => new TextList
                        {
                            Id = t.Id,
                            TextToTranslate = t.TextToTranslate,
                            Translations = t.TextTranslations.Select(tt=> 
                                new TextTranlationsTextLanguageList
                                {
                                    Text_Id= tt.Text_Id, 
                                    Language_Id = tt.Language_Id, 
                                    TranslationName=tt.TextTranslated, 
                                    TranslationText_Id = tt.TextTranslation_Id
                                }).ToList()
                        }

                );            
            return textEntity.ToList();

        }
        public IEnumerable<TextList> GetAllTexts(int texttypeId, int? defaultLanguage)
        {
            //Might not be needed in future since we have texttype. Earlier versions of DH Helpdesk assigned all core system phrases to id below 5000.
            const int CoreSystemPhrases = 4999;

            IEnumerable<TextList> txt = null;

            if (defaultLanguage == null)
            {
                txt =
                   from T in this.DataContext.Texts
                   join TT in this.DataContext.TextTranslations on T.Id equals TT.Text_Id into Translate
                   from Trans in Translate.DefaultIfEmpty()
                   join U1 in this.DataContext.Users on Trans.ChangedByUser_Id equals U1.Id into Users1
                   from User1 in Users1.DefaultIfEmpty()
                   join U2 in this.DataContext.Users on T.ChangedByUser_Id equals U2.Id into Users2
                   from User2 in Users2.DefaultIfEmpty()
                   where (T.Type == texttypeId)  //&& T.Id > CoreSystemPhrases)
                   group T by new
                   {
                       T.Id,
                       T.TextToTranslate,
                       User2.SurName,
                       User2.FirstName,
                       Trans.ChangedDate,
                       T.CreatedDate,
                       U1Name = User1.FirstName,
                       U1SurName = User1.SurName,
                       Translation = Trans,
                       Trans.TextTranslated
                   } into g
                   select new TextList
                   {
                       Id = g.Key.Id,
                       TextToTranslate = g.Key.TextToTranslate,
                       CreatedByFirstName = g.Key.FirstName,
                       CreatedByLastName = g.Key.SurName,
                       ChangedDate = g.Key.ChangedDate,
                       CreatedDate = g.Key.CreatedDate,
                       ChangedByFirstName = g.Key.U1Name,
                       ChangedByLastName = g.Key.U1SurName,
                       TextTranslated = g.Key.TextTranslated
                   };
            }
            else
            {
                txt =
                       from T in this.DataContext.Texts
                       join TT in this.DataContext.TextTranslations.Where(x=> x.Language_Id == defaultLanguage) on T.Id equals TT.Text_Id  into Translate
                       from Trans in Translate.DefaultIfEmpty()                       
                       join U1 in this.DataContext.Users on Trans.ChangedByUser_Id equals U1.Id into Users1
                       from User1 in Users1.DefaultIfEmpty()
                       join U2 in this.DataContext.Users on T.ChangedByUser_Id equals U2.Id into Users2
                       from User2 in Users2.DefaultIfEmpty()
                       where (T.Type == texttypeId)  //&& T.Id > CoreSystemPhrases)
                       group T by new
                       {
                           T.Id,
                           T.TextToTranslate,
                           User2.SurName,
                           User2.FirstName,
                           Trans.ChangedDate,
                           T.CreatedDate,
                           U1Name = User1.FirstName,
                           U1SurName = User1.SurName,
                           Translation = Trans,
                           Trans.TextTranslated
                       } into g
                       select new TextList
                       {
                           Id = g.Key.Id,
                           TextToTranslate = g.Key.TextToTranslate,
                           CreatedByFirstName = g.Key.FirstName,
                           CreatedByLastName = g.Key.SurName,
                           ChangedDate = g.Key.ChangedDate,
                           CreatedDate = g.Key.CreatedDate,
                           ChangedByFirstName = g.Key.U1Name,
                           ChangedByLastName = g.Key.U1SurName,
                           TextTranslated = g.Key.TextTranslated
                       };
            }
            var txtToReturn = txt.GroupBy(text => text.Id).Select(grp => grp.FirstOrDefault()).ToList();

            return txtToReturn.ToList();

        }
    }

    #endregion

    #region TEXTTYPE

    public interface ITextTypeRepository : IRepository<TextType>
    {
        TextType GetTextTypeById(int id);
        string GetTextTypeName(int id);
        TextType GetTextTypeByName(string name);
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

        public string GetTextTypeName(int id)
        {
            return this.DataContext.TextTypes.Where(x => x.Id == id).Select(tt => tt.Name).SingleOrDefault();
        }

        public TextType GetTextTypeByName(string name)
        {
            return Table.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
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

        TextTranslation GetTTByIdAndLanguageId(int textid, int langaugeId);
        TextTranslation GetTTById(int textid);
        List<TextTranslationLanguageList> GetTextTranslationByTextId(int textId);
        IList<CustomKeyValue<string, string>> GetTranslationsFor(IList<string> texts, int languageId);
        IList<CustomKeyValue<string, string>> GetTextTranslationsFor(int languageId, int textTypeId = 0);
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

        public TextTranslation GetTTByIdAndLanguageId(int textid, int languageId)
        {

            TextTranslation tt = (from t in this.DataContext.Set<TextTranslation>()
                                  where t.Text_Id == textid && t.Language_Id == languageId
                                  select t).FirstOrDefault();


            return tt;
        }

        public TextTranslation GetTTById(int textid)
        {

            TextTranslation tt = (from t in this.DataContext.Set<TextTranslation>()
                                  where t.Text_Id == textid
                                  select t).FirstOrDefault();


            return tt;
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
                        group l by new { l.Id, l.Name, tt.TextTranslated, textId, tt.TextTranslation_Id, tt.CreatedDate } into g
                        select new TextTranslationLanguageList
                        {
                            Language_Id = g.Key.Id,
                            LanguageName = g.Key.Name,
                            Text_Id = g.Key.textId,
                            TextTranslation_Id = g.Key.TextTranslation_Id,
                            TranslationName = g.Key.TextTranslated,
                            CreatedDate = g.Key.CreatedDate
                        };

            return query.OrderBy(x => x.Text_Id);
        }

        public List<TextTranslationLanguageList> GetTextTranslationByTextId(int textId)
        {
            var query = from tt in this.DataContext.TextTranslations
                        where tt.Text_Id == textId
                        group tt by new { tt.TextTranslation_Id, tt.TextTranslated, tt.Text_Id } into g
                        select new TextTranslationLanguageList
                        {
                            Text_Id = g.Key.Text_Id,
                            TextTranslation_Id = g.Key.TextTranslation_Id,
                            TranslationName = g.Key.TextTranslated,
                        };

            return query.ToList();
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
                        //join tt in this.DataContext.TextTranslations on l.Id equals tt.Language_Id
                        where l.IsActive == 1 //&& tt.Text_Id > 4999
                        group l by new { l.Name, l.Id, } into g
                        select new TextTranslationList
                        {
                            Language_Id = g.Key.Id,
                            LanguageName = g.Key.Name
                        };

            return query;
        }

        public IList<CustomKeyValue<string, string>> GetTranslationsFor(IList<string> texts, int languageId)
        {
            var query = from t in DataContext.Texts
                        join tt in DataContext.TextTranslations on t.Id equals tt.Text_Id
                        where tt.Text_Id > 4999 && tt.Language_Id == languageId && texts.Contains(t.TextToTranslate)                        
                        select new CustomKeyValue<string, string>()
                        {
                            Key = t.TextToTranslate,
                            Value = tt.TextTranslated
                        };

            return query.ToList();
        }

        public IList<CustomKeyValue<string, string>> GetTextTranslationsFor(int languageId, int textTypeId = 0)
        {
            var query = from t in DataContext.Texts
                         join tt in DataContext.TextTranslations on new { key1 = t.Id, key2 = languageId } equals new { key1 = tt.Text_Id, key2 = tt.Language_Id } into gr
                         from res in gr.DefaultIfEmpty()
                         where t.Type == textTypeId
                         select new CustomKeyValue<string, string>()
                         {
                             Key = t.TextToTranslate,
                             Value = res.TextTranslated ?? t.TextToTranslate
                         };

            return query.ToList();
        }
    }

    #endregion
}
