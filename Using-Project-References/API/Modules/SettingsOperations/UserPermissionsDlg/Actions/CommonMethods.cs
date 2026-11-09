using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.SettingsOperations.UserPermissionsDlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.UserPermissionsDlg."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
                                                             ref SetupPageEntriesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadSetupPageEntriesRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "ClientId");
                TempItem.ClientIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        /// <summary>
        /// Setups the page entries.
        /// NOTE: The ClientId here is NOT the logged in user
        /// it is the user for which the methods are called for as
        /// not all users have access to the Settings menu item
        /// </summary>
        /// <returns>The page entries.
        /// </returns>
        /// <param name="ConnectionString">Connection string.</param>
        /// <param name="CompanyId">Company identifier.</param>
        /// <param name="ClientId">Client identifier.</param>
        /// <param name="Entries">Entries.</param>
        public static string SetupPageEntries(string ConnectionString,
                                      int CompanyId,
                                      int ClientId,
                                      ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "SELECT OutputLine FROM EALOGINEDITGROUP(@CompanyId, @ClientId)";
                    Params.Clear();
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                    /*
                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                    }
                    */
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["OUTPUTLINE"];
                            Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
                        }
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadAllPageDataRequestItem(ActionBase ActionBase,
                                                             ref AllPageDataRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadAllPageDataRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "ClientId");
                TempItem.ClientIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ClientName");
                TempItem.ClientName = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "Login");
                TempItem.Login = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "IsABranch");
                TempItem.IsABranch = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "IsAManager");
                TempItem.IsAManager = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "IsAnAuditor");
                TempItem.IsAnAuditor = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "MgmtGridPMView");
                TempItem.MgmtGridPMView = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "IsActive");
                TempItem.IsActive = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowPortfolio");
                TempItem.ShowPortfolio = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowToDoList");
                TempItem.ShowToDoList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowDebtsList");
                TempItem.ShowDebtsList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowInvoicesList");
                TempItem.ShowInvoicesList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowReceiptsList");
                TempItem.ShowReceiptsList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowPaymentsList");
                TempItem.ShowPaymentsList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowLedgerList");
                TempItem.ShowLedgerList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowReconciliationList");
                TempItem.ShowReconciliationList = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowSettingsList");
                TempItem.ShowSettingsList = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsAdd");
                TempItem.AllowDebtsAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsEdit");
                TempItem.AllowDebtsEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsReverse");
                TempItem.AllowDebtsReverse = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsPeriodicalAdd");
                TempItem.AllowDebtsPeriodicalAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsPeriodicalEdit");
                TempItem.AllowDebtsPeriodicalEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtsPeriodicalInActive");
                TempItem.AllowDebtsPeriodicalInActive = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtIncreasesAdd");
                TempItem.AllowDebtIncreasesAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtIncreasesEdit");
                TempItem.AllowDebtIncreasesEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowDebtIncreasesRemove");
                TempItem.AllowDebtIncreasesRemove = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowInvoiceEdit");
                TempItem.AllowInvoiceEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowInvoiceReverse");
                TempItem.AllowInvoiceReverse = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowPayeeAdd");
                TempItem.AllowPayeeAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowPayeeEdit");
                TempItem.AllowPayeeEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowLedgerAdd");
                TempItem.AllowLedgerAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowLedgerEdit");
                TempItem.AllowLedgerEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowLedgerReverse");
                TempItem.AllowLedgerReverse = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "AllowBudgetAdd");
                TempItem.AllowBudgetAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowBudgetEdit");
                TempItem.AllowBudgetEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowBudgetReverse");
                TempItem.AllowBudgetReverse = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowInvoicePeriodicalAdd");
                TempItem.AllowInvoicePeriodicalAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowInvoicePeriodicalEdit");
                TempItem.AllowInvoicePeriodicalEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowInvoicePeriodicalInActive");
                TempItem.AllowInvoicePeriodicalInActive = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowFeesAdd");
                TempItem.AllowFeesAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowFeesEdit");
                TempItem.AllowFeesEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowFeesReverse");
                TempItem.AllowFeesReverse = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowSubLedgerAdd");
                TempItem.AllowSubLedgerAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowSubLedgerEdit");
                TempItem.AllowSubLedgerEdit = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowSubLedgerReverse");
                TempItem.AllowSubLedgerReverse = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowChangeBranch");
                TempItem.AllowChangeBranch = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowChangeManager");
                TempItem.AllowChangeManager = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowStorageAdd");
                TempItem.AllowStorageAdd = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowStorageRemove");
                TempItem.AllowStorageRemove = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "AllowReceiptsEdit");
                TempItem.AllowReceiptsEdit = (Obj == null) ? "" : Convert.ToString(Obj);

                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonSalesDepositRelease");
                TempItem.ShowButtonSalesDepositRelease = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonSalesReleaseFees");
                TempItem.ShowButtonSalesReleaseFees = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonOCCertificate");
                TempItem.ShowButtonOCCertificate = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonOCReports");
                TempItem.ShowButtonOCReports = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonOCMultiSetup");
                TempItem.ShowButtonOCMultiSetup = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonOCRollover");
                TempItem.ShowButtonOCRollover = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonOCAccruals");
                TempItem.ShowButtonOCAccruals = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonCommercialReports");
                TempItem.ShowButtonCommercialReports = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonCompanyReports");
                TempItem.ShowButtonCompanyReports = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonPropMgmtVacate");
                TempItem.ShowButtonPropMgmtVacate = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonPropMgmtBond");
                TempItem.ShowButtonPropMgmtBond = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonPropMgmtAgentNo");
                TempItem.ShowButtonPropMgmtAgentNo = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonPropMgmtKeys");
                TempItem.ShowButtonPropMgmtKeys = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ShowButtonPropMgmtDirectDebit");
                TempItem.ShowButtonPropMgmtDirectDebit = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string SaveChanges(string ConnectionString,
                                         Int64 CompanyId,
                                         AllPageDataRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "SaveChanges";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);


            ActionBase ActionBase = new ActionBase();
            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        SetupPageEntries(ConnectionString, (int)CompanyId, RequestItem.ClientId, ref TempEntries);
                }

                string Value = "";
                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL1", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = PostSql1(ConnectionString, CompanyId, QueryText, RequestItem);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = PostSql2(ConnectionString, CompanyId, QueryText, RequestItem);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third Post query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = PostSql3(ConnectionString, CompanyId, QueryText, RequestItem);
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

        public static string PostSql1(string ConnectionString,
                                      Int64 CompanyId,
                                      string QueryText,
                                      AllPageDataRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "PostSql1";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("USERNAME", RequestItem.Login));
                    Params.Add(new FbParameterItem("BRANCH", RequestItem.IsABranch));
                    Params.Add(new FbParameterItem("MANAGER", RequestItem.IsAManager));
                    Params.Add(new FbParameterItem("IsAuditor", RequestItem.IsAnAuditor));

                    Params.Add(new FbParameterItem("ACTIVE", RequestItem.IsActive));
                    Params.Add(new FbParameterItem("SHOWPortfolio", RequestItem.ShowPortfolio));
                    Params.Add(new FbParameterItem("SHOWTODO", RequestItem.ShowToDoList));
                    Params.Add(new FbParameterItem("SHOWDEBTS", RequestItem.ShowDebtsList));

                    Params.Add(new FbParameterItem("SHOWINVOICES", RequestItem.ShowInvoicesList));
                    Params.Add(new FbParameterItem("SHOWRECEIPTS", RequestItem.ShowReceiptsList));
                    Params.Add(new FbParameterItem("SHOWPAYMENTS", RequestItem.ShowPaymentsList));

                    Params.Add(new FbParameterItem("SHOWLEDGER", RequestItem.ShowLedgerList));
                    Params.Add(new FbParameterItem("SHOWRECONCILIATION", RequestItem.ShowReconciliationList));
                    Params.Add(new FbParameterItem("SHOWSETTINGS", RequestItem.ShowSettingsList));

                    Params.Add(new FbParameterItem("LEDGERADD", RequestItem.AllowLedgerAdd));
                    Params.Add(new FbParameterItem("LEDGEREDIT", RequestItem.AllowLedgerEdit));
                    Params.Add(new FbParameterItem("LEDGERREV", RequestItem.AllowLedgerReverse));

                    Params.Add(new FbParameterItem("CHARGEADD", RequestItem.AllowDebtsAdd));
                    Params.Add(new FbParameterItem("CHARGEEDIT", RequestItem.AllowDebtsEdit));
                    Params.Add(new FbParameterItem("CHARGEREV", RequestItem.AllowDebtsReverse));

                    Params.Add(new FbParameterItem("INVOICEEDIT", RequestItem.AllowInvoiceEdit));
                    Params.Add(new FbParameterItem("INVOICEREV", RequestItem.AllowInvoiceReverse));
                    Params.Add(new FbParameterItem("RECEDIT", RequestItem.AllowReceiptsEdit));

                    Params.Add(new FbParameterItem("CHANGEBRANCH", RequestItem.AllowChangeBranch));
                    Params.Add(new FbParameterItem("CHANGEMANAGER", RequestItem.AllowChangeManager));

                    Params.Add(new FbParameterItem("STORAGEADD", RequestItem.AllowStorageAdd));
                    Params.Add(new FbParameterItem("STORAGEREMOVE", RequestItem.AllowStorageRemove));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
            }

            return Result;
        }

        public static string PostSql2(string ConnectionString,
                                      Int64 CompanyId,
                                      string QueryText,
                                      AllPageDataRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "PostSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    /*
                     ALOGIN_USERNAME=@USERNAME ,B_ALOGIN_BUDGETADD=@BUDGETADD ,B_ALOGIN_BUDGETEDIT=@BUDGETEDIT ,
                     B_ALOGIN_BUDGETREV=@BUDGETREV ,B_ALOGIN_FEESADD=@FEESADD ,B_ALOGIN_FEESEDIT=@FEESEDIT ,
                     B_ALOGIN_FEESREMOVE=@FEESREMOVE ,B_ALOGIN_PAYEEADD=@PAYEEADD ,B_ALOGIN_PAYEEEDIT=@PAYEEEDIT ,
                     B_ALOGIN_SUBLEDGERADD=@SUBLEDGERADD ,B_ALOGIN_SUBLEDGEREDIT=@SUBLEDGEREDIT ,B_ALOGIN_SUBLEDGERREMO=@SUBLEDGERREMO ,
                     B_ALOGIN_DEBTPERADD=@DEBTPERADD ,B_ALOGIN_DEBTPEREDIT=@DEBTPEREDIT ,B_ALOGIN_DEBTPERINAC=@DEBTPERINAC ,
                     B_ALOGIN_INVPERADD=@INVPERADD ,B_ALOGIN_INVPEREDIT=@INVPEREDIT ,
                     B_ALOGIN_INVPERINAC=@INVPERINAC ,
                     B_ALOGIN_DEBTINCADD=@DEBTINCADD ,B_ALOGIN_DEBTINCEDIT=@DEBTINCEDIT ,B_ALOGIN_DEBTINCREMOVE=@DEBTINCREMOVE"
                    */
                    Params.Add(new FbParameterItem("USERNAME", RequestItem.Login));
                    Params.Add(new FbParameterItem("BUDGETADD", RequestItem.AllowBudgetAdd));
                    Params.Add(new FbParameterItem("BUDGETEDIT", RequestItem.AllowBudgetEdit));

                    Params.Add(new FbParameterItem("BUDGETREV", RequestItem.AllowBudgetReverse));
                    Params.Add(new FbParameterItem("FEESADD", RequestItem.AllowFeesAdd));
                    Params.Add(new FbParameterItem("FEESEDIT", RequestItem.AllowFeesEdit));

                    Params.Add(new FbParameterItem("FEESREMOVE", RequestItem.AllowFeesReverse));
                    Params.Add(new FbParameterItem("PAYEEADD", RequestItem.AllowPayeeAdd));
                    Params.Add(new FbParameterItem("PAYEEEDIT", RequestItem.AllowPayeeEdit));

                    Params.Add(new FbParameterItem("SUBLEDGERADD", RequestItem.AllowSubLedgerAdd));
                    Params.Add(new FbParameterItem("SUBLEDGEREDIT", RequestItem.AllowSubLedgerEdit));
                    Params.Add(new FbParameterItem("SUBLEDGERREMO", RequestItem.AllowSubLedgerReverse));

                    Params.Add(new FbParameterItem("DEBTPERADD", RequestItem.AllowDebtsPeriodicalAdd));
                    Params.Add(new FbParameterItem("DEBTPEREDIT", RequestItem.AllowDebtsPeriodicalEdit));
                    Params.Add(new FbParameterItem("DEBTPERINAC", RequestItem.AllowDebtsPeriodicalInActive));

                    Params.Add(new FbParameterItem("INVPERADD", RequestItem.AllowInvoicePeriodicalAdd));
                    Params.Add(new FbParameterItem("INVPEREDIT", RequestItem.AllowInvoicePeriodicalEdit));
                    Params.Add(new FbParameterItem("INVPERINAC", RequestItem.AllowInvoicePeriodicalInActive));

                    Params.Add(new FbParameterItem("DEBTINCADD", RequestItem.AllowDebtIncreasesAdd));
                    Params.Add(new FbParameterItem("DEBTINCEDIT", RequestItem.AllowDebtIncreasesEdit));
                    Params.Add(new FbParameterItem("DEBTINCREMOVE", RequestItem.AllowDebtIncreasesRemove));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
            }

            return Result;
        }

        public static string PostSql3(string ConnectionString,
                                      Int64 CompanyId,
                                      string QueryText,
                                      AllPageDataRequestItem RequestItem)
        {
            string Result = "";
            string Method = GetModuleName() + "PostSql3";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            try
            {
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    /*
                     ALOGIN_USERNAME=@USERNAME ,B_ALOGIN_DEPRELEASE=@DEPOSITRELEASE ,B_ALOGIN_RELEASEFEES=@RELEASEFEES ,
                     B_ALOGIN_OCCERTIFICATE=@OCCERTIFICATE ,B_ALOGIN_OCREPORTS=@OCREPORTS ,B_ALOGIN_OCMULTISETUP=@OCMULTISETUP ,
                     B_ALOGIN_OCROLLOVER=@OCROLLOVER ,B_ALOGIN_OCACCRUALS=@OCACCRUALS ,B_ALOGIN_COMMREPORTS=@COMMREPORTS ,
                     B_ALOGIN_COMPREPORTS=@COMPREPORTS ,B_ALOGIN_PROPVACATE=@PROPVACATE ,B_ALOGIN_PROPBOND=@PROPBOND ,
                     B_ALOGIN_PROPAGENTNO=@PROPAGENTNO ,B_ALOGIN_PROPKEYS=@PROPKEYS ,B_ALOGIN_PROPDIRECTDEBIT=@PROPDIRECTDEBIT"
                    */
                    Params.Add(new FbParameterItem("USERNAME", RequestItem.Login));
                    Params.Add(new FbParameterItem("DEPOSITRELEASE", RequestItem.ShowButtonSalesDepositRelease));
                    Params.Add(new FbParameterItem("RELEASEFEES", RequestItem.ShowButtonSalesReleaseFees));

                    Params.Add(new FbParameterItem("OCCERTIFICATE", RequestItem.ShowButtonOCCertificate));
                    Params.Add(new FbParameterItem("OCREPORTS", RequestItem.ShowButtonOCReports));
                    Params.Add(new FbParameterItem("OCMULTISETUP", RequestItem.ShowButtonOCMultiSetup));

                    Params.Add(new FbParameterItem("OCROLLOVER", RequestItem.ShowButtonOCRollover));
                    Params.Add(new FbParameterItem("OCACCRUALS", RequestItem.ShowButtonOCAccruals));
                    Params.Add(new FbParameterItem("COMMREPORTS", RequestItem.ShowButtonCommercialReports));

                    Params.Add(new FbParameterItem("COMPREPORTS", RequestItem.ShowButtonCompanyReports));
                    Params.Add(new FbParameterItem("PROPVACATE", RequestItem.ShowButtonPropMgmtVacate));
                    Params.Add(new FbParameterItem("PROPBOND", RequestItem.ShowButtonPropMgmtBond));

                    Params.Add(new FbParameterItem("PROPAGENTNO", RequestItem.ShowButtonPropMgmtAgentNo));
                    Params.Add(new FbParameterItem("PROPKEYS", RequestItem.ShowButtonPropMgmtKeys));
                    Params.Add(new FbParameterItem("PROPDIRECTDEBIT", RequestItem.ShowButtonPropMgmtDirectDebit));

                    Params.Add(new FbParameterItem("PMView", RequestItem.MgmtGridPMView));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
            }

            return Result;
        }
    }
}
