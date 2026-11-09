/*
 * Created by Make Item Classes.
 * Author: John Bennett
 * Date: 25-09-2019
 * Time: 11:22 AM

 * SourceFile: D:\Source\EAOffice\EAOfficeAPI\Modules\ComboBoxEntries\Actions\Templates\CommonFilteredLookupsItem.json
*/
using System;
using System.Data;
using System.Collections.Generic;
using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Linq;

namespace Actions.ComboBoxEntries
{
    public class CommonFilteredLookupsItem
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
        
        private string FCategoryOnlyIdValue = "";
        public bool CategoryOnlyIdIsValid = false;
        public int CategoryOnlyId { get; set; }
        public string CategoryOnlyIdValue
        {
            get { return FCategoryOnlyIdValue; }
            set
            {
                FCategoryOnlyIdValue = value;
                int Dummy = 0;
                CategoryOnlyIdIsValid = int.TryParse(FCategoryOnlyIdValue, out Dummy);
                CategoryOnlyId = (CategoryOnlyIdIsValid) ? Dummy : 0;
            }
        }
        
        public string DlgType { get; set; }
        
        #endregion
        
        #region Clear
        public void Clear()
        {
            PortfolioIdValue = "";
            CategoryOnlyIdValue = "";
            DlgType = "";
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
                    Key = "CategoryOnlyId";
                    if (!CategoryOnlyIdIsValid) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
                }
                
                if (string.IsNullOrEmpty(Result))
                {
                    Key = "DlgType";
                    if (string.IsNullOrEmpty(DlgType)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
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
        public string Update(CommonFilteredLookupsItem Source)
        {
            string Result = "";
            
            try
            {
                this.PortfolioIdValue = Source.PortfolioIdValue;
                this.CategoryOnlyIdValue = Source.CategoryOnlyIdValue;
                this.DlgType = Source.DlgType;
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }
            
            return Result;
        }
        #endregion
        
        #region Load from DataObject
        public string Load(ActionBase AActionBase, JObject DataObject)
            {
                string Result = "";
                string ClassName = this.GetType().ToString();
                string Method = ClassName + ".LoadFromDataObject";
            
                AActionBase.LogOnInsideMethod(Method);
            
                try
                {
                    Clear();
            
                    CommonFilteredLookupsItem TempItem = new CommonFilteredLookupsItem();
                    Result = CommonMethods.LoadCommonFilteredLookupsItem(DataObject, ref TempItem);
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = AActionBase.LogOnProcessingError(Method, Result, "");
                    }
                    else
                    {
                        Result = Update(TempItem);
                    }
                }
                catch (Exception Ex)
                {
                    Result = AActionBase.LogOnException(Method, Ex, "");
                    return Result;
                }
                finally
                {
                    AActionBase.LogOnLeavingMethod(Method);
                }
            
                return Result;
        }
        #endregion
        
    }
}
