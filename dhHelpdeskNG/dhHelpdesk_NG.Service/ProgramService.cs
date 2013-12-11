using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IProgramService
    {
        IList<Program> GetPrograms(int customerId);

        Program GetProgram(int id);

        DeleteMessage DeleteProgram(int id);

        void SaveProgram(Program program, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramService(
            IProgramRepository programRepository,
            IUnitOfWork unitOfWork)
        {
            _programRepository = programRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Program> GetPrograms(int customerId)
        {
            return _programRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Program GetProgram(int id)
        {
            return _programRepository.GetById(id);
        }

        public DeleteMessage DeleteProgram(int id)
        {
            var program = _programRepository.GetById(id);

            if (program != null)
            {
                try
                {
                    _programRepository.Delete(program);
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

        public void SaveProgram(Program program, out IDictionary<string, string> errors)
        {
            if (program == null)
                throw new ArgumentNullException("program");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(program.Name))
                errors.Add("Program.Name", "Du måste ange ett program");

            program.ChangedDate = DateTime.UtcNow;

            if (program.Id == 0)
                _programRepository.Add(program);
            else
                _programRepository.Update(program);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
