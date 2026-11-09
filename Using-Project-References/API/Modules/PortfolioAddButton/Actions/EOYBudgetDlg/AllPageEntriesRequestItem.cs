/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 08-07-2020
 * Time: 1:45 PM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\PortfolioAddButton\Actions\Templates\EOYBudgetDlg\AllPageEntriesRequestItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.PortfolioAddButton.EOYBudgetDlg
{
    public class AllPageEntriesRequestItem
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
        
        private string FChartCodeIdValue = "";
        public bool ChartCodeIdIsValid = false;
        public int ChartCodeId { get; set; }
        public string ChartCodeIdValue
        {
            get { return FChartCodeIdValue; }
            set
            {
                FChartCodeIdValue = value;
                int Dummy = 0;
                ChartCodeIdIsValid = int.TryParse(FChartCodeIdValue, out Dummy);
                ChartCodeId = (ChartCodeIdIsValid) ? Dummy : 0;
            }
        }
        
        private string FYearValue = "";
        public bool YearIsValid = false;
        public int Year { get; set; }
        public string YearValue
        {
            get { return FYearValue; }
            set
            {
                FYearValue = value;
                int Dummy = 0;
                YearIsValid = int.TryParse(FYearValue, out Dummy);
                Year = (YearIsValid) ? Dummy : 0;
            }
        }
        
        private string FTotalValue = "";
        public bool TotalIsValid = false;
        public double Total { get; set; }
        public string TotalValue
        {
            get { return FTotalValue; }
            set
            {
                FTotalValue = value;
                double Dummy = 0.0;
                TotalIsValid = Double.TryParse(FTotalValue, out Dummy);
                Total = (TotalIsValid) ? Dummy : 0;
            }
        }
        
        private string FJulyValue = "";
        public bool JulyIsValid = false;
        public double July { get; set; }
        public string JulyValue
        {
            get { return FJulyValue; }
            set
            {
                FJulyValue = value;
                double Dummy = 0.0;
                JulyIsValid = Double.TryParse(FJulyValue, out Dummy);
                July = (JulyIsValid) ? Dummy : 0;
            }
        }
        
        private string FAugustValue = "";
        public bool AugustIsValid = false;
        public double August { get; set; }
        public string AugustValue
        {
            get { return FAugustValue; }
            set
            {
                FAugustValue = value;
                double Dummy = 0.0;
                AugustIsValid = Double.TryParse(FAugustValue, out Dummy);
                August = (AugustIsValid) ? Dummy : 0;
            }
        }
        
        private string FSeptemberValue = "";
        public bool SeptemberIsValid = false;
        public double September { get; set; }
        public string SeptemberValue
        {
            get { return FSeptemberValue; }
            set
            {
                FSeptemberValue = value;
                double Dummy = 0.0;
                SeptemberIsValid = Double.TryParse(FSeptemberValue, out Dummy);
                September = (SeptemberIsValid) ? Dummy : 0;
            }
        }
        
        private string FOctoberValue = "";
        public bool OctoberIsValid = false;
        public double October { get; set; }
        public string OctoberValue
        {
            get { return FOctoberValue; }
            set
            {
                FOctoberValue = value;
                double Dummy = 0.0;
                OctoberIsValid = Double.TryParse(FOctoberValue, out Dummy);
                October = (OctoberIsValid) ? Dummy : 0;
            }
        }
        
        private string FNovemberValue = "";
        public bool NovemberIsValid = false;
        public double November { get; set; }
        public string NovemberValue
        {
            get { return FNovemberValue; }
            set
            {
                FNovemberValue = value;
                double Dummy = 0.0;
                NovemberIsValid = Double.TryParse(FNovemberValue, out Dummy);
                November = (NovemberIsValid) ? Dummy : 0;
            }
        }
        
        private string FDecemberValue = "";
        public bool DecemberIsValid = false;
        public double December { get; set; }
        public string DecemberValue
        {
            get { return FDecemberValue; }
            set
            {
                FDecemberValue = value;
                double Dummy = 0.0;
                DecemberIsValid = Double.TryParse(FDecemberValue, out Dummy);
                December = (DecemberIsValid) ? Dummy : 0;
            }
        }
        
        private string FJanuaryValue = "";
        public bool JanuaryIsValid = false;
        public double January { get; set; }
        public string JanuaryValue
        {
            get { return FJanuaryValue; }
            set
            {
                FJanuaryValue = value;
                double Dummy = 0.0;
                JanuaryIsValid = Double.TryParse(FJanuaryValue, out Dummy);
                January = (JanuaryIsValid) ? Dummy : 0;
            }
        }
        
        private string FFebruaryValue = "";
        public bool FebruaryIsValid = false;
        public double February { get; set; }
        public string FebruaryValue
        {
            get { return FFebruaryValue; }
            set
            {
                FFebruaryValue = value;
                double Dummy = 0.0;
                FebruaryIsValid = Double.TryParse(FFebruaryValue, out Dummy);
                February = (FebruaryIsValid) ? Dummy : 0;
            }
        }
        
        private string FMarchValue = "";
        public bool MarchIsValid = false;
        public double March { get; set; }
        public string MarchValue
        {
            get { return FMarchValue; }
            set
            {
                FMarchValue = value;
                double Dummy = 0.0;
                MarchIsValid = Double.TryParse(FMarchValue, out Dummy);
                March = (MarchIsValid) ? Dummy : 0;
            }
        }
        
        private string FAprilValue = "";
        public bool AprilIsValid = false;
        public double April { get; set; }
        public string AprilValue
        {
            get { return FAprilValue; }
            set
            {
                FAprilValue = value;
                double Dummy = 0.0;
                AprilIsValid = Double.TryParse(FAprilValue, out Dummy);
                April = (AprilIsValid) ? Dummy : 0;
            }
        }
        
        private string FMayValue = "";
        public bool MayIsValid = false;
        public double May { get; set; }
        public string MayValue
        {
            get { return FMayValue; }
            set
            {
                FMayValue = value;
                double Dummy = 0.0;
                MayIsValid = Double.TryParse(FMayValue, out Dummy);
                May = (MayIsValid) ? Dummy : 0;
            }
        }
        
        private string FJuneValue = "";
        public bool JuneIsValid = false;
        public double June { get; set; }
        public string JuneValue
        {
            get { return FJuneValue; }
            set
            {
                FJuneValue = value;
                double Dummy = 0.0;
                JuneIsValid = Double.TryParse(FJuneValue, out Dummy);
                June = (JuneIsValid) ? Dummy : 0;
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
            ChartCodeIdValue = "";
            YearValue = "";
            TotalValue = "";
            JulyValue = "";
            AugustValue = "";
            SeptemberValue = "";
            OctoberValue = "";
            NovemberValue = "";
            DecemberValue = "";
            JanuaryValue = "";
            FebruaryValue = "";
            MarchValue = "";
            AprilValue = "";
            MayValue = "";
            JuneValue = "";
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
                    Key = "ChartCodeId";
                    if (!ChartCodeIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Year";
                    if (!YearIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "Total";
                    if (!TotalIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "July";
                    if (!JulyIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "August";
                    if (!AugustIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "September";
                    if (!SeptemberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "October";
                    if (!OctoberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "November";
                    if (!NovemberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "December";
                    if (!DecemberIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "January";
                    if (!JanuaryIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "February";
                    if (!FebruaryIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "March";
                    if (!MarchIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "April";
                    if (!AprilIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "May";
                    if (!MayIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "June";
                    if (!JuneIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(AllPageEntriesRequestItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.Option1IdValue = Source.Option1IdValue;
                this.Option2IdValue = Source.Option2IdValue;
                this.GridIdValue = Source.GridIdValue;
                this.ChartCodeIdValue = Source.ChartCodeIdValue;
                this.YearValue = Source.YearValue;
                this.TotalValue = Source.TotalValue;
                this.JulyValue = Source.JulyValue;
                this.AugustValue = Source.AugustValue;
                this.SeptemberValue = Source.SeptemberValue;
                this.OctoberValue = Source.OctoberValue;
                this.NovemberValue = Source.NovemberValue;
                this.DecemberValue = Source.DecemberValue;
                this.JanuaryValue = Source.JanuaryValue;
                this.FebruaryValue = Source.FebruaryValue;
                this.MarchValue = Source.MarchValue;
                this.AprilValue = Source.AprilValue;
                this.MayValue = Source.MayValue;
                this.JuneValue = Source.JuneValue;
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
            
                    AllPageEntriesRequestItem TempItem = new AllPageEntriesRequestItem();
                    //Result = CommonMethods.LoadAllPageEntriesRequestItem(DataObject, ref TempItem);
                    Result = CommonMethods.LoadAllPageEntriesRequestItem(ActionBase, ref TempItem);
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
