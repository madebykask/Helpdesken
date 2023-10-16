namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IInfoService
    {
        IList<InfoText> GetInfoTexts(int customerId, int languageId);

        InfoText GetInfoText(int id, int customerId, int languageId);

        void SaveInfoText(InfoText infoText, out IDictionary<string, string> errors);
        void Commit();
        IList<InfoText> GetAllInfoTexts(int customerId);
    }

    public class InfoService : IInfoService
    {
        private readonly IInfoTextRepository _infoTextRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public InfoService(
            IInfoTextRepository infoTextRepository,
            IUnitOfWork unitOfWork)
        {
            this._infoTextRepository = infoTextRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<InfoText> GetInfoTexts(int customerId, int languageId)
        {
            return this._infoTextRepository.GetMany(x => x.Customer_Id == customerId && x.Language_Id == languageId).ToList();
        }
        public IList<InfoText> GetAllInfoTexts(int customerId)
        {
            return this._infoTextRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public InfoText GetInfoText(int id, int customerId, int languageId)
        {
            return this._infoTextRepository.Get(x => x.Type == id && x.Language_Id == languageId && x.Customer_Id == customerId);
        }

        public void SaveInfoText(InfoText infoText, out IDictionary<string, string> errors)
        {
            if (infoText == null)
                throw new ArgumentNullException("infotext");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(infoText.Name))
                errors.Add("InfoText.Name", "Du måste ange en informationstext");
            
            //infoText.Name = infoText.Name.Replace(global::System.Environment.NewLine, "<br />"); 
            infoText.Name = Guard.Sanitize(infoText.Name);
            infoText.ChangedDate = DateTime.UtcNow;

            if (infoText.Id == 0)
                this._infoTextRepository.Add(infoText);
            else
                this._infoTextRepository.Update(infoText);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}