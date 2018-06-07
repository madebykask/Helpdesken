using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.DisplayValues.Report
{
    public sealed class TimeDisplayValue : DisplayValue<int?>
    {
        private readonly string _translationHour;
        private readonly string _translationMinutes;

        public TimeDisplayValue(int? value, string translationHour, string translationMinutes)
            : base(value)
        {
            _translationHour = translationHour;
            _translationMinutes = translationMinutes;
        }

        public override string GetDisplayValue()
        {
            var tempValue = 0;
            if (Value.HasValue) tempValue = Value.Value;

            return $"{tempValue / 60} {_translationHour} {tempValue % 60} {_translationMinutes}";
        }
    }
}
