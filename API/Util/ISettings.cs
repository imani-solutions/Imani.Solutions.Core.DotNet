namespace Imani.Solutions.Core.API.Util
{
    public interface ISettings
    {
        public string GetProperty(string configProp);

        public string GetProperty(string propertyName, string defaultValue);

        public int GetPropertyInteger(string propertyName, int defaultValue);

        public int GetPropertyInteger(string propertyName);
    }
}