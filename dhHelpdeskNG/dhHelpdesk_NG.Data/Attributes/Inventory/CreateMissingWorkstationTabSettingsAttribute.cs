using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.Enums.Inventory.Computer;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Computers;
using PostSharp.Aspects;

namespace DH.Helpdesk.Dal.Attributes.Inventory
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingWorkstationTabSettingsAttribute: OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string _customerIdParameterName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingWorkstationTabSettingsAttribute(string customerIdParameter)
        {
            _customerIdParameterName = customerIdParameter;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            var customerIdParameter = methodParameters.Single(p => p.Name == _customerIdParameterName);
            var customerIdParameterIndex = customerIdParameter.Position;
            var customerId = (int)args.Arguments[customerIdParameterIndex];

            var dbcontext = new DatabaseFactory().Get();
            var settings = dbcontext.WorkstationTabSettings;
            var settingNames = settings.Where(s => s.Customer_Id == customerId).Select(s => s.TabField).ToList();

            CreateSettingIfNeeded(WorkstationTabs.Workstations, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.Storages, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.Softwares, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.HotFixes, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.ComputerLogs, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.Accessories, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationTabs.RelatedCases, customerId, settingNames, settings);

            dbcontext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static WorkstationTabSetting CreateDefaultSetting(string fieldName, int customerId)
        {
            return new WorkstationTabSetting
            {
                TabField = fieldName,
                CreatedDate = DateTime.Now,
                ChangedDate = DateTime.Now,
                Customer_Id = customerId,
                Show = true
            };
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int customerId,
            List<string> settingNames,
            DbSet<WorkstationTabSetting> settings)
        {
            var settingExists = settingNames.Any(s => string.Equals(s, checkSettingName, StringComparison.CurrentCultureIgnoreCase));
            if (settingExists)
            {
                return;
            }

            var setting = CreateDefaultSetting(checkSettingName, customerId);
            settings.Add(setting);
        }

        #endregion
    }
}
