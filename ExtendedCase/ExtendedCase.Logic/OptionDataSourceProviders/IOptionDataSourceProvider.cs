using System.Collections.Generic;
using ExtendedCase.Models;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.OptionDataSourceProviders
{
    public interface IOptionDataSourceProvider
    {
        string Type { get; }

        IList<DataSourceOptionModel> GetOptions(JObject metaData, JObject query);
    }
}
