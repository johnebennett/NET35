/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/08/2020
 * Time: 10:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.ToDo.Job
{
    /// <summary>
    /// Description of Save.
    /// </summary>
    public class SaveChanges : ActionBase
    {
        private string FModuleName = "ToDo";
        private AllPageEntriesRequestItem FRequestItem;

        public static string Execute(int MasterId,
                                     APISettingsItem SettingsItem,
                                     string Request,
                                     out string ErrorMsg)
        {
            string Method = "Execute";
            string Result = "";
            ErrorMsg = "";

            SaveChanges inst = new SaveChanges();
            if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
                return "";

            inst.LogOnInsideMethod(Method);
            try
            {
                Result = inst.Init();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Validate();
                if (!string.IsNullOrEmpty(Result))
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
                    return "";
                }

                Result = inst.Output();
                UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
            }
            finally
            {
                inst.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        private string Init()
        {
            string Method = "Init";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                FRequestItem = new AllPageEntriesRequestItem();
                Result = FRequestItem.Load((ActionBase)this);
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Validate()
        {
            string Method = "Validate";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                Result = FRequestItem.Validate();
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            return Result;
        }

        private string Output()
        {
            string Method = "Output";
            string Result = "";
            string ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
            LogOnInsideMethod(Method);
            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.SaveChanges(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem);
                }

                SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Item.Load((ActionBase)this);
                }

                StringBuilder Entries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    ErrorMsg =
                        CommonMethods.SetupPageEntries(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            Item, ref Entries);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                }
                else
                {
                    Result = LogOnProcessingError(Method, Result, "");
                }
            }
            catch (Exception Ex)
            {
                Result = LogOnException(Method, Ex, "");
            }
            finally
            {
                LogOnLeavingMethod(Method);
            }
            WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
            return Result;
        }
    }
}