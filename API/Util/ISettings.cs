namespace Imani.Solutions.Core.API.Util
{
    /// <summary>
    /// Interface for configuration settings
    /// 
    /// author: Gregory Green
    /// </summary>
    public interface ISettings
    {
        public string GetProperty(string configProp);

        public string GetProperty(string propertyName, string defaultValue);

        public int GetPropertyInteger(string propertyName, int defaultValue);

        public int GetPropertyInteger(string propertyName);

        public char[] GetPropertyPassword(string propertyName);

        public char[] GetPropertyPassword(string propertyName, char[] defaultValue);

        public bool GetPropertyBoolean(string propertyName, bool defaultValue);

        public bool GetPropertyBoolean(string propertyName);

        public string GetPropertySecret(string propertyName, string defaultValue);

        public string GetPropertySecret(string propertyName);

    }
}