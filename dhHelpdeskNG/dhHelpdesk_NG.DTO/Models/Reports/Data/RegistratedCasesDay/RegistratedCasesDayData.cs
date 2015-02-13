namespace DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistratedCasesDayData
    {
        public RegistratedCasesDayData(List<RegisteredCaseDay> registeredCases)
        {
            this.RegisteredCases = registeredCases;
        }

        [NotNull]
        public List<RegisteredCaseDay> RegisteredCases { get; private set; }
    }
}