/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/08/2019
 * Time: 4:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Actions.PaymentOperations
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
}
