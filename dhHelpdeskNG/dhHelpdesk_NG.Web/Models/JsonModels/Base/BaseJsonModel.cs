using DH.Helpdesk.BusinessData.Models.Shared.Input;

namespace DH.Helpdesk.Web.Models.JsonModels.Base
{
    public abstract class BaseJsonModel<T> where T:INewBusinessModel
    {
        public abstract T ToBussinessModel();
    }
    
}