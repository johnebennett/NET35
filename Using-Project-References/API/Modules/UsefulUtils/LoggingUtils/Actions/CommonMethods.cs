using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.UsefulUtils.LoggingUtils
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "UsefulUtils.LoggingUtils"; }

        public static string LoadStartStopLoggingRequestItem(ActionBase ActionBase,
                                                             ref StartStopLoggingRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadStartStopLoggingRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "Action");
                TempItem.Action = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "LoggingEntries");
                TempItem.LoggingEntries = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EventViewer");
                TempItem.EventViewer = (Obj == null) ? "" : Convert.ToString(Obj);
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string StopStartLogging(StartStopLoggingRequestItem Request)
        {
            string Result = "";
            string ErrorMsg = "";
            string DirectorySeparator = Path.DirectorySeparatorChar.ToString();
            string CurrentDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
            CurrentDir += (CurrentDir.EndsWith(DirectorySeparator, StringComparison.CurrentCulture)) ? "" : DirectorySeparator;
            string LoggingEntriesFileName =  CurrentDir + "LoggingEntries.txt";
            string EventViewerFileName = CurrentDir + "Event_Viewer.txt";

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                if (StringUtilities.SameText(Request.Action, "Start"))
                {
                    //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Start section", GetModuleName(), 0);
                    if (StringUtilities.SameText(Request.LoggingEntries, "Y"))
                    {
                        //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside LoggingEntries section", GetModuleName(), 0);
                        if (!File.Exists(LoggingEntriesFileName))
                            File.Create(LoggingEntriesFileName);
                    }

                    if (StringUtilities.SameText(Request.EventViewer, "Y"))
                    {
                        //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside EventViewer section", GetModuleName(), 0);
                        if (!File.Exists(EventViewerFileName))
                            File.Create(EventViewerFileName);
                    }
                }
                else if (StringUtilities.SameText(Request.Action, "Stop"))
                {
                    //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside Stop section", GetModuleName(), 0);
                    if (StringUtilities.SameText(Request.LoggingEntries, "Y"))
                    {
                        //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside LoggingEntries section", GetModuleName(), 0);
                        if (File.Exists(LoggingEntriesFileName))
                            File.Delete(LoggingEntriesFileName);
                    }

                    if (StringUtilities.SameText(Request.EventViewer, "Y"))
                    {
                        //WindowsEventLoggingUtilities.WriteToInfoEvent("Inside EventViewer section", GetModuleName(), 0);
                        if (File.Exists(EventViewerFileName))
                            File.Delete(EventViewerFileName);
                    }
                }
            }

            return Result;
        }
    }
}
