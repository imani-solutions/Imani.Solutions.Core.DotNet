using System;

namespace Imani.Solutions.Core.API.Util
{
    /// <summary>
    /// This object supports geting properties for environment
    /// variables or input arguments (prefix with --PROPERTY_NAME)
    /// 
    /// author: Gregory Green
    /// </summary>
    public class ConfigSettings : ISettings
    {
        private Cryption cryption  = null;
        private const string CRYPTION_KEY_PROP = "CRYPTION_KEY";

        public ConfigSettings() : this(Environment.GetCommandLineArgs())
        {
        }

        public ConfigSettings(string[] args)
        {
            this.AddArgs(args);
        }

        public string GetProperty(string configProp)
        {
            string configValue = GetFromEnv(configProp);

            if (String.IsNullOrEmpty(configValue))
            {
                string formattedEnv = FormatEnvVarName(configProp);
                throw new ArgumentException($"Missing Environment Variable: {formattedEnv} or input argument: {configProp}");
            }

            return configValue;
        }

        public string GetProperty(string propertyName, string defaultValue)
        {
            string configValue = GetFromEnv(propertyName);

            if(String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;

        }

        public string EncryptPassword(char[] password)
        {
            return this.GetCryption().EncryptTextWithPrefix(new string(password));
        }

        public char[] GetPropertyPassword(string propertyName)
        {
            string configValue = GetProperty(propertyName);

            
            return GetCryption().Interrupt(configValue).ToCharArray();
        }
        public char[] GetPropertyPassword(string propertyName, char[] defaultValue)
        {
            string configValue = GetProperty(propertyName,"");
            if("".Equals(configValue))
                return defaultValue;

            return GetCryption().Interrupt(configValue).ToCharArray();
        }

        private string GetFromEnv(string configProp)
        {
            string configValue = Environment.GetEnvironmentVariable(configProp);

            if (configValue != null && configValue.Length > 0)
            {
                return configValue;

            }
            string formattedEnv = FormatEnvVarName(configProp);

            return Environment.GetEnvironmentVariable(formattedEnv);
        }

        internal void AddArgs(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            foreach (string arg in args)
            {
                if (String.IsNullOrWhiteSpace(arg))
                    continue;

                string[] rawKeyValues = arg.Split('=');
                if (rawKeyValues.Length != 2)
                    continue;

                string key = rawKeyValues[0].Replace("--", "");
                string value = rawKeyValues[1];

                if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value))
                    return;

                Environment.SetEnvironmentVariable(FormatEnvVarName(key), value);
            }
        }


        private  Cryption GetCryption()
        {
            if(cryption == null)
            {
                cryption = new Cryption(GetProperty(CRYPTION_KEY_PROP));
            }

            return cryption;
        }

     

        internal string FormatEnvVarName(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return text.Replace('.', '_').ToUpper();
        }

        public int GetPropertyInteger(string propertyName)
        {
            string value = this.GetProperty(propertyName);
            
            return Convert.ToInt32(value);
        }

        public int GetPropertyInteger(string propertyName, int defaultValue)
        {
            return  GetProperty<int>(propertyName, defaultValue, (value) => Convert.ToInt32(value));
        }
        public bool GetPropertyBoolean(string propertyName)
        {
             string value = this.GetProperty(propertyName);
            
            return Convert.ToBoolean(value);
        }
       public bool GetPropertyBoolean(string propertyName, bool defaultValue)
        {
            return  GetProperty<bool>(propertyName, defaultValue, (value) => Convert.ToBoolean(value));
        }

        internal T GetProperty<T>(string propertyName, object defaultValue, Func<object,object> func)
        {
            string value = GetFromEnv(propertyName);
            if(String.IsNullOrEmpty(value))
                return (T)defaultValue;
                
            return (T)func(value);
        }

     
    }
}