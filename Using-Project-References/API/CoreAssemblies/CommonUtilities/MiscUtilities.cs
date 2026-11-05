/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 18/04/2017
 * Time: 4:04 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace CommonUtilities
{
    public enum SourceDateFormat {DDMMYYYY, MMDDYYYY};

    /// <summary>
	/// Description of MyClass.
	/// </summary>
	public class MiscUtilities
	{
        public static string BuildNumberDisplay(string Value, int DecimalPlaces)
        {
            string Result = (Value == "") ? "0" : Value;
            double dblValue = 0;
            string fmt = "";
            try
            {
                dblValue = Convert.ToDouble(Result);
                if (DecimalPlaces == 0) fmt = "{0:G0}";
                else                    fmt = "{0:G" + DecimalPlaces.ToString() + "}";
                Result = String.Format(fmt, dblValue);

            }
            catch (System.InvalidCastException)
            {
                dblValue = 0;
                Result = "Error";
            }
            Result = Result.Trim();
            return Result;
        }

        /// <summary>
        /// Format a date into DD-ShortMonthName-YYYY depending on the provided SourceDateFormat
        /// Assumptions:
        ///     4 digit year is provided
        ///     2 digit month is provided - with leading zero as required
        ///     2 digit day is provided - with leading zero as required
        ///     Single character separators are provided between the DD, MM, and YYYY
        /// </summary>
        /// <param name="Value">The string representation of the date using DD, MM and YYYY</param>
        /// <param name="Format">From the enum SourceDateFormat</param>
        /// <returns>Date in the format of DD-ShortMonthName-YYYY with dash separator</returns>
        public static string BuildDateDisplay(string Value, SourceDateFormat Format)
        {
            string Result = Value;

            switch (Format)
            {
                case SourceDateFormat.DDMMYYYY:
                    Result = BuildDateDisplayFromDDMMYYYY(Value);
                    break;
                case SourceDateFormat.MMDDYYYY:
                    Result = BuildDateDisplayFromMMDDYYYY(Value);
                    break;
            }
            return Result;
        }

        private static string BuildDateDisplayFromDDMMYYYY(string Value)
        {
            string Result = Value;
            string SplitChar = "";
            if (Result.Contains("/")) SplitChar = "/";
            else if (Result.Contains("-")) SplitChar = "-";
            string[] DateParts = Result.Split(SplitChar.ToCharArray());
            string DD = DateParts[0];
            string MM = DateParts[1];
            string YYYY = DateParts[2].Substring(0, 4);
            DateTime dt = new DateTime(Convert.ToInt32(YYYY), Convert.ToInt32(MM), Convert.ToInt32(DD));
            string ShortMonthName = dt.ToString("MMM").ToUpper();
            string Separator = "-";

            Result = DD + Separator + ShortMonthName + Separator + YYYY;

            return Result;
        }

        private static string BuildDateDisplayFromMMDDYYYY(string Value)
        {
            string Result = Value;
            string SplitChar = "";
            if (Result.Contains("/")) SplitChar = "/";
            else if (Result.Contains("-")) SplitChar = "-";
            string[] DateParts = Result.Split(SplitChar.ToCharArray());
            string DD = DateParts[1];
            string MM = DateParts[0];
            string YYYY = DateParts[2].Substring(0, 4);
            DateTime dt = new DateTime(Convert.ToInt32(YYYY), Convert.ToInt32(MM), Convert.ToInt32(DD));
            string ShortMonthName = dt.ToString("MMM").ToUpper();
            string Separator = "-";

            Result = DD + Separator + ShortMonthName + Separator + YYYY;

            return Result;
        }
        
        public static DateTime YYYYMMDDToDateTime(string Source)
        {
        	return YYYYMMDDToDateTime(Source, DateTime.MinValue);
        }
        
        public static DateTime YYYYMMDDToDateTime(string Source, DateTime Default)
        {
        	if (Source.Length != 8) return Default;
        	
			int DayInt = StringUtilities.StrToIntDef(Source.Substring(6, 2), 0);
			int MonthInt = StringUtilities.StrToIntDef(Source.Substring(4, 2), 0);
			int YearInt = StringUtilities.StrToIntDef(Source.Substring(0, 4), 0);
			
			if ((DayInt == 0) || (MonthInt == 0) || (YearInt == 0)) return Default;
			
			return new DateTime(YearInt, MonthInt, DayInt);
        }
        
        public static string DateTimeToYYYYMMDD(DateTime Source)
        {
        	/*
        	string YearStr = Source.Year.ToString();
        	string MonthStr = Source.Month.ToString();
        	if (MonthStr.Length == 1) MonthStr = "0" + MonthStr;
        	string DayStr = Source.Day.ToString();
        	if (DayStr.Length == 1) DayStr = "0" + DayStr;
        	*/
        	
        	string Result = Source.Year.ToString();
        	string Value = Source.Month.ToString();
        	if (Value.Length == 1) Value = "0" + Value;
        	Result = Result + Value;
        	Value = Source.Day.ToString();
        	if (Value.Length == 1) Value = "0" + Value;
        	Result = Result + Value;

        	return Result;
        }
        
        public static string DateTimeToYYYYMMDDTHHNNSS(DateTime Source)
        {
        	string Result = DateTimeToYYYYMMDD(Source) + "T";
        	string Value = Source.Hour.ToString();        	
        	if (Value.Length == 1) Value = "0" + Value;
        	Result = Result + Value;
        	Value = Source.Minute.ToString();
        	if (Value.Length == 1) Value = "0" + Value;
        	Result = Result + Value;
        	Value = Source.Second.ToString();
        	if (Value.Length == 1) Value = "0" + Value;
        	Result = Result + Value;
        	
        	return Result;
        }
	}
}