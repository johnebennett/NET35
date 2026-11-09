using System;
using System.Runtime.InteropServices;

namespace IPX_App
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

        public static void Main(string[] args)
        {
            ShowWindow(ThisConsole, MINIMIZE);
            // if there are not exactly three command line args just return
            if (args.Length != 3)
            {
                CommonMethods.WriteToErrorOutput("There must be three command line parameters");
                CommonMethods.WriteToErrorOutput("which must be the /RunId: value from the I_IPXLOGGING_ID_GEN generator,");
                CommonMethods.WriteToErrorOutput("the Direction /Dir(ection):Up(load) /Dir(ection):Down(load)");
                CommonMethods.WriteToErrorOutput("and /Id:CompanyId of the company to process or /Id:All to process all companies.");
                CommonMethods.WriteToErrorOutput("The parameters must be provided in the nominated order.");
                return;
            }

            string Cmd = "";
            string Direction = "";
            string RunId = "";

            //string Item = args[args.Length - 1].ToUpper();
            string Item = "";  args[args.Length - 1].ToUpper();

            for (int Index = 1; Index <= 3; Index++)
            {
                Item = args[args.Length - Index].ToUpper();
                if (Item.StartsWith(@"/DIR")) Direction = Item;
                else if (Item.StartsWith(@"/ID")) Cmd = Item;
                else if (Item.StartsWith(@"/RUNID")) RunId = Item;
            }

            /*
            if (Item.StartsWith(@"/DIR")) Direction = Item; else Cmd = Item;
            Item = args[args.Length - 2].ToUpper();
            if (Item.StartsWith(@"/DIR")) Direction = Item; else Cmd = Item;
            */           

            if (string.IsNullOrEmpty(Direction) || 
                string.IsNullOrEmpty(Cmd) ||
                string.IsNullOrEmpty(RunId))
            {
                CommonMethods.WriteToErrorOutput("RunId, Direction and Company Id parameters need to be provided");
                return;
            }

            Direction = Direction.Replace(@"/DIRECTION:", "");
            Direction = Direction.Replace(@"/DIR:", "");
            if (Direction == "UP") Direction = "UPLOAD";
            if (Direction == "DOWN") Direction = "DOWNLOAD";
            Cmd = Cmd.Replace(@"/ID:", "");
            RunId = RunId.Replace(@"/RUNID:", "");

            if ((Direction != "UPLOAD") && (Direction != "DOWNLOAD"))
            {
                CommonMethods.WriteToErrorOutput("Invalid Direction (" + Direction + ") supplied.");
                return;
            }

            if (Cmd != "ALL")
            {
                int CompanyIdValue = CommonUtilities.StringUtilities.StrToIntDef(Cmd, -1);
                if (CompanyIdValue < 0)
                {
                    CommonMethods.WriteToErrorOutput("Command " + Cmd + " is not a valid number");
                    return;
                }
            }

            if (string.IsNullOrEmpty(RunId))
            {
                CommonMethods.WriteToErrorOutput("RunId must be provided");
            }
            else
            {
                int RunIdValue = CommonUtilities.StringUtilities.StrToIntDef(RunId, -1);
                if (RunIdValue < 0)
                {
                    CommonMethods.WriteToErrorOutput("RunId " + RunId + " is not a valid number");
                    return;
                }
            }

            try
            {
                CommonMethods.WriteToOutput(Cmd, "Before Processing");
                new Processor().Execute(RunId, Direction, Cmd);
                CommonMethods.WriteToOutput(Cmd, "After Processing");
                CommonMethods.WriteToOutput(Cmd, "");
            }
            catch (Exception ex)
            {
                CommonMethods.WriteToErrorOutput("Direction: " + Direction + " Command: " + Cmd);
                CommonMethods.WriteToErrorOutput(ex.Message);
                CommonMethods.WriteToErrorOutput(ex.StackTrace);
            }
        }
    }
}
