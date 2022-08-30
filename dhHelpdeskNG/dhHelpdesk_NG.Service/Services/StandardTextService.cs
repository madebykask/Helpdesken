namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IStandardTextService
    {
        IList<StandardText> GetStandardTexts(int customerId);

        StandardText GetStandardText(int id);

        DeleteMessage DeleteStandardText(int id);

        void SaveStandardText(StandardText standardText, out IDictionary<string, string> errors);
        void Commit();
    }

    public class StandardTextService : IStandardTextService
    {
        private readonly IStandardTextRepository _standardTextRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public StandardTextService(
            IStandardTextRepository standardTextRepository,
            IUnitOfWork unitOfWork)
        {
            this._standardTextRepository = standardTextRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<StandardText> GetStandardTexts(int customerId)
        {
            return this._standardTextRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.StandardTextName).ToList();
        }

        public StandardText GetStandardText(int id)
        {
            return this._standardTextRepository.GetById(id);
        }

        public DeleteMessage DeleteStandardText(int id)
        {
            var standardText = this._standardTextRepository.GetById(id);

            if (standardText != null)
            {
                try
                {
                    this._standardTextRepository.Delete(standardText);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveStandardText(StandardText standardText, out IDictionary<string, string> errors)
        {
            if (standardText == null)
                throw new ArgumentNullException("standardtext");

            standardText.Text = Guard.Sanitize(standardText.Text);

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(standardText.Text))
                errors.Add("StandardText.Name", "Du måste ange en standardtext");
            if (string.IsNullOrEmpty(standardText.StandardTextName))
                errors.Add("StandardText.StandardTextName", "Du måste ange en standardtext");

            if (standardText.Id == 0)
                this._standardTextRepository.Add(standardText);
            else
                this._standardTextRepository.Update(standardText);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
