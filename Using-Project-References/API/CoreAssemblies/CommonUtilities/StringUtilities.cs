/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 18/04/2017
 * Time: 4:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CommonUtilities
{
	/// <summary>
	/// Description of StringUtilities.
	/// </summary>
	public class StringUtilities
	{
		public StringUtilities()
		{
		}

		public static bool SameText(string LeftStr,
                                    string RightStr)
        {
            return SameText(LeftStr, RightStr, true);
        }

        public static bool SameText(string LeftStr,
                                    string RightStr,
                                    bool IgnoreCase)
        {
            return (string.Compare(LeftStr, RightStr, IgnoreCase) == 0);
        }

        public static int StrToIntDef(string s, int @default)
        {
            int number;
            if (int.TryParse(s, out number)) return number;
            return @default;
        }
        
        public static string JObjectToString(JObject Source)
        {
        	string Result = Source.ToString();
        	return Result;
        }
        
        public static bool EndsWith(StringBuilder Source, string Value)
        {
			Value = Value.Trim();
			if (Value.Length == 0) return false;
			
			if (Source.Length < Value.Length) return false;
			
			// The * 5 used here is just a value plucked out of the air to ensure
			// there are enough characters to ensure there are enough characters to work
			// with after the trim
			string end = Source.ToString(Source.Length - (Value.Length * 5), (Value.Length * 5)).Trim();
			return end.EndsWith(Value);
        }
	}
}
