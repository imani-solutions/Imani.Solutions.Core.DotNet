using System.Text.Json;

namespace Imani.Solutions.Core.API.Serialization
{
    public class JsonSerde<T> : ISerde<T, string>
    {
        public T Deserialize(string serialized)
        {
             T output = (T)JsonSerializer.Deserialize(serialized,typeof(T));
            return output;
        }

        public string Serialize(T deserialized)
        {
            return JsonSerializer.Serialize(deserialized);
        }
    }
}