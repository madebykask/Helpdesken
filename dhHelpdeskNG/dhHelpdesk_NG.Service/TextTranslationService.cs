using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
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
            _textRepository = textRepository;
            _textTranslationRepository = textTranslationRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Text> GetAllTexts()
        {
            return _textRepository.GetAll().Where(x => x.Id > 4999);
        }

        public IList<TextTranslation> GetAllTextTranslations()
        {
            return _textTranslationRepository.GetMany(x => x.Text_Id > 4999).ToList();
        }

        public IList<TextTranslationLanguageList> GetEditListToTextTranslations(int textid)
        {
            return _textTranslationRepository.ReturnTTsListForEdit(textid).ToList();
        }

        public IList<TextTranslationLanguageList> GetIndexListToTextTranslations()
        {
            return _textTranslationRepository.ReturnTTsListForIndex().ToList();
        }

        public IList<TextTranslationList> GetNewListToTextTranslations()
        {
            return _textTranslationRepository.ReturnTTsListForNew().ToList();
        }

        public Text GetText(int id)
        {
            return _textRepository.Get(x => x.Id == id);
        }

        public TextTranslation GetTextTranslation(int id)
        {
            return _textTranslationRepository.GetById(id);
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
                        _textTranslationRepository.Update(tt);
                    }
                }
            }

            if (text.Id == 0)
                _textRepository.Add(text);
            else
                _textRepository.Update(text);

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
            text.Id = _textRepository.GetNextId() + 1;

            if (string.IsNullOrEmpty(text.TextToTranslate))
                errors.Add("text.TextToTranslate", "Du måste ange ett ord att översätta");

            if (text != null)
            {
                _textRepository.AddText(text);

                foreach (var tt in text.TextTranslations)
                {
                    foreach (var change in TTs.Where(x => x.Text_Id == tt.Text_Id && x.Language_Id == tt.Language_Id))
                    {
                        tt.TextTranslated = change.TranslationName;
                        tt.Language_Id = change.Language_Id;
                        tt.Text_Id = change.Text_Id;

                        _textTranslationRepository.Add(tt);
                    }
                }
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
