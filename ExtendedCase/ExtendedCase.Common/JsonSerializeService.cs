using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ExtendedCase.Common
{
    public interface IJsonSerializeService
    {
        string Serialize(object data);
        object Deserialize(string data);
        object Deserialize(string data, Type type);
        TObject Deserialize<TObject>(string data) where TObject : class, new();
    }

    public class JsonSerializeService : IJsonSerializeService
    {
        private readonly JsonSerializer _serializer;

        #region ctor()

        public JsonSerializeService()
        {
            _serializer = new JsonSerializer();
        }

        #endregion

        #region Serialize

        public string Serialize(object data)
        {
            var builder = new StringBuilder();

            using (TextWriter textWriter = new StringWriter(builder))
            using (JsonWriter jsonTextWriter = new JsonTextWriter(textWriter))
            {
                _serializer.Serialize(jsonTextWriter, data);
            }

            return builder.ToString();
        }

        #endregion

        #region Deserialize

        public object Deserialize(string data)
        {
            return DeserializeInner(data);
        }

        public object Deserialize(string data, Type type)
        {
            return DeserializeInner(data, type);
        }

        public TObject Deserialize<TObject>(string data) where TObject : class, new()
        {
            var desirializedValue = DeserializeInner(data, typeof(TObject));
            return desirializedValue as TObject;
        }

        private object DeserializeInner(string data, Type type = null)
        {
            using (TextReader textReader = new StringReader(data))
            using (JsonReader jsonReader = new JsonTextReader(textReader))
            {
                return type != null
                    ? _serializer.Deserialize(jsonReader, type)
                    : _serializer.Deserialize(jsonReader);
            }
        }

        #endregion
    }
}