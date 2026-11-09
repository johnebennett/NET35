using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OtpNet;
using Net.Codecrete.QrCodeGenerator;
//using System. Drawing;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.SettingsOperations.TwoFADlg
{
    public class CommonMethods
    {
        public CommonMethods()
        {
        }

        public static string GetModuleName() { return "SettingsOperations.TwoFADlg."; }

        public static string LoadResetQRCodeRequestItem(ActionBase ActionBase,
                                                        ref ResetQRCodeRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadResetQRCodeRequestItem";
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

        public static string ResetQRCode(string ConnectionString,
                                         int CompanyId,
                                         ResetQRCodeRequestItem RequestItem,
                                         ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "ResetQRCode";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string UniqueKey = Base32Encoding.GenerateRandomCode(12);
                if (string.IsNullOrEmpty(UniqueKey))
                {
                    Result = ActionBase.LogOnProcessingError(Method, "Error generating new 2FA Code", "");
                }

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " UPDATE EALogin SET ALOGIN_USE2FAKEY = @UniqueKey" +
                        " WHERE I_ALOGIN_COMPANYID = @CompanyId AND I_ALOGIN_CLIENTID = @ClientId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("UniqueKey", UniqueKey));
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("ClientId", RequestItem.ClientId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After ResetQRCode Result: " + Result, Method, 0);
            }

            return Result;
        }

        public static string LoadGenerateQRCodeRequestItem(ActionBase ActionBase,
                                                           ref GenerateQRCodeRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadGenerateQRCodeRequestItem";
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

        public static string GenerateQRCode(string ConnectionString,
                                            int CompanyId,
                                            ResetQRCodeRequestItem RequestItem,
                                            ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "GenerateQRCode";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string Secret = GetOur2FASecret(ConnectionString);
                string UserUniqueKey = GetUserUniqueKey(ConnectionString, CompanyId, RequestItem.ClientId);
                string OtpEntry =
                    @"otpauth://totp/GOATPM?secret=" + Secret + UserUniqueKey;
                WindowsEventLoggingUtilities.WriteToInfoEvent("OtpEntry: " + OtpEntry, Method, RequestItem.ClientId);
                QrCode qr = QrCode.EncodeText(OtpEntry, QrCode.Ecc.High);
                string FileNameOnly = "2FAQRCode_" + CompanyId.ToString() + "_" + RequestItem.ClientId.ToString() + ".png";
                string FileName = "";
                string FilePath = CommonUtilities.FolderHelpers.DocsLocation(ConnectionString, CompanyId, out Result);
                string PathSep = Path.DirectorySeparatorChar.ToString();

                if (string.IsNullOrEmpty(Result))
                {
                    if (!FilePath.EndsWith(PathSep)) FilePath += PathSep;
                    FileName = FilePath + FileNameOnly;
                    using (var bitmap = qr.ToBitmap(4, 10))
                    {
                        bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    if (!File.Exists(FileName))
                    {
                        Result = "The File: " + FileName + " does not exist";
                        ActionBase.LogOnProcessingError(Method, Result, "");
                        Result = "Cannot find the QR Code File";
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ImageSrc", "/docs/" + FileNameOnly, false));
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GenerateQRCode Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static string GetOur2FASecret(string ConnectionString)
        {
            string Result = "";
            string Method = GetModuleName() + "GetOur2FASecret";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "SELECT EAOffice_2FASecret FROM EAOfficeSettings";
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["EAOffice_2FASecret"];
                            Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                        }
                    }
                }

                if (string.IsNullOrEmpty(Result)) Result = "GOATPM";
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetOur2FASecret Result: " + Result, Method, 0);
            }

            if (string.IsNullOrEmpty(Result)) Result = "GOATPM";
            return Result;
        }

        private static string GetUserUniqueKey(string ConnectionString,
                                               Int64 CompanyId,
                                               Int64 ClientId)
        {
            string Result = "";
            string Method = GetModuleName() + "GetUserUniqueKey";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = 
                    " SELECT ALogin_Use2FAKey FROM EALogin" +
                    " WHERE I_ALogin_CompanyId = @CompanyId AND I_ALogin_ClientId = @ClientId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("ClientId", ClientId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["ALogin_Use2FAKey"];
                            Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetUserUniqueKey Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
