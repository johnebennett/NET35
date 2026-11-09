/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/08/2019
 * Time: 08:30 PM
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

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of StatementCancelPreview.
	/// </summary>
	public class StatementCancelPreview : ActionBase
	{
			private string FModuleName = "PaymentOperations";
			private StatementCancelPreviewRequestItem FRequestItem;
			
			public static string Execute(int MasterId,
										 RequestCommonItem CommonItem,
										 JObject DataObject,
										 APISettingsItem SettingsItem,
										 out string ErrorMsg)
			{
				string Method = "Execute";
				string Result = "";
				ErrorMsg = "";

				StatementCancelPreview inst = new StatementCancelPreview();
				if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
				LogOnInsideMethod(Method);
				try 
				{
					FRequestItem = new StatementCancelPreviewRequestItem();
					Result = FRequestItem.Load((ActionBase)this, DataObject);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
				LogOnInsideMethod(Method);
				try 
				{
					Result = BuildOutput(out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						Result = LogOnProcessingError(Method, ErrorMsg, "");
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
				return Result;
			}
			
			private string BuildOutput(out string ErrorMsg)
			{
				ErrorMsg = "";
				string Result = "";
				string Method = "BuildOutput";
				
				LogOnInsideMethod(Method);
				try
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
					ErrorMsg = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
					if (!string.IsNullOrEmpty(ErrorMsg)) return "";
					
					string QueryText = "EXECUTE PROCEDURE EAStatementCancelPreview(@companyclientid, @runid)";
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
					Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
					}
					
					if (string.IsNullOrEmpty(ErrorMsg)) Result = LogResponse(Method, "");
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
		}
	}
