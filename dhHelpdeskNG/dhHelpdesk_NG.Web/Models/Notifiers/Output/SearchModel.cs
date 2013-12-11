namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;
    using dhHelpdesk_NG.Web.Infrastructure;

    public sealed class SearchModel
    {
        public SearchModel(SearchDropDownModel domain, SearchDropDownModel department, SearchDropDownModel division, Enums.Show show, int recordsOnPage)
        {
            ArgumentsValidator.NotNull(domain, "domain");
            ArgumentsValidator.NotNull(department, "department");
            ArgumentsValidator.NotNull(division, "division");

            this.Domain = domain;
            this.Department = department;
            this.Division = division;
            this.Show = show;
            this.RecordsOnPage = recordsOnPage;
        }

        public SearchDropDownModel Domain { get; private set; }

        public SearchDropDownModel Department { get; private set; }

        public SearchDropDownModel Division { get; private set; }

        public int RecordsOnPage { get; private set; }

        public Enums.Show Show { get; private set; }
    }
}