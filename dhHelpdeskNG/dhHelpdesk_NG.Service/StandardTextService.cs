using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;

        public StandardTextService(
            IStandardTextRepository standardTextRepository,
            IUnitOfWork unitOfWork)
        {
            _standardTextRepository = standardTextRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<StandardText> GetStandardTexts(int customerId)
        {
            return _standardTextRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public StandardText GetStandardText(int id)
        {
            return _standardTextRepository.GetById(id);
        }

        public DeleteMessage DeleteStandardText(int id)
        {
            var standardText = _standardTextRepository.GetById(id);

            if (standardText != null)
            {
                try
                {
                    _standardTextRepository.Delete(standardText);
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

            standardText.Name = Guard.Sanitize(standardText.Name);

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(standardText.Name))
                errors.Add("StandardText.Name", "Du måste ange en standardtext");

            if (standardText.Id == 0)
                _standardTextRepository.Add(standardText);
            else
                _standardTextRepository.Update(standardText);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
