/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/04/2020
 * Time: 7:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonUtilities
{
    public class ValidationChecks
    {
        public static string ErrorIfZeroInteger(int IntValue,
                                                string ErrorString)
        {
            string Result = "";

            if (IntValue == 0)
            {
                Result = "The " + ErrorString + " must be supplied" + Environment.NewLine;
            }
            return Result;
        }
    	
        public static string ErrorIfNegativeInteger(int IntValue,
    	                                            string ErrorString)
        {
            string Result = "";

            if (IntValue < 0)
            {
                Result = "The " + ErrorString + " must be supplied" + Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfZeroCurrency(double CurrValue,
                                                 string ErrorString)
        {
            string Result = "";

            if (CurrValue == 0.0)
            {
                Result = "The " + ErrorString + " must be supplied." + Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfNegativeCurrency(double CurrValue,
    	                                             string ErrorString)
        {
            string Result = "";

            if (CurrValue < 0.0)
            {
                Result = "The " + ErrorString + " must be supplied." + Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfEmptyString(string StrValue,
                                                string ErrorString)
        {
            string Result = "";
            if (StrValue == "")
            {
                Result = "The " + ErrorString + " must be supplied." + Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfNeitherString(string FirstString,
                                                  string SecondString,
                                                  string FirstStringName,
                                                  string SecondStringName)
        {
            string Result = "";
            if ((FirstString == "") && (SecondString == ""))
            {
                Result =
                    Result +
                    "At least one of the " + FirstStringName + " or " + SecondStringName +
                    " must be supplied." + Environment.NewLine;
            }
            return Result;
        }
    	
    	public static string ErrorIfStringLongerThan(string StrValue,
    	                                             string ErrorString,
    	                                             int MaxLength)
    	{
            string Result = "";
            if (StrValue.Length > MaxLength)
            {
            	Result = 
            		"The " + ErrorString + " is too long - Max Length of " + MaxLength.ToString() + " characters";
            }
            return Result;
    	}
    	
    	public static string ErrorIfStringShorterThan(string StrValue,
    	                                              string ErrorString,
    	                                              int MinLength)
    	{
            string Result = "";
            if (StrValue.Length < MinLength)
            {
            	Result = 
            		"The " + ErrorString + " is too short - Min Length of " + MinLength.ToString() + " characters";
            }
            return Result;
    	}
    	
    	public static string ErrorIfStringNotInRange(string StrValue,
    	                                             string ErrorString,
    	                                             int MinLength,
    	                                             int MaxLength)
    	{
            string Result = "";
            if ((StrValue.Length < MinLength) ||
                (StrValue.Length > MaxLength))
            {
                Result =
                    "The " + ErrorString + " must be between " +
                	MinLength.ToString() + " and " + MaxLength.ToString() +
                	" characters long.";
            }
            return Result;
    	}
    	
    	public static string ErrorIfStringNotInList(string StrValue,
    	                                            List<string> StrList,
    	                                            string ErrorString)
    	{
    		string Result = "";
    		
    		bool found = false;
    		string Str = "";
    		int Index = 0;
    		
    		while ((found == false) && (Index < StrList.Count))
    		{
    			Str = StrList[Index];
    			found = StringUtilities.SameText(StrValue, Str);
    			Index++;
    		}
    		
    		if (!found)
    		{
    			Result = "The " + ErrorString + " does not contain an entry from the list provided.";
    		}
    		
    		return Result;
    	}

        public static string ErrorIfNoneOf(string FirstString,
                                           string SecondString,
                                           string ThirdString,
                                           string FirstStringName,
                                           string SecondStringName,
                                           string ThirdStringName)
        {
            string Result = "";
            if ((FirstString == "") && (SecondString == "") && (ThirdString == ""))
            {
                Result =
                    Result +
                    "At least one of the " + FirstStringName + ", " + SecondStringName + " or " + ThirdStringName +
                    " must be supplied." + Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfDateNotInRange(DateTime DateValue,
                                                   DateTime MinDate,
                                                   DateTime MaxDate,
                                                   string ErrorString)
        {
            string Result = "";
            string MinDateStr = MinDate.ToString("dd-MM-yyyy");
            string MaxDateStr = MaxDate.ToString("dd-MM-yyyy");
            DateTime MinDateOnly = DateTime.ParseExact(MinDateStr, "dd-MM-yyyy", null);
            DateTime MaxDateOnly = DateTime.ParseExact(MaxDateStr, "dd-MM-yyyy", null);

            if (!((DateValue >= MinDateOnly) && (DateValue <= MaxDateOnly)))
            {
                Result =
                    "The date supplied must be between " +
                    MinDateStr + " and " + MaxDateStr +
                    Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfInvalidPositiveInteger(string IntValueStr,
                                                           string ErrorString)
        {
            string Result = "";
            int IntValue = StringUtilities.StrToIntDef(IntValueStr, -1);
            if (IntValue < 0)
            {
                Result =
                    "The " + ErrorString + " is either not a valid number or is < 0" +
                    Environment.NewLine;
            }
            return Result;
        }

        public static string ErrorIfNotInteger(string Input,
                                             string ErrorString)
        {
            string Result = "";

            Regex regex = new Regex(@"^-\d+$");
            if (!regex.IsMatch(Input))
            {
                Result =
                    "The " + ErrorString + " is not a valid number" + Environment.NewLine;
            }

            return Result;
        }

        public static string ErrorIfNotValidEmailAddress(string Input,
                                                         string ErrorString)
        {
            string Result = "";
            string InvalidEmailAddressString = "The " + ErrorString + " is not a valid Email Address" + Environment.NewLine;

            /*
             * Must contain 1 and only 1 @
             * After the @ must contain at least 1 "." and the strings separated by the . must be at least 2 chars long
             * check to see if there are any invalid characters for email addresses
            */

            string[] emailParts = Input.Split(new string[] {"@"}, StringSplitOptions.RemoveEmptyEntries);

            if (emailParts.Length != 2)
            {
                //return InvalidEmailAddressString;
                return "emailParts.Length: " + emailParts.Length.ToString();
            }

            string DomainPart = emailParts[1];
            string[] domainParts = DomainPart.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);

            if ((domainParts.Length < 2) || (domainParts.Length > 3))
            {
                //return InvalidEmailAddressString;
                return "DomainPart.Split length: " + domainParts.Length.ToString();
            }

            Result = "";
            foreach (string Str in domainParts)
            {
                if ((Str.Length < 2) || (Str.Length > 63))
                {
                    Result = "The " + ErrorString + " has an invalid domain section" + Environment.NewLine;
                    break;
                }
            }
            if (Result != "") return Result;

            Result = "";
            string LocalPart = emailParts[0];
            string[] localParts = LocalPart.Split(new string[] { ".." }, StringSplitOptions.None);
            if ((localParts.Length - 1) > 1)
            {
                Result = "The " + ErrorString + " has an invalid local section" + Environment.NewLine;
            }
            if (Result != "") return Result;

            return "";
        }
    }
}