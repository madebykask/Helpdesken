using ECT.Core.FileStore;
using Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class FileController : BaseController
    {
        private readonly IContractRepository _contractRepository;

        public FileController(IContractRepository contractRepository
            , IUserRepository userRepository)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
        }

        public FileResult File(string id)
        {
            var mimetype = FileStore.GetMimeTypeSupport(id);
            return File(id, mimetype);
        }
    }
}
