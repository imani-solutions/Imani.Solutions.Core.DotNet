namespace Imani.Solutions.Core.API.Serialization
{
    public interface ISerde<T,S>
    {
        public  S Serialize(T deserialized);
       
        public  T Deserialize(S serialized);
    }
}