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
        IEnumerable<Text> GetAllTexts();

        IList<TextTranslation> GetAllTextTranslations();
        IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid);
        IList<TextTranslationLanguageList> GetIndexListToTextTranslations();
        IList<TextTranslationList> GetNewListToTextTranslations();

        Text GetText(int id);
        TextTranslation GetTextTranslation(int id);

        void SaveEditText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors);
        void SaveNewText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors);
        void Commit();
    }

    public class TextTranslationService : ITextTranslationService
    {
        private readonly ITextRepository _textRepository;
        private readonly ITextTranslationRepository _textTranslationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TextTranslationService(
            ITextRepository textRepository,
            ITextTranslationRepository textTranslationRepository,
            IUnitOfWork unitOfWork)
        {
            this._textRepository = textRepository;
            this._textTranslationRepository = textTranslationRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Text> GetAllTexts()
        {
            return this._textRepository.GetAll().Where(x => x.Id > 4999);
        }

        public IList<TextTranslation> GetAllTextTranslations()
        {
            return this._textTranslationRepository.GetMany(x => x.Text_Id > 4999).ToList();
        }

        public IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid)
        {
            return this._textTranslationRepository.ReturnTTsListForEdit(textid).ToList();
        }

        public IList<TextTranslationLanguageList> GetIndexListToTextTranslations()
        {
            return this._textTranslationRepository.ReturnTTsListForIndex().ToList();
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

        public void SaveEditText(Text text, List<TextTranslationLanguageList> TTs, out IDictionary<string, string> errors)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text.ChangedDate = DateTime.Now;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");

            foreach (var tt in text.TextTranslations)
            {
                foreach (var change in TTs.Where(x => x.Text_Id == tt.Text_Id && x.Language_Id == tt.Language_Id))
                {
                    if (change.TranslationName != tt.TextTranslated)
                    {
                        tt.TextTranslated = change.TranslationName;
                        this._textTranslationRepository.Update(tt);
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

                foreach (var tt in text.TextTranslations)
                {
                    foreach (var change in TTs.Where(x => x.Text_Id == tt.Text_Id && x.Language_Id == tt.Language_Id))
                    {
                        tt.TextTranslated = change.TranslationName;
                        tt.Language_Id = change.Language_Id;
                        tt.Text_Id = change.Text_Id;

                        this._textTranslationRepository.Add(tt);
                    }
                }
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
