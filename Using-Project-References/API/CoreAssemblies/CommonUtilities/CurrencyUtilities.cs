/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 18/04/2017
 * Time: 4:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CommonUtilities
{
	/// <summary>
	/// Description of CurrencyUtilities.
	/// </summary>
	public class CurrencyUtilities
	{
		public CurrencyUtilities()
		{
		}

		public static string BuildCurrencyDisplay(string Value,
		                                          bool ExcludeDollarSymbol)
        {
            string Result = (Value == "") ? "0" : Value;
            Result = Result.Replace("$", "");
            Result = Result.Trim();

            double dblValue = 0;
            try
            {
                dblValue = Convert.ToDouble(Result);
                Result = String.Format("{0:C}", dblValue);

            }
            catch (System.InvalidCastException ice)
            {
                dblValue = 0;
                Result = "Error: " + ice.Message;
            }
            catch (Exception ex)
            {
                dblValue = 0;
                Result = "Error: " + ex.Message;
            }
            if (ExcludeDollarSymbol) Result = Result.Replace("$", "");
            Result = Result.Trim();
            return Result;
        }

        public static string BuildCurrencyDisplay(string Value)
        {
            return BuildCurrencyDisplay(Value, true);
        }
	}
}
