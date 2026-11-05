/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/03/2019
 * Time: 1:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace UsefulDBMethods
{
	/// <summary>
	/// Description of DateHelpers.
	/// </summary>
	public class DateHelpers
	{
		public DateHelpers()
		{
		}
		
		public static string YYYYMMDDToDbDateString(string Source,
		                                            string Separator)
		{
			string Result = "";
			
			if (Source.Length == 10)
			{
				Result = 
					Source.Substring(5, 2) + Separator +
					Source.Substring(7, 2) + Separator +
					Source.Substring(1, 4) + Separator;
			}
			
			return Result;
		}
		
		public static string YYYYMMDDToDbDateString(string Source)
		{
			return YYYYMMDDToDbDateString(Source, "/");
		}
	}
}
