using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
    public class ReceiptSettings : ActionBase
    {
        //private string FModuleName = "SettingsOperations";

        public ReceiptSettings()
        {
        }

        public static string GetModuleName() {  return "SettingsOperations.ReceiptSettings.";  }

        public static string LoadSettings(ActionBase ActionBase,
                                          NameValueCollection DataCollection,
                                          ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSettings";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder ReceiptEntries = new StringBuilder();
                Result = GatherSettings(ActionBase, DataCollection, ref ReceiptEntries);

                if (string.IsNullOrEmpty(Result))
                {
                    if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
                    //Entries.Append(JsonUtilities.StartJsonObject("ReceiptSettings"));
                    Entries.Append(ReceiptEntries.ToString());
                    //Entries.Append(JsonUtilities.EndJsonObject());
                }
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

        public static string GatherSettings(ActionBase ActionBase,
                                            NameValueCollection DataCollection,
                                            ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "GatherSettings";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT OutputLine FROM Init_Settings_Receipts(@CompanyId, @ClientId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
                    Params.Add(new FbParameterItem("ClientId", ActionBase.CommonItem.ClientIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
                }

                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    object Obj;
                    if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                    foreach (DataRow Row in tbl.Rows)
                    {
                        Obj = Row["OUTPUTLINE"];
                        Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                    }
                }
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

        public static string SaveSettings(ActionBase ActionBase, 
                                          SaveReceiptSettingsRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "SaveSettings";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                     QueryText =
                        " UPDATE eacompanysettings" +
                        " SET B_CO_PRINTCASHCHEQUEREC = @PrintCashChequeRec," +
                        "     B_CO_PRINTDIRECTREC = @PrintDirectRec," +
                        "     B_CO_ADDCHECKDIGIT = @AddCheckDigit," +
                        "     B_CO_AUTONUMBERRES = @AutoNumberRes," +
                        "     B_CO_AUTONUMBERCOM = @AutoNumberCom," +
                        "     B_CO_AUTONUMBERSTR = @AutoNumberStrata," +
                        "     I_CO_AGENTNOLEN = @AgentNoLength" +
                        " WHERE I_SETTINGS_COMPANYID = @CompanyId";
                    Params.Add(new FbParameterItem("PrintCashChequeRec", RequestItem.PrintCashChequeRec));
                    Params.Add(new FbParameterItem("PrintDirectRec", RequestItem.PrintDirectRec));
                    Params.Add(new FbParameterItem("AddCheckDigit", RequestItem.AutoAgentNumberAddCheckDigit));
                    Params.Add(new FbParameterItem("AutoNumberRes", RequestItem.AutoInsertAgentNumbersForResidential));
                    Params.Add(new FbParameterItem("AutoNumberCom", RequestItem.AutoInsertAgentNumbersForCommercial));
                    Params.Add(new FbParameterItem("AutoNumberStrata", RequestItem.AutoInsertAgentNumbersforStrataLotOwners));
                    Params.Add(new FbParameterItem("AgentNoLength", RequestItem.AutoAgentNumberMinimumLength));
                    Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
                }
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
    }
}
