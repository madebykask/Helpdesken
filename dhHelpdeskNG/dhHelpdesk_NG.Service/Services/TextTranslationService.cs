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
        IEnumerable<Text> GetAllTexts(int texttypeId);

        IList<TextTranslation> GetAllTextTranslations();
        IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid);
        IList<TextTranslationLanguageList> GetIndexListToTextTranslations(int languageId);
        IList<TextTranslationList> GetNewListToTextTranslations();
  

        Text GetText(int id);
        TextTranslation GetTextTranslation(int id);
        TextTranslation GetTextTranslationByIdAndLanguage(int id, int languageid);
        TextTranslation GetTextTranslationById(int id);
        string GetTextTranslationByLanguage(int id, int langaugeid);

        IList<TextType> GetTextTypes();
        TextType GetTextType(int id);

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

        public IEnumerable<Text> GetAllTexts(int texttypeId)
        {
            return this._textRepository.GetAll().Where(x => x.Id > 4999 && x.Type == texttypeId).OrderBy(x => x.TextToTranslate);
        }

        public IList<TextTranslation> GetAllTextTranslations()
        {
            return this._textTranslationRepository.GetMany(x => x.Text_Id > 4999).ToList();
        }

        public IList<TextType> GetTextTypes()
        {
            return this._textTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public TextType GetTextType(int id)
        {
            return this._textTypeRepository.GetById(id);
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

            text.ChangedDate = DateTime.Now;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");


            if (text.TextTranslations.Count == 0)
            {
                foreach (var t in TTs)
                {
                    if (t.TranslationName == null)
                        t.TranslationName = "";

                    var newTT = new TextTranslation { TextTranslated = t.TranslationName, Language_Id = t.Language_Id, Text_Id = text.Id };

                    this._textTranslationRepository.Add(newTT);
                }
            }
            else
            {
                foreach (var tt in text.TextTranslations)
                {

                    foreach (var change in TTs.Where(x => x.Text_Id == tt.Text_Id && x.Language_Id == tt.Language_Id))
                    {
                        if (change.TranslationName == null)
                            change.TranslationName = "";

                        if (change.TranslationName != tt.TextTranslated)
                        {
                            tt.TextTranslated = change.TranslationName;
                            this._textTranslationRepository.Update(tt);
                        }
                    }
                }
            }

            if (text.Id == 0)
                this._textRepository.Add(text);
            else
                this._textRepository.Update(text);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveNewText(Text text, out IDictionary<string, string> errors)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            errors = new Dictionary<string, string>();

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
            
            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");


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
    }
}
