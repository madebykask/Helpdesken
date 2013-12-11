using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IInfoService
    {
        IList<InfoText> GetInfoTexts(int customerId, int languageId);

        InfoText GetInfoText(int id, int customerId, int languageId);

        void SaveInfoText(InfoText infoText, out IDictionary<string, string> errors);
        void Commit();
    }

    public class InfoService : IInfoService
    {
        private readonly IInfoTextRepository _infoTextRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InfoService(
            IInfoTextRepository infoTextRepository,
            IUnitOfWork unitOfWork)
        {
            _infoTextRepository = infoTextRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<InfoText> GetInfoTexts(int customerId, int languageId)
        {
            return _infoTextRepository.GetMany(x => x.Customer_Id == customerId && x.Language_Id == languageId).ToList();
        }

        public InfoText GetInfoText(int id, int customerId, int languageId)
        {
            return _infoTextRepository.Get(x => x.Type == id && x.Language_Id == languageId && x.Customer_Id == customerId);
        }

        public void SaveInfoText(InfoText infoText, out IDictionary<string, string> errors)
        {
            if (infoText == null)
                throw new ArgumentNullException("infotext");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(infoText.Name))
                errors.Add("InfoText.Name", "Du måste ange en informationstext");
            
            infoText.Name = infoText.Name.Replace(System.Environment.NewLine, "<br />");
            infoText.Name = Guard.Sanitize(infoText.Name);
            infoText.ChangedDate = DateTime.UtcNow;

            if (infoText.Id == 0)
                _infoTextRepository.Add(infoText);
            else
                _infoTextRepository.Update(infoText);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}