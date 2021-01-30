using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Imani.Solutions.Core.API.Util
{
    /// <summary>
    ///  This class is geared toward string based processing. 
    /// It includes template engine support 
    /// like Free Marker that builds composite strings/values dynamically at runtime 
    /// (see http://freemarker.sourceforge.net/). 
    /// 
    /// author: Gregory Green
    /// </summary>
    public class Text
    {
        public string Format(string text, IDictionary<string, string> dictionary)
        {
            if (text == null || text.Length == 0)
                return ""; //nothing to format

            String output = text;
            object replacementValue = null;
        
            
            foreach (string key in dictionary.Keys)
            {
                replacementValue = dictionary[key];

                if ( replacementValue == null)
                    replacementValue = "";

                //replace placeholders
                output = output.Replace("${" + key + "}", replacementValue.ToString());
            }
            
            
            return ReplaceRE(@"\$\{.*\}","",output);
     
        }

        private string ReplaceRE(String re, String replacement, String source)
    {
    		Regex regex = new Regex(re);
    		return regex.Replace(source,replacement);
    		
    }
    /// <summary>
    /// Generated text of a given value
    /// </summary>
    /// <param name="size">the test length</param>
    /// <param name="seed">the text values to use to generate text</param>
    /// <returns>Outputted text with size</returns>
        public string GenerateText(int size,string seed)
        {
            if(seed == null || seed.Length == 0)
                throw new ArgumentException("Not empty text seed required");

            if(size < 1)
                return "";

            var builder = new StringBuilder(size);
            int seedLength = seed.Length;

            for(int i=0;i< size;i++)
            {
                builder.Append(seed[i%seedLength]);
            }
            
            return builder.ToString();

        }
    }
}