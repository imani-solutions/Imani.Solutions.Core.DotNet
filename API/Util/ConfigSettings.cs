using System;

namespace Imani.Solutions.Core.API.Util
{

    public class ConfigSettings : ISettings
    {

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


        public string GetProperty(string propertyName, string defaultValue)
        {
            string configValue = GetFromEnv(propertyName);

            if(String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;

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

        internal T GetProperty<T>(string propertyName, object defaultValue, Func<object,object> func)
        {
            string value = GetFromEnv(propertyName);
            if(String.IsNullOrEmpty(value))
                return (T)defaultValue;
                
            return (T)func(value);
        }
    }
}