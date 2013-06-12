using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public class CommonController
	{
		public static string DefaultSuggestionTemplates = @"[FirstName][LastName]
[LastName][FirstName]
[FirstName]_[LastName]
[LastName]_[FirstName]
[FirstName:0:1][LastName]
[LastName:0:1][FirstName]
[FirstName:0:1][LastName:0:1][Random:0:3]
[FirstName:0:1][LastName:0:1][DateTime:yyyy]
[FirstName:0:1][LastName:0:1][DateTime:ddMMyyyy]
[UserName][Random:0:3]
[UserName][DateTime:yyyy]
[UserName][DateTime:ddMMyyyy]
";
		#region Cache

		public static bool UseCache = false;

		public static void UpdateCache(string KeyFormat, object KeyID, object objCache, string CommonKeys)
		{
			if (!UseCache)
			{
				return;
			}
			string CacheKey = string.Format(KeyFormat, KeyID);
			if (objCache == null)
			{
				DotNetNuke.Common.Utilities.DataCache.RemoveCache(CacheKey);
			}
			else
			{
				DotNetNuke.Common.Utilities.DataCache.SetCache(CacheKey, objCache);
			}
			foreach (string CommonKey in CommonKeys.Split(new char[] { ',' }))
			{
				DotNetNuke.Common.Utilities.DataCache.RemoveCache(string.Format(KeyFormat, CommonKey));
			}
		}

		#endregion

		#region Verification Methods

		public static bool UserNameExists(int PortalID, string UserName)
		{
			return DataProvider.Instance().UserNameExists(PortalID, UserName);
		}

		public static bool EmailExists(int PortalID, string Email)
		{
			return DataProvider.Instance().EmailExists(PortalID, Email);
		}

		#endregion

		#region UserName Suggestion

		private static string ParseToken(string Source, string Token, string ReplaceWithValue)
		{
			try
			{
				Regex objRegEx = new Regex(
					  "\\[" + Token + ".{0,}?\\]",
					RegexOptions.IgnoreCase
					| RegexOptions.CultureInvariant
					| RegexOptions.IgnorePatternWhitespace
					| RegexOptions.Compiled
					);
				MatchCollection lstMatches = objRegEx.Matches(Source);
				if (lstMatches.Count == 0)
				{
					return Source;
				}
				string Result = Source;
				foreach (Match objMatch in lstMatches)
				{
					string CurrentToken = objMatch.Value;
					if (objMatch.Value.IndexOf(":") == -1)
					{
						//simple replace
						Result = Result.Replace(CurrentToken, ReplaceWithValue);
					}
					else
					{
						//need to get substring or format date
						try
						{
							string[] lstSub = CurrentToken.Split(new char[] { '[', ']', ':' }, StringSplitOptions.RemoveEmptyEntries);
							switch (lstSub[0].ToUpper())
							{
								case "DATETIME":
									Result = Result.Replace(CurrentToken, string.Format("{0:" + lstSub[1] + "}", DateTime.Now));
									break;
								case "RANDOM":
									System.Random objRandom = new Random();
									string RandomValue = objRandom.NextDouble().ToString().Substring(2);
									Result = Result.Replace(CurrentToken, RandomValue.Substring(Convert.ToInt32(lstSub[1]), Convert.ToInt32(lstSub[2])));
									break;
								default:
									Result = Result.Replace(CurrentToken, ReplaceWithValue.Substring(Convert.ToInt32(lstSub[1]), Convert.ToInt32(lstSub[2])));
									break;
							}
						}
						catch
						{
							Result = Result.Replace(CurrentToken, "");
						}
					}
				}
				return Result;
			}
			catch
			{
				return Source;
			}
		}

		public static string SuggestSingleUserNames(string Template, NameValueCollection htParams)
		{
			string Result = ParseToken(Template, "FirstName", htParams["firstName"]);
			Result = ParseToken(Result, "LastName", htParams["lastName"]);
			Result = ParseToken(Result, "UserName", htParams["userName"]);
			Result = ParseToken(Result, "DateTime", "");
			Result = ParseToken(Result, "Random", "");
			return Result.Trim();
		}

		public static string SuggestUserNames(string ResourceFile, NameValueCollection htParams)
		{
			int PortalID = -1;
			try
			{
				PortalID = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalId;
			}
			catch (Exception Exc)
			{
				return string.Format("Exception, can't detect PortalID: {0}", Exc.Message);
			}

			ufRegistrationSettingInfo objSettings = ufRegistrationSettingController.ufRegistrationSetting_GetByPortalID(PortalID);

			StringBuilder sb = new StringBuilder();
			foreach (string Template in objSettings.SuggestionTemplates.Split(new char[] { '\n' }))
			{
				string Suggestion = SuggestSingleUserNames(Template, htParams);
				if ((Suggestion.Trim() == "") || (Suggestion.Length < 3))
				{
					continue;
				}
				if (CommonController.UserNameExists(-1, Suggestion))
				{
					continue;
				}
				sb.AppendFormat(Localization.GetString("SuggestionFormat", ResourceFile), Suggestion);
			}
			return sb.ToString();
		}

		#endregion
	}
}
