using System;

namespace Imani.Solutions.Core.API.Util
{

    public class ConfigSettings
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
            string configValue = Environment.GetEnvironmentVariable(configProp);

            if (configValue != null && configValue.Length > 0)
            {
                return configValue;

            }
            string formattedEnv = FormatEnvVarName(configProp);

            configValue = Environment.GetEnvironmentVariable(formattedEnv);
            if (String.IsNullOrEmpty(configValue))
                throw new ArgumentException($"Missing Environment Variable: {formattedEnv} or input argument: {configProp}");

            return configValue;
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

        internal string FormatEnvVarName(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return text.Replace('.', '_').ToUpper();
        }
    }
}