/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/08/2019
 * Time: 4:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using CommonUtilities;

namespace UsefulDBMethods.Reporting
{
	/// <summary>
	/// Description of GeneralReportParameters.
	/// </summary>
	public class GeneralReportParameters
	{
		public string Parameter { get; set; }
		public string Source { get; set; }
		public string Field { get; set; }

		public GeneralReportParameters()
		{
		}
	}
	
	public class EAReportGetDefaultItem
	{
		public string ReportName { get; set; }
		public int ReportId { get; set; }
		public string Default { get; set; }
		
		public string AsJsonObject()
		{
			string Result =
				JsonUtilities.StartJsonObject("") +
				JsonUtilities.AddPropertyNameAndValue("Name", ReportName) +
				JsonUtilities.AddPropertyNameAndValue("Value", ReportId, false);			
			if (Default == "1")
			{
				Result = Result + "," + JsonUtilities.AddPropertyNameAndValue("Default", "1", false);
			}

			Result = Result + JsonUtilities.EndJsonObject();
			
			return Result;
		}
	}	
}
