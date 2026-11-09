using System;
using System.IO;
using StrUtils = CommonUtilities.StringUtilities;

namespace Base64EncoderDecoder
{
    public class CommandLineParams
    {
        public CommandLineParams()
        {
            this.Action = "";
            this.ActionType = "";
            this.SrcFile = "";
            this.DestFile = "";
            this.ConnectionString = "";
            this.SrcTableName = "";
            this.SrcFieldName = "";
            this.SrcWhereClause = "";
            this.DestTableName = "";
            this.DestFieldName = "";
            this.DestWhereClause = "";
        }

        public string Action { get; set; }
        public string ActionType { get; set; }
        public string SrcFile { get; set; }
        public string DestFile { get; set; }
        public string ConnectionString { get; set; }
        public string SrcTableName { get; set; }
        public string SrcFieldName { get; set; }
        public string SrcWhereClause { get; set; }
        public string DestTableName { get; set; }
        public string DestFieldName { get; set; }
        public string DestWhereClause { get; set; }

        public bool Validate()
        {
            bool Result = true;

            if (Result)
            {
                /*
                if (Action == "") Action = "Empty Action";
                Result = (StrUtils.SameText(Action, "ENCODE") || StrUtils.SameText(Action, "DECODE"));
                if (!Result) CommonMethods.WriteToErrorOutput("Invalid Action: " + Action + " provided");
                */
                Result = Validate_Action();               
            }

            if (Result)
            {
                Result = Validate_ActionType();
            }

            return Result;
        }

        private bool Validate_Action()
        {
            bool Result = true;

            if (Action == "") Action = "Empty Action";
            Result = (StrUtils.SameText(Action, "ENCODE") || StrUtils.SameText(Action, "DECODE"));
            if (!Result) CommonMethods.WriteToErrorOutput("Invalid Action: " + Action + " provided");

            return Result;
        }

        private bool Validate_ActionType()
        {
            bool Result = true;

            if (ActionType == "") ActionType = "Empty Action Type";
            switch (ActionType.ToUpper())
            {
                case "F2F": Result = Validate_ActionType_F2F(); break;
                case "F2D": Result = Validate_ActionType_F2D(); break;
                case "D2F": Result = Validate_ActionType_D2F(); break;
                case "D2D": Result = Validate_ActionType_D2D(); break;
                default:
                    Result = false;
                    CommonMethods.WriteToErrorOutput("Invalid ActionType: " + ActionType + " provided");
                    break;
            }


            return Result;
        }

        private bool Validate_FileExists(string FileName)
        {
            return File.Exists(FileName);
        }

        private bool Validate_ActionType_F2F()
        {
            bool Result = true;

            if (string.IsNullOrEmpty(SrcFile))
            {
                Result = false;
                CommonMethods.WriteToErrorOutput("The SrcFile cannot be empty");
            }
            else if (string.IsNullOrEmpty(DestFile))
            {
                Result = false;
                CommonMethods.WriteToErrorOutput("The DestFile cannot be empty");
            }

            if (Result)
            {
                if (!File.Exists(SrcFile))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcFile: " + SrcFile + " does not exist");
                }
            }

            return Result;
        }

        private bool Validate_ActionType_F2D()
        {
            bool Result = true;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                Result = false;
                CommonMethods.WriteToErrorOutput("The Connection String must be provided.");
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcFile))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcFile cannot be empty");
                }
                else
                {
                    Result = File.Exists(SrcFile);
                    if (!Result) CommonMethods.WriteToErrorOutput("The SrcFile: " + SrcFile + " does not exist");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestTableName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestTableName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestFieldName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestFieldName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestWhereClause))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestWhereClause cannot be empty");
                }
            }

            return Result;
        }

        private bool Validate_ActionType_D2F()
        {
            bool Result = true;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                Result = false;
                CommonMethods.WriteToErrorOutput("The Connection String must be provided.");
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestFile))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestFile cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcTableName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcTableName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcFieldName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcFieldName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcWhereClause))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcWhereClause cannot be empty");
                }
            }


            return Result;
        }

        private bool Validate_ActionType_D2D()
        {
            bool Result = true;


            if (string.IsNullOrEmpty(ConnectionString))
            {
                Result = false;
                CommonMethods.WriteToErrorOutput("The Connection String must be provided.");
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcTableName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcTableName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcFieldName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcFieldName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(SrcWhereClause))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The SrcWhereClause cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestTableName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestTableName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestFieldName))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestFieldName cannot be empty");
                }
            }

            if (Result)
            {
                if (string.IsNullOrEmpty(DestWhereClause))
                {
                    Result = false;
                    CommonMethods.WriteToErrorOutput("The DestWhereClause cannot be empty");
                }
            }

            return Result;
        }

        public void Display()
        {
            Console.WriteLine("Action: " + Action);
            Console.WriteLine("ActionType: " + ActionType);
            Console.WriteLine("SrcFile: " + SrcFile);
            Console.WriteLine("DestFile: " + DestFile);
            Console.WriteLine("ConnectionString: " + ConnectionString);
            Console.WriteLine("SrcTableName: " + SrcTableName);
            Console.WriteLine("SrcFieldName: " + SrcFieldName);
            Console.WriteLine("SrcWhereClause: " + SrcWhereClause);
            Console.WriteLine("DestTableName: " + DestTableName);
            Console.WriteLine("DestFieldName: " + DestFieldName);
            Console.WriteLine("DestWhereClause: " + DestWhereClause);
        }
    }
}
