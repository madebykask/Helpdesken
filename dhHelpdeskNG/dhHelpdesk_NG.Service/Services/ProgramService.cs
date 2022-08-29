namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ProgramService(
            IProgramRepository programRepository,
            IUnitOfWork unitOfWork)
        {
            this._programRepository = programRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Program> GetPrograms(int customerId)
        {
            return this._programRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Program GetProgram(int id)
        {
            return this._programRepository.GetById(id);
        }

        public DeleteMessage DeleteProgram(int id)
        {
            var program = this._programRepository.GetById(id);

            if (program != null)
            {
                try
                {
                    this._programRepository.Delete(program);
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
                this._programRepository.Add(program);
            else
                this._programRepository.Update(program);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
