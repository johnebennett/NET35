/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 22-06-2020
 * Time: 7:57 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\Actions\Templates\DebtIncreasesDlg\SaveCmdRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.DebtIncreasesDlg
{
    public class SaveCmdRequestItem
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
        
        private string FIncreaseIdValue = "";
        public bool IncreaseIdIsValid = false;
        public int IncreaseId { get; set; }
        public string IncreaseIdValue
        {
            get { return FIncreaseIdValue; }
            set
            {
                FIncreaseIdValue = value;
                int Dummy = 0;
                IncreaseIdIsValid = int.TryParse(FIncreaseIdValue, out Dummy);
                IncreaseId = (IncreaseIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FPastOrFutureValue = "";
        public bool PastOrFutureIsValid = false;
        public int PastOrFuture { get; set; }
        public string PastOrFutureValue
        {
            get { return FPastOrFutureValue; }
            set
            {
                FPastOrFutureValue = value;
                int Dummy = 0;
                PastOrFutureIsValid = int.TryParse(FPastOrFutureValue, out Dummy);
                PastOrFuture = (PastOrFutureIsValid) ? Dummy : 0;
            }
        }
        
        public string IncreaseDate { get; set; }
        
        private string FFixedAmountValue = "";
        public bool FixedAmountIsValid = false;
        public double FixedAmount { get; set; }
        public string FixedAmountValue
        {
            get { return FFixedAmountValue; }
            set
            {
                FFixedAmountValue = value;
                double Dummy = 0.0;
                FixedAmountIsValid = Double.TryParse(FFixedAmountValue, out Dummy);
                FixedAmount = (FixedAmountIsValid) ? Dummy : 0;
            }
        }
        
        private string FPercentIncreaseValue = "";
        public bool PercentIncreaseIsValid = false;
        public double PercentIncrease { get; set; }
        public string PercentIncreaseValue
        {
            get { return FPercentIncreaseValue; }
            set
            {
                FPercentIncreaseValue = value;
                double Dummy = 0.0;
                PercentIncreaseIsValid = Double.TryParse(FPercentIncreaseValue, out Dummy);
                PercentIncrease = (PercentIncreaseIsValid) ? Dummy : 0;
            }
        }
        
        public string Detail { get; set; }
        
        public string CPIIncrease { get; set; }
        
        private string FCPIPlusExtraInterestValue = "";
        public bool CPIPlusExtraInterestIsValid = false;
        public double CPIPlusExtraInterest { get; set; }
        public string CPIPlusExtraInterestValue
        {
            get { return FCPIPlusExtraInterestValue; }
            set
            {
                FCPIPlusExtraInterestValue = value;
                double Dummy = 0.0;
                CPIPlusExtraInterestIsValid = Double.TryParse(FCPIPlusExtraInterestValue, out Dummy);
                CPIPlusExtraInterest = (CPIPlusExtraInterestIsValid) ? Dummy : 0;
            }
        }
        
        private string FOperandValue = "";
        public bool OperandIsValid = false;
        public int Operand { get; set; }
        public string OperandValue
        {
            get { return FOperandValue; }
            set
            {
                FOperandValue = value;
                int Dummy = 0;
                OperandIsValid = int.TryParse(FOperandValue, out Dummy);
                Operand = (OperandIsValid) ? Dummy : 0;
            }
        }
        
        private string FComparePercentageValue = "";
        public bool ComparePercentageIsValid = false;
        public double ComparePercentage { get; set; }
        public string ComparePercentageValue
        {
            get { return FComparePercentageValue; }
            set
            {
                FComparePercentageValue = value;
                double Dummy = 0.0;
                ComparePercentageIsValid = Double.TryParse(FComparePercentageValue, out Dummy);
                ComparePercentage = (ComparePercentageIsValid) ? Dummy : 0;
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
            IncreaseIdValue = "";
            PastOrFutureValue = "";
            IncreaseDate = "";
            FixedAmountValue = "";
            PercentIncreaseValue = "";
            CPIIncrease = "";
            CPIPlusExtraInterestValue = "";
            OperandValue = "";
            ComparePercentageValue = "";
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
                    Key = "IncreaseId";
                    if (!IncreaseIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PastOrFuture";
                    if (!PastOrFutureIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "IncreaseDate";
                    if (string.IsNullOrEmpty(IncreaseDate)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "FixedAmount";
                    if (!FixedAmountIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "PercentIncrease";
                    if (!PercentIncreaseIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CPIIncrease";
                    if (string.IsNullOrEmpty(CPIIncrease)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "CPIPlusExtraInterest";
                    if (!CPIPlusExtraInterestIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Operand";
                    if (!OperandIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "ComparePercentage";
                    if (!ComparePercentageIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(SaveCmdRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.IncreaseIdValue = Source.IncreaseIdValue;
                this.PastOrFutureValue = Source.PastOrFutureValue;
                this.IncreaseDate = Source.IncreaseDate;
                this.FixedAmountValue = Source.FixedAmountValue;
                this.PercentIncreaseValue = Source.PercentIncreaseValue;
                this.Detail = Source.Detail;
                this.CPIIncrease = Source.CPIIncrease;
                this.CPIPlusExtraInterestValue = Source.CPIPlusExtraInterestValue;
                this.OperandValue = Source.OperandValue;
                this.ComparePercentageValue = Source.ComparePercentageValue;
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
            
                    SaveCmdRequestItem TempItem = new SaveCmdRequestItem();
                    //Result = CommonMethods.LoadSaveCmdRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadSaveCmdRequestItem(ActionBase, ref TempItem);
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
