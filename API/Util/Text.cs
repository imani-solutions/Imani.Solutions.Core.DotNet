using System;
using System.Collections;
using System.Collections.Generic;
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

    }
}