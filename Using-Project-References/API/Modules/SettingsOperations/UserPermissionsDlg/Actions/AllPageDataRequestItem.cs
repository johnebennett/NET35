/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 11-04-2021
 * Time: 12:03 PM

 * SourceFile: /home/john/Source/EAOffice/EAOfficeAPI/Modules/SettingsOperations/UserPermissionsDlg/Actions/Templates/AllPageDataRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.SettingsOperations.UserPermissionsDlg
{
    public class AllPageDataRequestItem
    {
        #region Properties - Getters and Setters
        private string FClientIdValue = "";
        public bool ClientIdIsValid = false;
        public int ClientId { get; set; }
        public string ClientIdValue
        {
            get { return FClientIdValue; }
            set
            {
                FClientIdValue = value;
                int Dummy = 0;
                ClientIdIsValid = int.TryParse(FClientIdValue, out Dummy);
                ClientId = (ClientIdIsValid) ? Dummy : 0;
            }
        }
        
        public string ClientName { get; set; }
        
        public string Login { get; set; }
        
        public string IsABranch { get; set; }
        
        public string IsAManager { get; set; }
        
        public string IsAnAuditor { get; set; }
        
        public string MgmtGridPMView { get; set; }
        
        public string IsActive { get; set; }
        
        public string ShowPortfolio { get; set; }
        
        public string ShowToDoList { get; set; }
        
        public string ShowDebtsList { get; set; }
        
        public string ShowInvoicesList { get; set; }
        
        public string ShowReceiptsList { get; set; }
        
        public string ShowPaymentsList { get; set; }
        
        public string ShowLedgerList { get; set; }
        
        public string ShowReconciliationList { get; set; }
        
        public string ShowSettingsList { get; set; }
        
        public string AllowDebtsAdd { get; set; }
        
        public string AllowDebtsEdit { get; set; }
        
        public string AllowDebtsReverse { get; set; }
        
        public string AllowDebtsPeriodicalAdd { get; set; }
        
        public string AllowDebtsPeriodicalEdit { get; set; }
        
        public string AllowDebtsPeriodicalInActive { get; set; }
        
        public string AllowDebtIncreasesAdd { get; set; }
        
        public string AllowDebtIncreasesEdit { get; set; }
        
        public string AllowDebtIncreasesRemove { get; set; }
        
        public string AllowInvoiceEdit { get; set; }
        
        public string AllowInvoiceReverse { get; set; }
        
        public string AllowPayeeAdd { get; set; }
        
        public string AllowPayeeEdit { get; set; }
        
        public string AllowLedgerAdd { get; set; }
        
        public string AllowLedgerEdit { get; set; }
        
        public string AllowLedgerReverse { get; set; }
        
        public string AllowBudgetAdd { get; set; }
        
        public string AllowBudgetEdit { get; set; }
        
        public string AllowBudgetReverse { get; set; }
        
        public string AllowInvoicePeriodicalAdd { get; set; }
        
        public string AllowInvoicePeriodicalEdit { get; set; }
        
        public string AllowInvoicePeriodicalInActive { get; set; }
        
        public string AllowFeesAdd { get; set; }
        
        public string AllowFeesEdit { get; set; }
        
        public string AllowFeesReverse { get; set; }
        
        public string AllowSubLedgerAdd { get; set; }
        
        public string AllowSubLedgerEdit { get; set; }
        
        public string AllowSubLedgerReverse { get; set; }
        
        public string AllowChangeBranch { get; set; }
        
        public string AllowChangeManager { get; set; }
        
        public string AllowStorageAdd { get; set; }
        
        public string AllowStorageRemove { get; set; }
        
        public string AllowReceiptsEdit { get; set; }
        
        public string ShowButtonSalesDepositRelease { get; set; }
        
        public string ShowButtonSalesReleaseFees { get; set; }
        
        public string ShowButtonOCCertificate { get; set; }
        
        public string ShowButtonOCReports { get; set; }
        
        public string ShowButtonOCMultiSetup { get; set; }
        
        public string ShowButtonOCRollover { get; set; }
        
        public string ShowButtonOCAccruals { get; set; }
        
        public string ShowButtonCommercialReports { get; set; }
        
        public string ShowButtonCompanyReports { get; set; }
        
        public string ShowButtonPropMgmtVacate { get; set; }
        
        public string ShowButtonPropMgmtBond { get; set; }
        
        public string ShowButtonPropMgmtAgentNo { get; set; }
        
        public string ShowButtonPropMgmtKeys { get; set; }
        
        public string ShowButtonPropMgmtDirectDebit { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            ClientIdValue = "";
            ClientName = "";
            Login = "";
            IsABranch = "";
            IsAManager = "";
            IsAnAuditor = "";
            MgmtGridPMView = "";
            IsActive = "";
            ShowPortfolio = "";
            ShowToDoList = "";
            ShowDebtsList = "";
            ShowInvoicesList = "";
            ShowReceiptsList = "";
            ShowPaymentsList = "";
            ShowLedgerList = "";
            ShowReconciliationList = "";
            ShowSettingsList = "";
            AllowDebtsAdd = "";
            AllowDebtsEdit = "";
            AllowDebtsReverse = "";
            AllowDebtsPeriodicalAdd = "";
            AllowDebtsPeriodicalEdit = "";
            AllowDebtsPeriodicalInActive = "";
            AllowDebtIncreasesAdd = "";
            AllowDebtIncreasesEdit = "";
            AllowDebtIncreasesRemove = "";
            AllowInvoiceEdit = "";
            AllowInvoiceReverse = "";
            AllowPayeeAdd = "";
            AllowPayeeEdit = "";
            AllowLedgerAdd = "";
            AllowLedgerEdit = "";
            AllowLedgerReverse = "";
            AllowBudgetAdd = "";
            AllowBudgetEdit = "";
            AllowBudgetReverse = "";
            AllowInvoicePeriodicalAdd = "";
            AllowInvoicePeriodicalEdit = "";
            AllowInvoicePeriodicalInActive = "";
            AllowFeesAdd = "";
            AllowFeesEdit = "";
            AllowFeesReverse = "";
            AllowSubLedgerAdd = "";
            AllowSubLedgerEdit = "";
            AllowSubLedgerReverse = "";
            AllowChangeBranch = "";
            AllowChangeManager = "";
            AllowStorageAdd = "";
            AllowStorageRemove = "";
            AllowReceiptsEdit = "";
            ShowButtonSalesDepositRelease = "";
            ShowButtonSalesReleaseFees = "";
            ShowButtonOCCertificate = "";
            ShowButtonOCReports = "";
            ShowButtonOCMultiSetup = "";
            ShowButtonOCRollover = "";
            ShowButtonOCAccruals = "";
            ShowButtonCommercialReports = "";
            ShowButtonCompanyReports = "";
            ShowButtonPropMgmtVacate = "";
            ShowButtonPropMgmtBond = "";
            ShowButtonPropMgmtAgentNo = "";
            ShowButtonPropMgmtKeys = "";
            ShowButtonPropMgmtDirectDebit = "";
        }
        #endregion
        
        #region Validate
        public string Validate()
        {
            string Result = "";
            try
            {
                string BaseFormat = "{0} is {1}";
                string NotProvided = "Not Provided";
                string Invalid = "Invalid";
                string Key = "";
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientId";
                    if (!ClientIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ClientName";
                    if (string.IsNullOrEmpty(ClientName)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Login";
                    if (string.IsNullOrEmpty(Login)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IsABranch";
                    if (string.IsNullOrEmpty(IsABranch)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IsAManager";
                    if (string.IsNullOrEmpty(IsAManager)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IsAnAuditor";
                    if (string.IsNullOrEmpty(IsAnAuditor)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "MgmtGridPMView";
                    if (string.IsNullOrEmpty(MgmtGridPMView)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IsActive";
                    if (string.IsNullOrEmpty(IsActive)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowPortfolio";
                    if (string.IsNullOrEmpty(ShowPortfolio)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowToDoList";
                    if (string.IsNullOrEmpty(ShowToDoList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowDebtsList";
                    if (string.IsNullOrEmpty(ShowDebtsList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowInvoicesList";
                    if (string.IsNullOrEmpty(ShowInvoicesList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowReceiptsList";
                    if (string.IsNullOrEmpty(ShowReceiptsList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowPaymentsList";
                    if (string.IsNullOrEmpty(ShowPaymentsList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowLedgerList";
                    if (string.IsNullOrEmpty(ShowLedgerList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowReconciliationList";
                    if (string.IsNullOrEmpty(ShowReconciliationList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowSettingsList";
                    if (string.IsNullOrEmpty(ShowSettingsList)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsAdd";
                    if (string.IsNullOrEmpty(AllowDebtsAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsEdit";
                    if (string.IsNullOrEmpty(AllowDebtsEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsReverse";
                    if (string.IsNullOrEmpty(AllowDebtsReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsPeriodicalAdd";
                    if (string.IsNullOrEmpty(AllowDebtsPeriodicalAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsPeriodicalEdit";
                    if (string.IsNullOrEmpty(AllowDebtsPeriodicalEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtsPeriodicalInActive";
                    if (string.IsNullOrEmpty(AllowDebtsPeriodicalInActive)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtIncreasesAdd";
                    if (string.IsNullOrEmpty(AllowDebtIncreasesAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtIncreasesEdit";
                    if (string.IsNullOrEmpty(AllowDebtIncreasesEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowDebtIncreasesRemove";
                    if (string.IsNullOrEmpty(AllowDebtIncreasesRemove)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowInvoiceEdit";
                    if (string.IsNullOrEmpty(AllowInvoiceEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowInvoiceReverse";
                    if (string.IsNullOrEmpty(AllowInvoiceReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowPayeeAdd";
                    if (string.IsNullOrEmpty(AllowPayeeAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowPayeeEdit";
                    if (string.IsNullOrEmpty(AllowPayeeEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowLedgerAdd";
                    if (string.IsNullOrEmpty(AllowLedgerAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowLedgerEdit";
                    if (string.IsNullOrEmpty(AllowLedgerEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowLedgerReverse";
                    if (string.IsNullOrEmpty(AllowLedgerReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowBudgetAdd";
                    if (string.IsNullOrEmpty(AllowBudgetAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowBudgetEdit";
                    if (string.IsNullOrEmpty(AllowBudgetEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowBudgetReverse";
                    if (string.IsNullOrEmpty(AllowBudgetReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowInvoicePeriodicalAdd";
                    if (string.IsNullOrEmpty(AllowInvoicePeriodicalAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowInvoicePeriodicalEdit";
                    if (string.IsNullOrEmpty(AllowInvoicePeriodicalEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowInvoicePeriodicalInActive";
                    if (string.IsNullOrEmpty(AllowInvoicePeriodicalInActive)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowFeesAdd";
                    if (string.IsNullOrEmpty(AllowFeesAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowFeesEdit";
                    if (string.IsNullOrEmpty(AllowFeesEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowFeesReverse";
                    if (string.IsNullOrEmpty(AllowFeesReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowSubLedgerAdd";
                    if (string.IsNullOrEmpty(AllowSubLedgerAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowSubLedgerEdit";
                    if (string.IsNullOrEmpty(AllowSubLedgerEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowSubLedgerReverse";
                    if (string.IsNullOrEmpty(AllowSubLedgerReverse)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowChangeBranch";
                    if (string.IsNullOrEmpty(AllowChangeBranch)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowChangeManager";
                    if (string.IsNullOrEmpty(AllowChangeManager)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowStorageAdd";
                    if (string.IsNullOrEmpty(AllowStorageAdd)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowStorageRemove";
                    if (string.IsNullOrEmpty(AllowStorageRemove)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "AllowReceiptsEdit";
                    if (string.IsNullOrEmpty(AllowReceiptsEdit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonSalesDepositRelease";
                    if (string.IsNullOrEmpty(ShowButtonSalesDepositRelease)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonSalesReleaseFees";
                    if (string.IsNullOrEmpty(ShowButtonSalesReleaseFees)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonOCCertificate";
                    if (string.IsNullOrEmpty(ShowButtonOCCertificate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonOCReports";
                    if (string.IsNullOrEmpty(ShowButtonOCReports)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonOCMultiSetup";
                    if (string.IsNullOrEmpty(ShowButtonOCMultiSetup)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonOCRollover";
                    if (string.IsNullOrEmpty(ShowButtonOCRollover)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonOCAccruals";
                    if (string.IsNullOrEmpty(ShowButtonOCAccruals)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonCommercialReports";
                    if (string.IsNullOrEmpty(ShowButtonCommercialReports)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonCompanyReports";
                    if (string.IsNullOrEmpty(ShowButtonCompanyReports)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonPropMgmtVacate";
                    if (string.IsNullOrEmpty(ShowButtonPropMgmtVacate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonPropMgmtBond";
                    if (string.IsNullOrEmpty(ShowButtonPropMgmtBond)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonPropMgmtAgentNo";
                    if (string.IsNullOrEmpty(ShowButtonPropMgmtAgentNo)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonPropMgmtKeys";
                    if (string.IsNullOrEmpty(ShowButtonPropMgmtKeys)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ShowButtonPropMgmtDirectDebit";
                    if (string.IsNullOrEmpty(ShowButtonPropMgmtDirectDebit)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
            }
            catch (Exception Ex)
            {
                Result =
                    Ex.Message + Environment.NewLine +
                    Ex.InnerException;
                }
            
            return Result;
        }
        #endregion
        
        #region Update
        public string Update(AllPageDataRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.ClientIdValue = Source.ClientIdValue;
                this.ClientName = Source.ClientName;
                this.Login = Source.Login;
                this.IsABranch = Source.IsABranch;
                this.IsAManager = Source.IsAManager;
                this.IsAnAuditor = Source.IsAnAuditor;
                this.MgmtGridPMView = Source.MgmtGridPMView;
                this.IsActive = Source.IsActive;
                this.ShowPortfolio = Source.ShowPortfolio;
                this.ShowToDoList = Source.ShowToDoList;
                this.ShowDebtsList = Source.ShowDebtsList;
                this.ShowInvoicesList = Source.ShowInvoicesList;
                this.ShowReceiptsList = Source.ShowReceiptsList;
                this.ShowPaymentsList = Source.ShowPaymentsList;
                this.ShowLedgerList = Source.ShowLedgerList;
                this.ShowReconciliationList = Source.ShowReconciliationList;
                this.ShowSettingsList = Source.ShowSettingsList;
                this.AllowDebtsAdd = Source.AllowDebtsAdd;
                this.AllowDebtsEdit = Source.AllowDebtsEdit;
                this.AllowDebtsReverse = Source.AllowDebtsReverse;
                this.AllowDebtsPeriodicalAdd = Source.AllowDebtsPeriodicalAdd;
                this.AllowDebtsPeriodicalEdit = Source.AllowDebtsPeriodicalEdit;
                this.AllowDebtsPeriodicalInActive = Source.AllowDebtsPeriodicalInActive;
                this.AllowDebtIncreasesAdd = Source.AllowDebtIncreasesAdd;
                this.AllowDebtIncreasesEdit = Source.AllowDebtIncreasesEdit;
                this.AllowDebtIncreasesRemove = Source.AllowDebtIncreasesRemove;
                this.AllowInvoiceEdit = Source.AllowInvoiceEdit;
                this.AllowInvoiceReverse = Source.AllowInvoiceReverse;
                this.AllowPayeeAdd = Source.AllowPayeeAdd;
                this.AllowPayeeEdit = Source.AllowPayeeEdit;
                this.AllowLedgerAdd = Source.AllowLedgerAdd;
                this.AllowLedgerEdit = Source.AllowLedgerEdit;
                this.AllowLedgerReverse = Source.AllowLedgerReverse;
                this.AllowBudgetAdd = Source.AllowBudgetAdd;
                this.AllowBudgetEdit = Source.AllowBudgetEdit;
                this.AllowBudgetReverse = Source.AllowBudgetReverse;
                this.AllowInvoicePeriodicalAdd = Source.AllowInvoicePeriodicalAdd;
                this.AllowInvoicePeriodicalEdit = Source.AllowInvoicePeriodicalEdit;
                this.AllowInvoicePeriodicalInActive = Source.AllowInvoicePeriodicalInActive;
                this.AllowFeesAdd = Source.AllowFeesAdd;
                this.AllowFeesEdit = Source.AllowFeesEdit;
                this.AllowFeesReverse = Source.AllowFeesReverse;
                this.AllowSubLedgerAdd = Source.AllowSubLedgerAdd;
                this.AllowSubLedgerEdit = Source.AllowSubLedgerEdit;
                this.AllowSubLedgerReverse = Source.AllowSubLedgerReverse;
                this.AllowChangeBranch = Source.AllowChangeBranch;
                this.AllowChangeManager = Source.AllowChangeManager;
                this.AllowStorageAdd = Source.AllowStorageAdd;
                this.AllowStorageRemove = Source.AllowStorageRemove;
                this.AllowReceiptsEdit = Source.AllowReceiptsEdit;
                this.ShowButtonSalesDepositRelease = Source.ShowButtonSalesDepositRelease;
                this.ShowButtonSalesReleaseFees = Source.ShowButtonSalesReleaseFees;
                this.ShowButtonOCCertificate = Source.ShowButtonOCCertificate;
                this.ShowButtonOCReports = Source.ShowButtonOCReports;
                this.ShowButtonOCMultiSetup = Source.ShowButtonOCMultiSetup;
                this.ShowButtonOCRollover = Source.ShowButtonOCRollover;
                this.ShowButtonOCAccruals = Source.ShowButtonOCAccruals;
                this.ShowButtonCommercialReports = Source.ShowButtonCommercialReports;
                this.ShowButtonCompanyReports = Source.ShowButtonCompanyReports;
                this.ShowButtonPropMgmtVacate = Source.ShowButtonPropMgmtVacate;
                this.ShowButtonPropMgmtBond = Source.ShowButtonPropMgmtBond;
                this.ShowButtonPropMgmtAgentNo = Source.ShowButtonPropMgmtAgentNo;
                this.ShowButtonPropMgmtKeys = Source.ShowButtonPropMgmtKeys;
                this.ShowButtonPropMgmtDirectDebit = Source.ShowButtonPropMgmtDirectDebit;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        //public string Load(ActionBase ActionBase, JObject DataObject)
        public string Load(ActionBase ActionBase)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                //string Method = ClassName + ".LoadFromDataObject";
                string Method = ClassName + ".LoadFromActionBase";
            
                ActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    AllPageDataRequestItem TempItem = new AllPageDataRequestItem();
                    //Result = CommonMethods.LoadAllPageDataRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageDataRequestItem(ActionBase, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = ActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = ActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    ActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
