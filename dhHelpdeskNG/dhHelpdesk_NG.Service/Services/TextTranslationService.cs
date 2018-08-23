namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ITextTranslationService
    {
        IEnumerable<Text> GetAllNewTexts(int texttypeId);

        IEnumerable<TextList> GetAllTexts(int texttypeId, int? defaultLanguage);
        List<TextList> GetAllTextsAndTranslations(int texttypeId);
        //IList<Text> SearchAndGenerateTexts(TranslationSearch SearchTranslation);
        IList<TextTranslation> GetAllTextTranslations();
        IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid);
        IList<TextTranslationLanguageList> GetIndexListToTextTranslations(int languageId);
        IList<TextTranslationList> GetNewListToTextTranslations();
        IList<CustomKeyValue<string, string>> GetTranslationsFor(IList<string> texts, int languageId);
        IList<CustomKeyValue<string, string>> GetTextTranslationsFor(int languageId, int typeId = 0);

        Text GetText(int id);
        TextTranslation GetTextTranslation(int id);
        TextTranslation GetTextTranslationByIdAndLanguage(int id, int languageid);
        TextTranslation GetTextTranslationById(int id);
        string GetTextTranslationByLanguage(int id, int langaugeid);

        IList<TextType> GetTextTypes();
        IList<TextType> GetTextTypesForNewText();
        TextType GetTextType(int id);
        String GetTextTypeName(int id);
        


        void SaveEditText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors);
        void SaveEditText(Text text, TextTranslation texttranslation, bool update, out IDictionary<string, string> errors);
        void SaveNewText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors);
        void SaveNewText(Text text, out IDictionary<string, string> errors);
        void DeleteTextTranslation(TextTranslation texttranslation, out IDictionary<string, string> errors);
        void DeleteText(Text text, out IDictionary<string, string> errors);
        void Commit();
    }

    public class TextTranslationService : ITextTranslationService
    {
        private readonly ITextRepository _textRepository;
        private readonly ITextTranslationRepository _textTranslationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITextTypeRepository _textTypeRepository;

        public TextTranslationService(
            ITextRepository textRepository,
            ITextTranslationRepository textTranslationRepository,
            ITextTypeRepository textTypeRepository,
            IUnitOfWork unitOfWork)
        {
            this._textRepository = textRepository;
            this._textTranslationRepository = textTranslationRepository;
            this._unitOfWork = unitOfWork;
            this._textTypeRepository = textTypeRepository;
        }

        public IEnumerable<Text> GetAllNewTexts(int texttypeId)
        {
            return this._textRepository.GetAll().AsQueryable().Where(x => x.Id > 4999 && x.Type == texttypeId).OrderBy(x => x.TextToTranslate);
        }

        public IEnumerable<TextList> GetAllTexts(int texttypeId, int? defaultLanguage)
        {
            return this._textRepository.GetAllTexts(texttypeId, defaultLanguage).OrderBy(x => x.TextToTranslate);
        }

        public List<TextList> GetAllTextsAndTranslations(int texttypeId)
        {
            return this._textRepository.GetAllTextsAndTranslations(texttypeId);
        }

        public IList<TextTranslation> GetAllTextTranslations()
        {
            return this._textTranslationRepository.GetMany(x => x.Text_Id > 4999).ToList();
        }

        public IList<TextType> GetTextTypes()
        {
            return this._textTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<TextType> GetTextTypesForNewText()
        {
            return this._textTypeRepository.GetMany(x => x.Id > 0).OrderBy(x => x.Name).ToList();
        }

        public TextType GetTextType(int id)
        {
            return this._textTypeRepository.GetById(id);
        }

        public String GetTextTypeName(int id)
        {
            return this._textTypeRepository.GetTextTypeName(id);
        }

        public IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid)
        {
            return this._textTranslationRepository.ReturnTTsListForEdit(textid).ToList();
        }

        public IList<TextTranslationLanguageList> GetIndexListToTextTranslations(int languageId)
        {
            return this._textTranslationRepository.ReturnTTsListForIndex(languageId).ToList();
        }

        public IList<TextTranslationList> GetNewListToTextTranslations()
        {
            return this._textTranslationRepository.ReturnTTsListForNew().ToList();
        }

        public Text GetText(int id)
        {
            return this._textRepository.Get(x => x.Id == id);
        }

        public TextTranslation GetTextTranslation(int id)
        {
            return this._textTranslationRepository.GetById(id);
        }

        public string GetTextTranslationByLanguage(int id, int languageid)
        {
            return this._textTranslationRepository.GetTTByLanguageId(id, languageid);
        }

        public TextTranslation GetTextTranslationByIdAndLanguage(int id, int languageId)
        {
            return this._textTranslationRepository.GetTTByIdAndLanguageId(id, languageId);
        }

        public TextTranslation GetTextTranslationById(int id)
        {
            return this._textTranslationRepository.GetTTById(id);
        }

        public void SaveEditText(Text text, TextTranslation textranslation, bool update, out IDictionary<string, string> errors)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text.ChangedDate = DateTime.Now;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");


            //if (textranslation != null)
            //{
            //    this._textTranslationRepository.Add(textranslation);
            //}

            if (!update)
            {
                this._textTranslationRepository.Add(textranslation);
            }
            else
            {
                this._textTranslationRepository.Update(textranslation);

            }
           
            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveEditText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var curNow = DateTime.Now;

            text.ChangedDate = curNow;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");

            foreach (var tt in TTs)
            {
                if (tt.TranslationName == null)
                    tt.TranslationName = "";

                var ttEntity = new TextTranslation()
                    {
                        TextTranslation_Id = tt.TextTranslation_Id,
                        Text_Id = text.Id,
                        Language_Id = tt.Language_Id,
                        TextTranslated = tt.TranslationName,
                        ChangedByUser_Id = tt.ChangedByUser_Id,
                        CreatedDate = tt.CreatedDate,
                        ChangedDate = curNow
                    };

                if (ttEntity.TextTranslation_Id == 0)
                {
                    ttEntity.CreatedDate = curNow;
                    ttEntity.TextTranslation_Id = 0;
                    this._textTranslationRepository.Add(ttEntity);

                }
                else
                    this._textTranslationRepository.Update(ttEntity);                    
            }           
                    
            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveNewText(Text text, out IDictionary<string, string> errors)
        {
            
            if (text == null)
                throw new ArgumentNullException("text");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");

            var hasDublicate = this.GetAllTexts(text.Type, null)
                            .Any(t => t.TextToTranslate.Equals(text.TextToTranslate));

            if (hasDublicate)
            {
                errors.Add("text.TextToTranslate", "Ordet finns redan.");

            }
            else
            {
                text.ChangedDate = DateTime.Now;
                text.CreatedDate = DateTime.Now;

                text.Id = this._textRepository.GetNextId() + 1;

                if (text.Id < 5000)
                {
                    text.Id = 5000;
                }
                else
                {
                    text.Id = text.Id + 1;
                }
            }


            this._textRepository.Add(text);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveNewText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            errors = new Dictionary<string, string>();

            text.ChangedDate = DateTime.Now;
            text.CreatedDate = DateTime.Now;
            text.Id = this._textRepository.GetNextId() + 1;
            
            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");

            if (text != null)
            {
                this._textRepository.AddText(text);

                foreach (var t in TTs)
                {
                    var newTT = new TextTranslation { TextTranslated = t.TranslationName, Language_Id = t.Language_Id, Text_Id = text.Id };

                    this._textTranslationRepository.Add(newTT);

                }
             
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void DeleteTextTranslation(TextTranslation texttranslation, out IDictionary<string, string> errors)
        {

            errors = new Dictionary<string, string>();


            if (texttranslation.Language_Id != 0 && texttranslation.Text_Id != 0)
                this._textTranslationRepository.Delete(texttranslation);

            if (errors.Count == 0)
                this.Commit();
        }

        public void DeleteText(Text text, out IDictionary<string, string> errors)
        {

            errors = new Dictionary<string, string>();

            if (text != null)
                this._textRepository.Delete(text);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IList<CustomKeyValue<string, string>> GetTranslationsFor(IList<string> texts, int languageId)
        {
            //var preparedTexts = texts.Select(t => t.ToLower());
            return _textTranslationRepository.GetTranslationsFor(texts, languageId);
        }

        public IList<CustomKeyValue<string, string>> GetTextTranslationsFor(int languageId, int typeId = 0)
        {
            return _textTranslationRepository.GetTextTranslationsFor(languageId, typeId);
        }
    }
}
