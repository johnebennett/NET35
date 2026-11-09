/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 10:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ServiceModel;

namespace EAOfficeAPI
{
	/// <summary>
	/// Description of IEAOfficeAPI.
	/// </summary>
	[ServiceContract]
	public interface IEAOfficeAPI
	{
		[OperationContract]
		string Execute(string Request);
		
		[OperationContract]
		string HelloWorld();
	}
}
