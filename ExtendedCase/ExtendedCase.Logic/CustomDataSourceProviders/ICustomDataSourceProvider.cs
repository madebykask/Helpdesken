using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.CustomDataSourceProviders
{
    public interface ICustomDataSourceProvider
    {
        string Type { get; }

        IList<IDictionary<string, string>> GetData(JObject metaData, JObject query);
    }
}
