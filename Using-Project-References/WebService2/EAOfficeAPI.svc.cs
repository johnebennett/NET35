/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 07/04/2017
 * Time: 3:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ServiceModel;

namespace EAOfficeAPI
{
	[ServiceContract]
	public interface IEAOfficeAPI
	{
	   [OperationContract]
	   string Execute(string Request);
	   
	   [OperationContract]
	   string HelloWorld();
	}
}
