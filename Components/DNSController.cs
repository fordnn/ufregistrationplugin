using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public class DNSController
	{
		public static string IsMailServerExists(string Email, string ResourceFile)
		{
			string Domain = Email.Substring(Email.IndexOf("@") + 1);
			DnsDig.Dig objDig = new DnsDig.Dig();
			string Result = objDig.DigIt(Domain, Heijden.DNS.QType.MX, Heijden.DNS.QClass.ANY);

			if (Result != "")
			{
				//Lets check if we have localization for this error
				object objMessage = Localization.GetString(Result, ResourceFile);
				if (objMessage != null)
				{
					Result = (string) objMessage;
				}
			}

			return Result;
		}
	}
}
