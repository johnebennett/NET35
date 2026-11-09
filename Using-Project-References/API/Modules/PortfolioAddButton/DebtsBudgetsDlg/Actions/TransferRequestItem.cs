/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 01-08-2020
 * Time: 8:20 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\DebtsBudgetsDlg\Actions\Templates\TransferRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.DebtsBudgetsDlg
{
    public class TransferRequestItem
    {
        #region Properties - Getters and Setters
        private string FPortfolioIdValue = "";
        public bool PortfolioIdIsValid = false;
        public int PortfolioId { get; set; }
        public string PortfolioIdValue
        {
            get { return FPortfolioIdValue; }
            set
            {
                FPortfolioIdValue = value;
                int Dummy = 0;
                PortfolioIdIsValid = int.TryParse(FPortfolioIdValue, out Dummy);
                PortfolioId = (PortfolioIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOption1IdValue = "";
        public bool Option1IdIsValid = false;
        public int Option1Id { get; set; }
        public string Option1IdValue
        {
            get { return FOption1IdValue; }
            set
            {
                FOption1IdValue = value;
                int Dummy = 0;
                Option1IdIsValid = int.TryParse(FOption1IdValue, out Dummy);
                Option1Id = (Option1IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOption2IdValue = "";
        public bool Option2IdIsValid = false;
        public int Option2Id { get; set; }
        public string Option2IdValue
        {
            get { return FOption2IdValue; }
            set
            {
                FOption2IdValue = value;
                int Dummy = 0;
                Option2IdIsValid = int.TryParse(FOption2IdValue, out Dummy);
                Option2Id = (Option2IdIsValid) ? Dummy : 0;
            }
        }
        
        private string FGridIdValue = "";
        public bool GridIdIsValid = false;
        public int GridId { get; set; }
        public string GridIdValue
        {
            get { return FGridIdValue; }
            set
            {
                FGridIdValue = value;
                int Dummy = 0;
                GridIdIsValid = int.TryParse(FGridIdValue, out Dummy);
                GridId = (GridIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FBudgetIdValue = "";
        public bool BudgetIdIsValid = false;
        public int BudgetId { get; set; }
        public string BudgetIdValue
        {
            get { return FBudgetIdValue; }
            set
            {
                FBudgetIdValue = value;
                int Dummy = 0;
                BudgetIdIsValid = int.TryParse(FBudgetIdValue, out Dummy);
                BudgetId = (BudgetIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FOptionIdValue = "";
        public bool OptionIdIsValid = false;
        public int OptionId { get; set; }
        public string OptionIdValue
        {
            get { return FOptionIdValue; }
            set
            {
                FOptionIdValue = value;
                int Dummy = 0;
                OptionIdIsValid = int.TryParse(FOptionIdValue, out Dummy);
                OptionId = (OptionIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FExpenseIdValue = "";
        public bool ExpenseIdIsValid = false;
        public int ExpenseId { get; set; }
        public string ExpenseIdValue
        {
            get { return FExpenseIdValue; }
            set
            {
                FExpenseIdValue = value;
                int Dummy = 0;
                ExpenseIdIsValid = int.TryParse(FExpenseIdValue, out Dummy);
                ExpenseId = (ExpenseIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransferToIdValue = "";
        public bool TransferToIdIsValid = false;
        public int TransferToId { get; set; }
        public string TransferToIdValue
        {
            get { return FTransferToIdValue; }
            set
            {
                FTransferToIdValue = value;
                int Dummy = 0;
                TransferToIdIsValid = int.TryParse(FTransferToIdValue, out Dummy);
                TransferToId = (TransferToIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FTransferPercentageValue = "";
        public bool TransferPercentageIsValid = false;
        public double TransferPercentage { get; set; }
        public string TransferPercentageValue
        {
            get { return FTransferPercentageValue; }
            set
            {
                FTransferPercentageValue = value;
                double Dummy = 0.0;
                TransferPercentageIsValid = Double.TryParse(FTransferPercentageValue, out Dummy);
                TransferPercentage = (TransferPercentageIsValid) ? Dummy : 0;
            }
        }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            Option1IdValue = "";
            Option2IdValue = "";
            GridIdValue = "";
            BudgetIdValue = "";
            OptionIdValue = "";
            ExpenseIdValue = "";
            TransferToIdValue = "";
            TransferPercentageValue = "";
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
                    Key = "PortfolioId";
                    if (!PortfolioIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option1Id";
                    if (!Option1IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Option2Id";
                    if (!Option2IdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "GridId";
                    if (!GridIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "BudgetId";
                    if (!BudgetIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "OptionId";
                    if (!OptionIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ExpenseId";
                    if (!ExpenseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransferToId";
                    if (!TransferToIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "TransferPercentage";
                    if (!TransferPercentageIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(TransferRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.BudgetIdValue = Source.BudgetIdValue;
                this.OptionIdValue = Source.OptionIdValue;
                this.ExpenseIdValue = Source.ExpenseIdValue;
                this.TransferToIdValue = Source.TransferToIdValue;
                this.TransferPercentageValue = Source.TransferPercentageValue;
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
            
                    TransferRequestItem TempItem = new TransferRequestItem();
                    //Result = CommonMethods.LoadTransferRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadTransferRequestItem(ActionBase, ref TempItem);
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
