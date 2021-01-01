using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Imani.Solutions.Core.API.Util
{
    /// <summary>
    /// Cryption provides a set of functions to encrypt and 
    ///  decrypt bytes and text. 
    /// 
    /// author: Gregory Green
    /// </summary>
    public class Cryption
    {
        private byte[] key;

        private const int REQUIRED_KEY_SIZE = 32;
        public const String CRYPTION_PREFIX = "{cryption}";

        public Cryption(string key)
        {
            if(String.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Non whitespace key required");
                
            this.key = Encoding.UTF8.GetBytes(key.PadRight(REQUIRED_KEY_SIZE));
        }


         public string EncryptText(string plainText)  
        {  
            byte[] iv = new byte[16];  
            byte[] array;  
  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = key;  
                aes.IV = iv;  
  
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
                using (MemoryStream memoryStream = new MemoryStream())  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))  
                        {  
                            streamWriter.Write(plainText);  
                        }  
  
                        array = memoryStream.ToArray();  
                    }  
                }  
            }  
  
            return Convert.ToBase64String(array);  
        }

        public string EncryptTextWithPrefix(string value)
        {
            return new StringBuilder().Append(CRYPTION_PREFIX)
            .Append(this.EncryptText(value)).ToString();
        }

        public string DecryptText(string cipherText)  
        {  
            byte[] iv = new byte[16];  
            byte[] buffer = Convert.FromBase64String(cipherText);  
  
            using (Aes aes = Aes.Create())  
            {  
                aes.Key = key;  
                aes.IV = iv;  
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);  
  
                using (MemoryStream memoryStream = new MemoryStream(buffer))  
                {  
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))  
                    {  
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))  
                        {  
                            return streamReader.ReadToEnd();  
                        }  
                    }  
                }  
            }  
        }

        public String Interrupt(string configValue)
        {
            if(configValue == null)
                return "";
            
            String trimmedConfigValue = configValue.Trim();
            if(trimmedConfigValue.StartsWith(CRYPTION_PREFIX))
            {
                String value = trimmedConfigValue.Substring(CRYPTION_PREFIX.Length);
                return DecryptText(value);
            }

            return configValue;
        }
    }  

}

        