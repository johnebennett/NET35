/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/06/2019
 * Time: 3:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Serialization;

namespace Actions.SettingsOperations
{
    /// <summary>
    /// Description of CommonMethods.
    /// </summary>
    public class CommonMethods : ActionBase
    {
        private string FModuleName = "SettingsOperations";
        
        public CommonMethods()
        {
        }
        
        public static string Init_Settings_Items(int MasterId,
                                                 RequestCommonItem CommonItem,
                                                 JObject DataObject,
                                                 APISettingsItem SettingsItem,
                                                 string Section,
                                                 ref StringBuilder Entries,
                                                 out string ErrorMsg)
        {
            string Result = "";
            string Method = "Init_Settings_Items";
            ErrorMsg = "";
            
            CommonMethods inst = new CommonMethods();
            if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
            {
                Result = inst.LogOnProcessingError(Method, ErrorMsg, "");
                return Result;
            }

            inst.LogOnInsideMethod(Method);
            try
            {
                string ClientIdValue = (string)DataObject["clientId"];
                int ClientId = StringUtilities.StrToIntDef(ClientIdValue, 0);
                string QueryText = "SELECT * FROM Init_Settings_Items(@CompanyId, @ClientId, @Section)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
                if (ClientId == 0) Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
                else               Params.Add(new FbParameterItem("ClientId", ClientId));
                Params.Add(new FbParameterItem("@Section", Section));
                //WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, inst.FModuleName, 0);
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, MasterId);
                
                /*
                foreach (FbParameterItem Param in Params)
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), Method, MasterId);
                }
                */

                DataTable dt = null;
                try
                {
                    try
                    {
                        dt = FirebirdUtilities.GetFirstDataTable(inst.FbConnectionString, QueryText, Params, 0);
                        if (dt == null)
                        {
                            Result = inst.LogOnProcessingError(Method, "No tables in the dataset to process", "");
                            return Result;
                        }
                    }
                    catch (Exception Ex)
                    {
                        Result = inst.LogOnException(inst.FModuleName + "." + Method, Ex, "");
                        return Result;
                    }

                    foreach (DataRow dataRow in dt.Rows)
                    {
                        Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
                    }
                }
                finally
                {
                    dt.Clear();
                    dt.Dispose();
                }

                //string UpperSection = Section.ToUpper();
                //if (string.IsNullOrEmpty(Result) && ((UpperSection == "ALL") || (UpperSection == "COMMERCIAL")))
                if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(Section, "ALL") ||StringUtilities.SameText(Section, "COMMERCIAL")))
                {
                    NameValueCollection ConvertedCollection = MakeCollectionFromJObject(inst.DataObject, out Result);
                    Result = CommercialSettings.LoadCommercialSettings((ActionBase)inst, ConvertedCollection, ref Entries);
                }
                //if (string.IsNullOrEmpty(Result) && ((UpperSection == "ALL") || (UpperSection == "CHARTOFACCOUNTS")))
                if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(Section, "ALL") || StringUtilities.SameText(Section, "CHARTOFACCOUNTS")))
                {
                    NameValueCollection ConvertedCollection = MakeCollectionFromJObject(inst.DataObject, out Result);
                    Result = ChartOfAccountSettings.LoadSettings((ActionBase)inst, ConvertedCollection, ref Entries);
                }
                if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(Section, "ALL") || StringUtilities.SameText(Section, "SYSTEMSETTINGS")))
                {
                    NameValueCollection ConvertedCollection = MakeCollectionFromJObject(inst.DataObject, out Result);
                    Result = SystemSettings.LoadSettings((ActionBase)inst, ConvertedCollection, ref Entries);
                }
                if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(Section, "ALL") || StringUtilities.SameText(Section, "RECEIPTSETTINGS")))
                {
                    NameValueCollection ConvertedCollection = MakeCollectionFromJObject(inst.DataObject, out Result);
                    Result = ReceiptSettings.LoadSettings((ActionBase)inst, ConvertedCollection, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = inst.LogOnException(inst.FModuleName + "." + Method, Ex, "");
            }
            finally
            {
                inst.LogOnLeavingMethod(Method);
            }
            return Result;
        }
        
        public static string LoadUserSettingsItem(JObject DataObject, ref UserSettingsItem Item)            
        {
            string Result = "";
            try 
            {
                Item.Section = (string)DataObject["section"];
                /*
                Item.OfficeIdValue = (string)DataObject["officeId"];
                Item.ManagerIdValue = (string)DataObject["managerId"];
                Item.PortfolioTypeIdValue = (string)DataObject["portfolioTypes"];
                */
                Item.OfficeIdValue = (string)DataObject["sel_Offices"];
                Item.ManagerIdValue = (string)DataObject["sel_Managers"];
                Item.PortfolioTypeIdValue = (string)DataObject["sel_PortfolioTypes"];
                Item.TransactionOptionValue = (string)DataObject["sel_TransactionOptions"];
                Item.CanEmptyNotepads = (string)DataObject["chk_UserSettings_CanEmptyNotepads"];
                Item.ClientIdValue = (string)DataObject["clientId"];
                Item.DefaultTaskListToMe = (string)DataObject["chk_UserSettings_DefaultTaskListToMe"];
                Item.ToDateDaysAheadValue = (string)DataObject["txt_UserSettings_DefaultToDateDaysAhead"];
                Item.ShowADLButton = (string)DataObject["chk_UserSettings_ShowADLButton"];
                Item.DefaultJobTypeValue = (string)DataObject["sel_DefaultJobTypes"];
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.InnerException + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }
            return Result;
        }
        
        public static string LoadUserSettingsItem(string ConnectionString, Int64 CompanyId, Int64 ClientId, string Section, ref UserSettingsItem Item)
        {
            string Result = "";
            string QueryText =
                " SELECT" +
                "   I_ALogin_PortfolioBranchId AS OfficeId," +
                "   I_ALogin_PortfolioManagerId AS ManagerId," +
                "   I_ALogin_PortfolioPortfolioId AS PortfolioTypeId" +
                " FROM EALogin" +
                " WHERE I_ALogin_CompanyId = @CompanyId" +
                "   AND I_ALogin_ClientId = @ClientId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@CompanyId", CompanyId));
            Params.Add(new FbParameterItem("@ClientId", ClientId));
            DataTable dt = null;
            try 
            {
                Item.Section = Section;
                dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                if (dt == null)
                {
                    Result = "No tables in the dataset to process";
                    return Result;
                }
                
                if (dt.Rows.Count > 0)
                {
                    DataRow Row = dt.Rows[0]; // there should only be 1 row but just in case only use the first row
                    Object Obj = Row["OfficeId"];
                    Item.OfficeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);                    
                    Obj = Row["ManagerId"];
                    Item.ManagerIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);                    
                    Obj = Row["PortfolioTypeId"];
                    Item.PortfolioTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
                }
            }                    
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            return Result;
        }
        
        public static string SaveUserSettingsItem(string ConnectionString, Int64 CompanyId, Int64 ClientId, UserSettingsItem Item)
        {
            string Result = "";
            string QueryText =
                " UPDATE EALogin" +
                "    SET I_ALOGIN_PORTFOLIOBRANCHID = @OfficeId," +
                "        I_ALOGIN_PORTFOLIOMANAGERID = @ManagerId," +
                "        I_ALOGIN_PORTFOLIOPORTFOLIOID = @PortfolioTypeId," +
                "        I_ALOGIN_TRANSACTIONOPTION = @TransactionOption," +
                "        B_ALOGIN_ALLOWEMPTYNOTES = @CanEmptyNotepads," +
                "        B_ALOGIN_DEFAULTTASKLISTOME = @DefaultTaskListToMe," +
                "        I_ALOGIN_DEFAULTTODATEDAYSAHEAD = @DefaultToDateDaysAhead," +
                "        B_ALOGIN_ADL = @ShowADLButton," +
                "        I_ALOGIN_JOBTYPE = @DefaultJobType" +
                " WHERE I_ALogin_CompanyId = @CompanyId" +
                "   AND I_ALogin_ClientId = @ClientId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@OfficeId", Item.OfficeId));
            Params.Add(new FbParameterItem("@ManagerId", Item.ManagerId));
            Params.Add(new FbParameterItem("@PortfolioTypeId", Item.PortfolioTypeId));
            Params.Add(new FbParameterItem("@TransactionOption", Item.TransactionOption));
            Params.Add(new FbParameterItem("@CanEmptyNotepads", Item.CanEmptyNotepads));
            Params.Add(new FbParameterItem("@DefaultTaskListToMe", Item.DefaultTaskListToMe));
            Params.Add(new FbParameterItem("@DefaultToDateDaysAhead", Item.ToDateDaysAhead));
            Params.Add(new FbParameterItem("@ShowADLButton", Item.ShowADLButton));
            Params.Add(new FbParameterItem("@DefaultJobType", Item.DefaultJobType));
            Params.Add(new FbParameterItem("@CompanyId", CompanyId));
            if (Item.ClientId == 0) Params.Add(new FbParameterItem("@ClientId", ClientId));
            else                    Params.Add(new FbParameterItem("@ClientId", Item.ClientId));

            WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "SaveUserSettingsItem", 0);
            
            /*
            foreach (FbParameterItem Param in Params)
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent(
                    Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), "SaveUserSettingsItem", 0);
            }
            */

            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "SaveUserSettingsItem", 0);

            Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            return Result;
        }
        
        public static string LoadPaymentSettingsItem(JObject DataObject, ref PaymentSettingsItem Item)
        {
            string Result = "";
            try 
            {
                Item.Section = (string)DataObject["section"];
                /*
                Item.BankNameIdValue = (string)DataObject["bankNameId"];
                Item.LedgerTypeIdValue = (string)DataObject["ledgerTypeId"];
                Item.PaymentTypeIdValue = (string)DataObject["paymentTypeId"];
                Item.DateRangeIdValue = (string)DataObject["dateRangeId"];
                Item.StatementOrderByIdValue = (string)DataObject["statementOrderById"];
                */
                Item.BankNameIdValue = (string)DataObject["BankTypes"];
                Item.LedgerTypeIdValue = (string)DataObject["LedgerType"];
                Item.PaymentTypeIdValue = (string)DataObject["PaymentType"];
                Item.DateRangeIdValue = (string)DataObject["DateRange"];
                Item.StatementOrderByIdValue = (string)DataObject["StatementOrderByTypes"];
                Item.ClientIdValue = (string)DataObject["clientId"];
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.InnerException + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }
            return Result;
        }
        
        public static string LoadPaymentSettingsItem(string ConnectionString, Int64 CompanyId, string Section, ref PaymentSettingsItem Item)
        {
            string Result = "";
            string QueryText =
                " SELECT" +
                "   I_CO_BankNameId AS BankNameId," +
                "   I_CO_LedgerTypeId AS LedgerTypeId," +
                "   I_CO_PaymentTypeId AS PaymentTypeId," +
                "   I_CO_PaymentDateOptionId AS DateRangeId," +
                "   I_CO_StatementOrderById AS StatementOrderById" +
                " FROM EACompany" +
                " WHERE I_CO_CompanyClientId = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@CompanyId", CompanyId));
            DataTable dt = null;
            try 
            {
                Item.Section = Section;
                dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                if (dt == null)
                {
                    Result = "No tables in the dataset to process";
                    return Result;
                }
                
                if (dt.Rows.Count > 0)
                {
                    DataRow Row = dt.Rows[0]; // there should only be 1 row but just in case only use the first row
                    Object Obj = Row["BankNameId"];
                    Item.BankNameIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);                    
                    Obj = Row["LedgerTypeId"];
                    Item.LedgerTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
                    Obj = Row["PaymentTypeId"];
                    Item.PaymentTypeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
                    Obj = Row["DateRangeId"];
                    Item.DateRangeIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
                    Obj = Row["StatementOrderById"];
                    Item.StatementOrderByIdValue = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
                }
            }                    
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            return Result;
        }
        
        public static string SavePaymentSettingsItem(string ConnectionString, Int64 CompanyId, PaymentSettingsItem Item)
        {
            string Result = "";
            string QueryText =
                " UPDATE EACompany" +
                "    SET I_CO_BankNameId = @BankNameId," +
                "        I_CO_LedgerTypeId = @LedgerTypeId," +
                "        I_CO_PaymentTypeId = @PaymentTypeId," +
                "        I_CO_PaymentDateOptionId = @DateRangeId," +
                "        I_CO_StatementOrderById = @StatementOrderById" +
                " WHERE I_CO_CompanyClientId = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("@BankNameId", Item.BankNameId));
            Params.Add(new FbParameterItem("@LedgerTypeId", Item.LedgerTypeId));
            Params.Add(new FbParameterItem("@PaymentTypeId", Item.PaymentTypeId));
            Params.Add(new FbParameterItem("@DateRangeId", Item.DateRangeId));
            Params.Add(new FbParameterItem("@StatementOrderById", Item.StatementOrderById));
            Params.Add(new FbParameterItem("@CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "SavePaymentSettingsItem", 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            return Result;
        }
        
        public static string BankSettings_SaveDirectEntryFileType(string ConnectionString, 
                                                                  Int64 CompanyId, 
                                                                  int DirectEntryId)
        {
            string Result = "";
            string Method = "BankSettings_SaveDirectEntryFileType";
            string QueryText =
                "UPDATE EACOMPANY SET I_CO_DIRECTENTRYDEFAULT = @directentryid WHERE I_CO_COMPANYCLIENTID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("DirectEntryId", DirectEntryId));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            
            return Result;
        }
        
        public static string BankSettings_SaveDirectEntryCustomerNumber(string ConnectionString,                                                                        
                                                                        Int64 CompanyId,
                                                                        string DirectEntryCustomerNumber)
        {
            string Result = "";
            string Method = "BankSettings_SaveDirectEntryCustomerNumber";
            string QueryText =
                "UPDATE EACOMPANY SET CO_DIRECTENTRYCUSTOMERNUMBER = @DirectEntryCustomerNumber WHERE I_CO_COMPANYCLIENTID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("DirectEntryCustomerNumber", DirectEntryCustomerNumber));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            
            return Result;
        }
        
        public static string BankSettings_SaveDirectDebitCustomerNumber(string ConnectionString,                                                                        
                                                                        Int64 CompanyId,
                                                                        string DirectDebitCustomerNumber)
        {
            string Result = "";
            string Method = "BankSettings_SaveDirectDebitCustomerNumber";
            string QueryText =
                "UPDATE EACOMPANY SET CO_DIRECTDEBITCUSTOMERNUMBER = @DirectDebitCustomerNumber WHERE I_CO_COMPANYCLIENTID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("DirectDebitCustomerNumber", DirectDebitCustomerNumber));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            
            return Result;
        }            
        
        public static string BankSettings_SaveNSWUniqueTrustId(string ConnectionString,
                                                               Int64 CompanyId,
                                                               string NSWUniqueTrustId)
        {
            string Result = "";
            string Method = "BankSettings_SaveNSWUniqueTrustId";
            string QueryText =
                "UPDATE EACOMPANY SET CO_NSW_TRUSTID = @NSWUniqueTrustId WHERE I_CO_COMPANYCLIENTID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("NSWUniqueTrustId", NSWUniqueTrustId));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            
            return Result;
        }            
        
        public static string BankSettings_SaveExecutiveOfficer(string ConnectionString,
                                                               Int64 CompanyId,
                                                               string EffectiveOfficer)
        {
            string Result = "";
            string Method = "BankSettings_SaveExecutiveOfficer";
            string QueryText =
                "UPDATE EACOMPANY SET CO_EFFECTIVE_OFFICER = @EffectiveOfficer WHERE I_CO_COMPANYCLIENTID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("EffectiveOfficer", EffectiveOfficer));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            
            return Result;
        }

        public static string BankSettings_SaveBankImportTypes(string ConnectionString,
                                                               Int64 CompanyId,
                                                               string BankImportTypeId)
        {
            string Result = "";
            string Method = "BankSettings_SaveBankImportTypes";
            string QueryText =
                "UPDATE EACompanySettings2 SET I_CO_BANKIMPORTDEFAULT=@BankImportTypeId where I_COMPANYSETTINGS2_COMPANYID=@CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("BankImportTypeId", BankImportTypeId));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }

            return Result;
        }

        public static string LoadSaveResidentialBondsRequestItem(ActionBase ActionBase,
                                                                 ref SaveResidentialBondsRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSaveResidentialBondsRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
                    
            try 
            {
                WindowsEventLoggingUtilities.WriteToInfoEvent("DataCollection.Count: " + ActionBase.DataCollection.Count.ToString(), Method, 0);
                //Item.BankNameIdValue = (string)DataObject["BankTypes"];
                Object Obj = ActionBase.GetCollectionValue("Data", "BPay_Vic");
                TempItem.BPayVic = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_NSW");
                TempItem.BPayNSW = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_Qld");
                TempItem.BPayQld = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_Act");
                TempItem.BPayACT = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_NT");
                TempItem.BPayNT = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_WA");
                TempItem.BPayWA = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_SA");
                TempItem.BPaySA = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "BPay_Tas");
                TempItem.BPayTas = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "DD_Vic");
                TempItem.DDVic = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_NSW");
                TempItem.DDNSW = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_Qld");
                TempItem.DDQld = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_Act");
                TempItem.DDACT = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_NT");
                TempItem.DDNT = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_WA");
                TempItem.DDWA = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_SA");
                TempItem.DDSA = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "DD_Tas");
                TempItem.DDTas = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "ResInspMonthStart");
                TempItem.ResInspMonthStartValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "InspectionAdvance");
                TempItem.InspectionAdvanceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "RentReviewInitialAdvance");
                TempItem.RentReviewInitialAdvanceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "RentReviewAdvance");
                TempItem.RentReviewAdvanceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CreateLeaseExpiry");
                TempItem.CreateLeaseExpiry = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CreateResVacate");
                TempItem.CreateResVacate = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "LettingFeeTimes");
                TempItem.LettingFeeTimes = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "LettingFeePercentage");
                TempItem.LettingFeePercentage = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "LettingFeeOption");
                TempItem.LettingFeeOptionValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "EndOfYearStatement");
                TempItem.EndOfYearStatement = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SendEndOfYearStmtTo");
                TempItem.SendEndOfYearStmtToValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EndOfYearStmtFee");
                TempItem.EndOfYearStmtFee = (Obj == null) ? "" : Convert.ToString(Obj);

                string Str =
                    "LettingFeeTimes" + Environment.NewLine +
                    "AsLong: " + TempItem.LettingFeeTimes;
                WindowsEventLoggingUtilities.WriteToInfoEvent(Str, Method, 0);
                Str =
                    "LettingFeePercentage" + Environment.NewLine +
                    "AsLong: " + TempItem.LettingFeePercentage;
                WindowsEventLoggingUtilities.WriteToInfoEvent(Str, Method, 0);
                Str =
                    "LettingFeeOption" + Environment.NewLine +
                    "Value: " + TempItem.LettingFeeOptionValue + Environment.NewLine +
                    "AsLong: " + TempItem.LettingFeeOption;
                WindowsEventLoggingUtilities.WriteToInfoEvent(Str, Method, 0);

                Obj = ActionBase.GetCollectionValue("Data", "LettingFeeLedger");
                TempItem.LettingFeeAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AdvertisingFeeLedger");
                TempItem.AdvertisingAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                /*
                    TempItem.BPayVic = (string)DataObject["BPay_Vic"];
                    TempItem.BPayNSW = (string)DataObject["BPay_NSW"];
                    TempItem.BPayQld = (string)DataObject["BPay_Qld"];
                    TempItem.BPayACT = (string)DataObject["BPay_Act"];
                    TempItem.BPayNT = (string)DataObject["BPay_NT"];
                    TempItem.BPayWA = (string)DataObject["BPay_WA"];
                    TempItem.BPaySA = (string)DataObject["BPay_SA"];
                    TempItem.BPayTas = (string)DataObject["BPay_Tas"];


                    TempItem.DDVic = (string)DataObject["DD_Vic"];
                    TempItem.DDNSW = (string)DataObject["DD_NSW"];
                    TempItem.DDQld = (string)DataObject["DD_Qld"];
                    TempItem.DDACT = (string)DataObject["DD_Act"];
                    TempItem.DDNT = (string)DataObject["DD_NT"];
                    TempItem.DDWA = (string)DataObject["DD_WA"];
                    TempItem.DDSA = (string)DataObject["DD_SA"];
                    TempItem.DDTas = (string)DataObject["DD_Tas"];
                    */
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.InnerException + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }
            
            return Result;
        }
        
        public static string SaveResidentialBonds(string ConnectionString, 
                                                  Int64 CompanyId, 
                                                  SaveResidentialBondsRequestItem Item)
        {
            string Result = "";
            string Method = "SaveResidentialBonds";
            string QueryText =
                " UPDATE EACompanySettings" +
                " SET B_CO_BONDBPAYFORVIC = @BPay_Vic," +
                "     B_CO_BONDBPAYFORNSW = @BPay_NSW," +
                "     B_CO_BONDBPAYFORQLD = @BPay_Qld," +
                "     B_CO_BONDBPAYFORACT = @BPay_Act," +
                "     B_CO_BONDBPAYFORNT = @BPay_NT," +
                "     B_CO_BONDBPAYFORWA = @BPay_WA," +
                "     B_CO_BONDBPAYFORSA = @BPay_SA," +
                "     B_CO_BONDBPAYFORTAS = @BPay_Tas," +
                
                "     B_CO_BONDDIRECTDEBITVIC = @DD_Vic," +
                "     B_CO_BONDDIRECTDEBITNSW = @DD_NSW," +
                "     B_CO_BONDDIRECTDEBITQLD = @DD_Qld," +
                "     B_CO_BONDDIRECTDEBITACT = @DD_Act," +
                "     B_CO_BONDDIRECTDEBITNT = @DD_NT," +
                "     B_CO_BONDDIRECTDEBITWA = @DD_WA," +
                "     B_CO_BONDDIRECTDEBITSA = @DD_SA," +
                "     B_CO_BONDDIRECTDEBITTAS = @DD_Tas," +
                "     I_CO_RESINSPMONTHSTART = @ResInspMonthStart," +
                "     I_CO_RENTREVIEWINITIALADVANCE = @RentReviewInitialAdvance," +
                "     B_CO_RESENDOFYEARSTATEMENT = @ResEndOfYearStmt," +
                "     I_CO_RESENDOFYEARSTATEMENTTO = @ResEndOfYearStmtTo," +
                "     M_CO_RESENDOFYEARSTATEMENTFEE = @ResEndOfYearStmtFee" +
                " WHERE I_SETTINGS_COMPANYID = @CompanyId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("BPay_Vic", Item.BPayVic));
            Params.Add(new FbParameterItem("BPay_NSW", Item.BPayNSW));
            Params.Add(new FbParameterItem("BPay_Qld", Item.BPayQld));
            Params.Add(new FbParameterItem("BPay_Act", Item.BPayACT));
            Params.Add(new FbParameterItem("BPay_NT", Item.BPayNT));
            Params.Add(new FbParameterItem("BPay_WA", Item.BPayWA));
            Params.Add(new FbParameterItem("BPay_SA", Item.BPaySA));
            Params.Add(new FbParameterItem("BPay_Tas", Item.BPayTas));
            Params.Add(new FbParameterItem("DD_Vic", Item.DDVic));
            Params.Add(new FbParameterItem("DD_NSW", Item.DDNSW));
            Params.Add(new FbParameterItem("DD_Qld", Item.DDQld));
            Params.Add(new FbParameterItem("DD_Act", Item.DDACT));
            Params.Add(new FbParameterItem("DD_NT", Item.DDNT));
            Params.Add(new FbParameterItem("DD_WA", Item.DDWA));
            Params.Add(new FbParameterItem("DD_SA", Item.DDSA));
            Params.Add(new FbParameterItem("DD_Tas", Item.DDTas));
            Params.Add(new FbParameterItem("ResInspMonthStart", Item.ResInspMonthStart));
            Params.Add(new FbParameterItem("RentReviewInitialAdvance", Item.RentReviewInitialAdvance));
            Params.Add(new FbParameterItem("ResEndOfYearStmt", Item.EndOfYearStatement));
            Params.Add(new FbParameterItem("ResEndOfYearStmtTo", Item.SendEndOfYearStmtTo));
            Params.Add(new FbParameterItem("ResEndOfYearStmtFee", Item.EndOfYearStmtFee));
            Params.Add(new FbParameterItem("CompanyId", CompanyId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            
            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }

            QueryText = "";
            Params.Clear();
            if (string.IsNullOrEmpty(Result))
            {
                QueryText =
                    " UPDATE eacompanysettings2" +
                    " SET I_CO_INSPECTIONADVANCE = @InspectionAdvance," +
                    "     I_CO_RENTREVIEWADVANCE = @RentReviewAdvance," +
                    "     B_CO_CREATELEASEEXPIRY = @CreateLeaseExpiry," +
                    "     B_CO_CREATERESVACATE = @CreateResVacate," +
                    "     M_CO_LFEETIMES = @LFeeTimes," +
                    "     M_CO_LFEEPERCENT = @LFeePercent," +
                    "     I_CO_LFEEOPTION = @LFeeOption," +
                    "     I_CO_LETTINGFEEACCOUNTID = @LETTINGFEEACCOUNTID," +
                    "     I_CO_ADVERTISINGACCOUNTID = @ADVERTISINGACCOUNTID" +
                    " WHERE I_COMPANYSETTINGS2_COMPANYID = @CompanyId";
                Params.Add(new FbParameterItem("InspectionAdvance", Item.InspectionAdvance));
                Params.Add(new FbParameterItem("RentReviewAdvance", Item.RentReviewAdvance));
                Params.Add(new FbParameterItem("CreateLeaseExpiry", Item.CreateLeaseExpiry));
                Params.Add(new FbParameterItem("CreateResVacate", Item.CreateResVacate));
                Params.Add(new FbParameterItem("LFeeTimes", Item.LettingFeeTimes));
                Params.Add(new FbParameterItem("LFeePercent", Item.LettingFeePercentage));
                Params.Add(new FbParameterItem("LFeeOption", Item.LettingFeeOption));
                Params.Add(new FbParameterItem("LETTINGFEEACCOUNTID", Item.LettingFeeAccountId));
                Params.Add(new FbParameterItem("ADVERTISINGACCOUNTID", Item.AdvertisingAccountId));
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            }

            if (string.IsNullOrEmpty(Result))
            {
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }

            return Result;
        }
        
        public static NameValueCollection MakeCollectionFromJObject(JObject DataObject,
                                                                    out string ErrorMsg)
        {
            string Method = "MakeCollectionFromJObject";
            ErrorMsg = "";
            NameValueCollection Result = new NameValueCollection();            
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
            
            ActionBase ActionBase = new ActionBase();
            
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                foreach (KeyValuePair<string, JToken> Item in DataObject)
                {
                    string name = Item.Key;
                    JToken value = Item.Value;
                    string StrValue = (value == null) ? "": Convert.ToString(value);
                    Result.Add(name, StrValue);
                }
            }
            catch (Exception Ex)
            {
                ErrorMsg = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }
            
            return Result;
        }
        
        public static string LoadSaveCommercialSettingsRequestItem(ActionBase ActionBase,
                                                                   ref SaveCommercialSettingsRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSaveCommercialSettingsRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
                    
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                /*
                    Object Obj = ActionBase.GetCollectionValue("Data", "MinTaxInvoiceAmount");
                    TempItem.MinTaxInvoiceAmountValue = (string)(ActionBase.GetCollectionValue("Data", "MinTaxInvoiceAmount", "0"));
                    //Obj = ActionBase.DataObject["MinTaxInvoiceAmount"];
                    TempItem.MinTaxInvoiceAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
                */                  
                Object Obj = ActionBase.GetCollectionValue("Data", "MinTaxInvoiceAmount");
                TempItem.MinTaxInvoiceAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComInspectionAdvance");
                TempItem.ComInspectionAdvanceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComLeaseExpiry");
                TempItem.ComLeaseExpiry = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComVacateDate");
                TempItem.ComVacateDate = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComRentalReview");
                TempItem.ComRentalReview = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComOptionBegin");
                TempItem.ComOptionBegin = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ComOptionEnd");
                TempItem.ComOptionEnd = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "LettingFeeLedger");
                TempItem.LettingFeeAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AdvertisingFeeLedger");
                TempItem.AdvertisingAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "EndOfYearStatement");
                TempItem.EndOfYearStatement = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "SendEndOfYearStmtTo");
                TempItem.SendEndOfYearStmtToValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EndOfYearStmtFee");
                TempItem.EndOfYearStmtFee = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "EstOutgoingsAddPercent");
                TempItem.EstOutgoingsAddPercent = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "EstOutgoingsSource");
                TempItem.EstOutgoingsSourceValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
        
        public static string EAChartCodeValidate(string ConnectionString, Int64 CompanyId, int RecordId, string ChartCode)
        {
            string Result = "";
            string Method = "EAChartCodeValidate";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
                    
            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try 
            {
                string QueryText = "select okmessage from EACHARTCODEVALIDATE(@CompanyId, @RecordId, @ChartCode)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyID", CompanyId));
                Params.Add(new FbParameterItem("RecordId", RecordId));
                Params.Add(new FbParameterItem("ChartCode", ChartCode));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                
                DataTable tbl = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                }
                
                if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count > 0))
                {
                    object Obj = tbl.Rows[0]["okmessage"];
                    Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    if (StringUtilities.SameText(Result, "OK")) Result = "";
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
        
        public static string LoadSaveChartOfAccountEditRequestItem(ActionBase ActionBase,
                                                                   ref SaveChartOfAccountEditRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSaveChartOfAccountEditRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
                    
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                    Object Obj = ActionBase.GetCollectionValue("Data", "RecordId");
                    TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "DlgMode");
                    TempItem.DlgModeValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Description");
                    TempItem.Description = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "Account");
                    TempItem.Account = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "NumberOrder");
                    TempItem.NumberOrderValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Order2");
                    TempItem.Order2Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Order3");
                    TempItem.Order3Value = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Order2Id");
                    TempItem.Order2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Order3Id");
                    TempItem.Order3IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    
                    // Checkboxes - No validation done on these items here
                    Obj = ActionBase.GetCollectionValue("Data", "Hide");
                    TempItem.Hide = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Advertising");
                    TempItem.Advertising = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "CommissionOnRental");
                    TempItem.CommissionOnRental = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "TenancySchedule");
                    TempItem.TenancySchedule = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Outgoings");
                    TempItem.Outgoings = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "NotOnBAS");
                    TempItem.NotOnBAS = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "SecurityDeposit");
                    TempItem.SecurityDeposit = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "CommissionOnSale");
                    TempItem.CommissionOnSale = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Insurance");
                    TempItem.Insurance = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "PenaltyInterest");
                    TempItem.PenaltyInterest = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "DoNotChargeInterest");
                    TempItem.DoNotChargeInterest = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "GroupOnStatement");
                    TempItem.GroupOnStatement = (Obj == null) ? "" : Convert.ToString(Obj);
                    
                    Obj = ActionBase.GetCollectionValue("Data", "InvoiceHandling");
                    TempItem.InvoiceHandling = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Maintenance");
                    TempItem.Maintenance = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "Compliance");
                    TempItem.Compliance = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string LoadSaveReceiptSettingsRequestItem(ActionBase ActionBase,
                                                                ref SaveReceiptSettingsRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSaveReceiptSettingsRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "PrintCashChequeRec");
                TempItem.PrintCashChequeRec = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "PrintDirectRec");
                TempItem.PrintDirectRec = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "AutoInsertAgentNumbersForResidential");
                TempItem.AutoInsertAgentNumbersForResidential = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AutoInsertAgentNumbersForCommercial");
                TempItem.AutoInsertAgentNumbersForCommercial = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AutoInsertAgentNumbersforStrataLotOwners");
                TempItem.AutoInsertAgentNumbersforStrataLotOwners = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AutoAgentNumberAddCheckDigit");
                TempItem.AutoAgentNumberAddCheckDigit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AutoAgentNumberMinimumLength");
                TempItem.AutoAgentNumberMinimumLengthValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string LoadSaveSystemSettingsRequestItem(ActionBase ActionBase,
                                                               ref SaveSystemSettingsRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSaveSystemSettingsRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = ActionBase.GetCollectionValue("Data", "SalesCommToLedger");
                TempItem.SaleCommToLedgerValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ResLettingFeeRevenue");
                TempItem.ResLettingFeeRevenueValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ResLettingFeeExpense");
                TempItem.ResLettingFeeExpenseValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AdvertisingExpense");
                TempItem.AdvertisingExpenseValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AdvertisingRevenue");
                TempItem.AdvertisingRevenueValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CommercialLettingFeeExpense");
                TempItem.CommercialLettingFeeExpenseValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "CommercialLettingFeeRevenue");
                TempItem.CommercialLettingFeeRevenueValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AutoPostApproved");
                TempItem.AutoPostApproved = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Use2FA");
                TempItem.Use2FA = (Obj == null) ? "" : Convert.ToString(Obj);
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
