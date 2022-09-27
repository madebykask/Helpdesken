using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerService _customerService;

        #region ctor()

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion

        [HttpGet]
        [Route("{cid:int}")]
        public async Task<CustomerDetails> Get([FromUri]int cid)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(cid);
            return customer;
        }

        [HttpGet]
        [Route("communicatewithnotifier")]
        public async Task<bool> GetCommunicateWithNotifier([FromUri] int cid)
        {
            var communicateWithNotifier = await _customerService.GetCommunicateWithNotifier(cid);
            return communicateWithNotifier == 1 ? true: false;
        }
    }
}