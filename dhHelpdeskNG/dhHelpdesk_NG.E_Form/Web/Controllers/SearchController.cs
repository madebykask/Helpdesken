using System.Web.Mvc;
using ECT.Model.Abstract;

namespace ECT.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IGlobalViewRepository _globalViewRepository;

        public SearchController(IGlobalViewRepository globalViewRepository
            , IUserRepository userRepository)
            : base(userRepository)
        {
            _globalViewRepository = globalViewRepository;
        }

        [HttpPost]
        public ActionResult GlobalView(string query, int customerId, string searchKey)
        {
            var result = _globalViewRepository.GlobalViewSearch(query, customerId, searchKey);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GlobalViewForCustomer(string query, int customerId)
        {
            var result = _globalViewRepository.GlobalViewSearch(query, customerId);
            return Json(result);
        }
    }
}