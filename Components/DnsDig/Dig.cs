using System;
using System.Diagnostics;
using System.Text;

using System.Threading;
using System.Globalization;

using Heijden.DNS;

namespace DnsDig
{
	class Dig
	{
		public Resolver resolver;

		public Dig()
		{
			resolver = new Resolver();
		}

		public string DigIt(string name, QType qtype, QClass qclass)
		{
			//this StringBuilder is for Debug purposes only
			StringBuilder sb = new StringBuilder();
			
			//sb.AppendFormat("; <<>> Dig.Net {0} <<>> @{1} {2} {3}<br/>", resolver.Version, resolver.DnsServer, qtype, name);
			//sb.Append(";; global options: printcmd<br/>");

			Stopwatch sw = new Stopwatch();

			sw.Start();
			Response response = resolver.Query(name, qtype, qclass);
			sw.Stop();

			if(response.Error != "")
			{
				sb.AppendFormat("{0}<br/>", response.Error);
				return sb.ToString();
			}

			return (response.header.ANCOUNT > 0) ? "" : "MailServerNotFound";
			/*
			sb.Append(";; Got answer:<br/>");

			sb.AppendFormat(";; ->>HEADER<<- opcode: {0}, status: {1}, id: {2}<br/>",
				response.header.OPCODE,
				response.header.RCODE,
				response.header.ID);
			sb.AppendFormat(";; flags: {0}{1}{2}{3}; QUERY: {4}, ANSWER: {5}, AUTHORITY: {6}, ADDITIONAL: {7}<br/>",
				response.header.QR ? " qr" : "",
				response.header.AA ? " aa" : "",
				response.header.RD ? " rd" : "",
				response.header.RA ? " ra" : "",
				response.header.QDCOUNT,
				response.header.ANCOUNT,
				response.header.NSCOUNT,
				response.header.ARCOUNT);
			
			if (response.header.QDCOUNT > 0)
			{
				sb.Append(";; QUESTION SECTION:<br/>");
				foreach (Question question in response.Questions)
					sb.AppendFormat(";{0}<br/>", question);
			}

			if (response.header.ANCOUNT > 0)
			{
				sb.Append(";; ANSWER SECTION:<br/>");
				foreach (AnswerRR answerRR in response.Answers)
					sb.AppendFormat("{0}<br/>", answerRR);
			}

			if (response.header.NSCOUNT > 0)
			{
				sb.Append(";; AUTHORITY SECTION:<br/>");
				foreach (AuthorityRR authorityRR in response.Authorities)
					sb.AppendFormat("{0}<br/>", authorityRR);
			}

			if (response.header.ARCOUNT > 0)
			{
				sb.Append(";; ADDITIONAL SECTION:<br/>");
				foreach (AdditionalRR additionalRR in response.Additionals)
					sb.AppendFormat("{0}<br/>", additionalRR);
			}

			sb.AppendFormat(";; Query time: {0} msec<br/>", sw.ElapsedMilliseconds);
			sb.AppendFormat(";; SERVER: {0}#{1}({2})<br/>", response.Server.Address, response.Server.Port, response.Server.Address);
			sb.AppendFormat(";; WHEN: {0}<br/>", response.TimeStamp.ToString("ddd MMM dd HH:mm:ss yyyy",new System.Globalization.CultureInfo("en-US")));
			sb.AppendFormat(";; MSG SIZE rcvd: {0}<br/>", response.MessageSize);

			return sb.ToString();
			 */
		}
	}
}
