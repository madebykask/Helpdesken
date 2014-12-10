namespace DH.Helpdesk.Services.Response.Account
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public class AccountOptionsResponse
    {
        public AccountOptionsResponse(
            List<ItemOverview> regions,
            List<ItemOverview> departments,
            List<ItemOverview> units,
            List<ItemOverview> employmentTypes,
            List<AccountTypeOverview> accountTypes,
            List<ItemOverview> programs)
        {
            this.Regions = regions;
            this.Departments = departments;
            this.Units = units;
            this.EmploymentTypes = employmentTypes;
            this.AccountTypes = accountTypes;
            this.Programs = programs;
        }

        public List<ItemOverview> Regions { get; set; }

        public List<ItemOverview> Departments { get; set; }

        public List<ItemOverview> Units { get; set; }

        public List<ItemOverview> EmploymentTypes { get; set; }

        public List<AccountTypeOverview> AccountTypes { get; set; }

        public List<ItemOverview> Programs { get; set; }
    }
}
