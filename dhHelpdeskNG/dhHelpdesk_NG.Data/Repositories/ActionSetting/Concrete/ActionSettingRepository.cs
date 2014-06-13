using System.Security.Cryptography.X509Certificates;

namespace DH.Helpdesk.Dal.Repositories.ActionSetting.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.ActionSetting;
    using DH.Helpdesk.Domain;

    public sealed class ActionSettingRepository : Repository, IActionSettingRepository
    {
        #region Constructors and Destructors

        public ActionSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        public List<ActionSetting> GetActionSettings(int customerId)
        {
            var actionSettings =
                this.DbContext.ActionSetting.Where(a => a.Customer_Id == customerId)
                    .Select(
                        c => new {customerId = c.Customer_Id, objectId = c.ObjectId, objectValue = c.ObjectValue, objectClass = c.ObjectClass, visibled = c.Visibled})
                    .ToList();

            return
                actionSettings.Select(
                    a =>
                    new ActionSetting
                        {
                            CustomerId = a.customerId,
                            ObjectId = a.objectId,
                            ObjectValue = a.objectValue,
                            ObjectClass = a.objectClass,
                            Visibled = a.visibled
                        }).ToList();
        }

       

    }
}