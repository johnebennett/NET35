using System;
using System.Runtime.InteropServices;
using System.IO;
using StrUtils = CommonUtilities.StringUtilities;

namespace Base64EncoderDecoder
{
    class MainClass
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        
        //private static ConfigItems m_ConfigItems = new ConfigItems();
        private static ConfigItems m_ConfigItems;

        public static void Main(string[] args)
        {
            //ShowWindow(ThisConsole, MINIMIZE);
            /*
            if (args.Length != 3)
            {
                CommonMethods.WriteToErrorOutput("There must be three command line parameters");
                CommonMethods.WriteToErrorOutput("which must be the /Src(FileName): value,");
                CommonMethods.WriteToErrorOutput("the  /Dest(ination FileName): value");
                CommonMethods.WriteToErrorOutput("and /Action:Encode or /Action:Decode.");
                CommonMethods.WriteToErrorOutput("The parameters must be provided in the nominated order.");
                CommonMethods.WriteToErrorOutput("If the Destination file exists it will be overwritten.");
                return;
            }
            */

            /*
            string Action = "";
            string ActionType = "";
            string SrcFile = "";
            string DestFile = "";
            string ConnectionString = "";
            string SrcTableName = "";
            string SrcFieldName = "";
            string SrcWhereClause = "";
            string DestTableName = "";
            string DestFieldName = "";
            string DestWhereClause = "";
            */
           
           
           
           string Result = "";
           m_ConfigItems = ConfigItems.Load(out Result);
           	if (!string.IsNullOrEmpty(Result))
           	{
           		Console.WriteLine(Result);           			
           		return;
           	}
           	
			CommandLineParams Params = new CommandLineParams();
                       
            int MaxArgs = args.Length;
            string Item = ""; 
            //args[args.Length - 1].ToUpper();

            for (int Index = 1; Index <= MaxArgs; Index++)
            {
                Item = args[args.Length - Index].ToUpper();
                Item = args[args.Length - Index];
                CommonMethods.WriteToErrorOutput("Processing Item: " + Item);
                if (Item.StartsWith(@"/ACTION:")) Params.Action = Item;
                else if (Item.StartsWith(@"/TYPE:")) Params.ActionType = Item;
                else if (Item.StartsWith(@"/SRCFILE:")) Params.SrcFile = Item;
                else if (Item.StartsWith(@"/DESTFILE:")) Params.DestFile = Item;
                else if (Item.StartsWith(@"/CONN:")) Params.ConnectionString = Item;

                else if (Item.StartsWith(@"/SRCTABLE:")) Params.SrcTableName = Item;
                else if (Item.StartsWith(@"/SRCFIELD:")) Params.SrcFieldName = Item;
                else if (Item.StartsWith(@"/SRCWHERE:")) Params.SrcWhereClause = Item;

                else if (Item.StartsWith(@"/DESTTABLE:")) Params.DestTableName = Item;
                else if (Item.StartsWith(@"/DESTFIELD:")) Params.DestFieldName = Item;
                else if (Item.StartsWith(@"/DESTWHERE:")) Params.DestWhereClause = Item;
            }

            /*
            if (string.IsNullOrEmpty(Src) ||
                string.IsNullOrEmpty(Dest) ||
                string.IsNullOrEmpty(Action))
            {
                CommonMethods.WriteToErrorOutput("Src, Dest and Action parameters need to be provided");
                return;
            }
            */

            Params.Action = Params.Action.Replace(@"/ACTION:", "");
            Params.ActionType = Params.ActionType.Replace(@"/TYPE:", "");
            Params.SrcFile = Params.SrcFile.Replace(@"/SRCFILE:", "");
            Params.DestFile = Params.DestFile.Replace(@"/DESTFILE:", "");
            Params.ConnectionString = Params.ConnectionString.Replace("/CONN:", "");
            Params.SrcTableName = Params.SrcTableName.Replace("/SRCTABLE:", "");
            Params.SrcFieldName = Params.SrcFieldName.Replace("/SRCFIELD:", "");
            Params.SrcWhereClause = Params.SrcWhereClause.Replace("/SRCWHERE:", "");
            Params.DestTableName = Params.DestTableName.Replace("/DESTTABLE:", "");
            Params.DestFieldName = Params.DestFieldName.Replace("/DESTFIELD:", "");
            Params.DestWhereClause = Params.DestWhereClause.Replace("/DESTWHERE:", "");

            Params.Display();

            CommonMethods.WriteToErrorOutput("Before Params.Validate");
            if (!Params.Validate()) return;
            CommonMethods.WriteToErrorOutput("After Params.Validate");
            
            if (Params.ActionType.Contains("D"))
            {
            	Result = CommonMethods.TestDatabaseConnection(Params.ConnectionString);
            	if (!string.IsNullOrEmpty(Result))
            	{
            		CommonMethods.WriteToErrorOutput(Result);
            		Console.WriteLine(Result);
            		return;
            	}
            }

            /*
            if (!CommonMethods.ValidateInputs())

            if (!File.Exists(Src))
            {
                CommonMethods.WriteToErrorOutput("The Source File: " + Src + " does not exist");
            }

            string Msg = "";

            if (Action == "ENCODE")
            {
                Msg = "Encoding " + Src + " to produce the file: " + Dest;
                CommonMethods.WriteToOutput("", Msg);
                new Encoder().Execute(Src, Dest);
            }
            else if (Action == "DECODE")
            {
                Msg = "Decoding " + Src + " to produce the file: " + Dest;
                CommonMethods.WriteToOutput("", Msg);
                new Decoder().Execute(Src, Dest);
            }
            else
            {
                Msg = 
                    "Invalid Action value provided." + Environment.NewLine + 
                    "Action must be either Encode or Decode";
                CommonMethods.WriteToErrorOutput(Msg);
                return;
            }
            */

            string Msg = "";

            if (StrUtils.SameText(Params.Action, "ENCODE") && StrUtils.SameText(Params.ActionType, "F2F"))
            {
                Msg = "Encoding " + Params.SrcFile + " to produce the file: " + Params.DestFile;
                Console.WriteLine(Msg);
                new Encoder(m_ConfigItems).ExecuteF2F(Params.SrcFile, Params.DestFile);
            }
            else if (StrUtils.SameText(Params.Action, "DECODE") && StrUtils.SameText(Params.ActionType, "F2F"))

            {
                Msg = "Decoding " + Params.SrcFile + " to produce the file: " + Params.DestFile;
                Console.WriteLine(Msg);
                new Decoder(m_ConfigItems).ExecuteF2F(Params.SrcFile, Params.DestFile);
            }
            else if (StrUtils.SameText(Params.Action, "ENCODE") && StrUtils.SameText(Params.ActionType, "F2D"))
            {
                Msg = "Encoding " + Params.SrcFile + " to populate the table: " + Params.DestTableName;
                Console.WriteLine(Msg);
                new Encoder(m_ConfigItems).ExecuteF2D(Params);
            }
            else if (StrUtils.SameText(Params.Action, "DECODE") && StrUtils.SameText(Params.ActionType, "D2F"))
            {
                Msg = "Decoding " + Params.SrcTableName + " to populate the file: " + Params.DestFile;
                Console.WriteLine(Msg);
                new Decoder(m_ConfigItems).ExecuteD2F(Params);
            }
            else
            {
                Msg =
                    "Unknown commands" + Environment.NewLine +
                    "Action: " + Params.Action + " ActionType: " + Params.ActionType;
                Console.WriteLine(Msg);
            }

            Console.WriteLine("Done");
            //CommonMethods.WriteToOutput("", "Hello World");
        }
    }
}
