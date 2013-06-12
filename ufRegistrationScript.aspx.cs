using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public partial class ufRegistrationScript : System.Web.UI.Page
	{

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		private string ResourceFile = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ResourceFile = ResolveUrl("App_LocalResources/ufRegistrationScript.aspx.resx");
			DrawAction();
		}

		private void DrawAction()
		{
			string Result = "";

			string Action = Request.QueryString["action"].ToLower();

			try
			{
				switch (Action)
				{
					case "checkusername":
						Result = CheckUserName();
						break;
					case "verifyemail":
						Result = VerifyEmail();
						break;
					case "verifypassword":
						Result = VerifyPassword();
						break;
					case "getsettings":
						Result = GetSettings();
						break;
				}
			}
			catch (Exception Exc)
			{
				ResponseInfo objResponse = new ResponseInfo();
				objResponse.success = false;
				objResponse.innerHTML = Exc.Message;

				DotNetNuke.Services.Log.EventLog.ExceptionLogController objController = new DotNetNuke.Services.Log.EventLog.ExceptionLogController();
				objController.AddLog(Exc, DotNetNuke.Services.Log.EventLog.ExceptionLogController.ExceptionLogType.GENERAL_EXCEPTION);

				System.Web.Script.Serialization.JavaScriptSerializer objSerializer =
						 new System.Web.Script.Serialization.JavaScriptSerializer();
				Result = objSerializer.Serialize(objResponse);
			}

			System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

			byte[] lstByte = System.Text.Encoding.UTF8.GetBytes(Result);

			Response.Clear();
			Response.ClearHeaders();
			Response.ClearContent();
			Response.ContentType = "application/json; charset=utf-8";
			Response.AppendHeader("Content-Length", lstByte.Length.ToString());
			Response.BinaryWrite(lstByte);
			Response.Flush();
			Response.End();
		}

		private string GetSettings()
		{
			ufRegistrationSettingInfo objSettings =
				ufRegistrationSettingController.ufRegistrationSetting_GetByPortalID(DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalId);
			if (objSettings == null)
			{
				objSettings = new ufRegistrationSettingInfo();
			}
			objSettings.VerifyingMessage = Localization.GetString("VerifyingMessage", ResourceFile);

			System.Web.Script.Serialization.JavaScriptSerializer objSerializer =
					 new System.Web.Script.Serialization.JavaScriptSerializer();
			string Result = objSerializer.Serialize(objSettings);

			return Result;
		}

		private string CheckUserName()
		{
			string Result = "";

			System.Web.HttpContext objContext = System.Web.HttpContext.Current;
			string Income = objContext.Request.ContentEncoding.GetString(objContext.Request.BinaryRead(objContext.Request.ContentLength));
			NameValueCollection htParams = System.Web.HttpUtility.ParseQueryString(Income);

			//Check if UserName already exists
			if (CommonController.UserNameExists(-1, htParams["userName"]))
			{
				Result = string.Format(Localization.GetString("UserNameExists", ResourceFile),
					htParams["userName"],
					CommonController.SuggestUserNames(ResourceFile, htParams));
			}

			ResponseInfo objResponse = new ResponseInfo();
			objResponse.success = true;
			objResponse.innerHTML = Result;

			System.Web.Script.Serialization.JavaScriptSerializer objSerializer =
					 new System.Web.Script.Serialization.JavaScriptSerializer();
			Result = objSerializer.Serialize(objResponse);

			return Result;
		}

		private string VerifyEmail()
		{
			StringBuilder Result = new StringBuilder();

			System.Web.HttpContext objContext = System.Web.HttpContext.Current;
			string Income = objContext.Request.ContentEncoding.GetString(objContext.Request.BinaryRead(objContext.Request.ContentLength));
			NameValueCollection htParams = System.Web.HttpUtility.ParseQueryString(Income);

			if (htParams["email"].Trim() != "")
			{
				//Check if requires uniq email
				if (DotNetNuke.Security.Membership.MembershipProvider.Instance().RequiresUniqueEmail)
				{
					if (CommonController.EmailExists(-1, htParams["email"]))
					{
						Result = Result.AppendFormat(Localization.GetString("EmailExists", ResourceFile), htParams["email"]);
					}
				}

				//Check if mailserver exists
				Result.Append(DNSController.IsMailServerExists(htParams["email"], ResourceFile));
			}

			ResponseInfo objResponse = new ResponseInfo();
			objResponse.success = true;
			objResponse.innerHTML = Result.ToString();

			System.Web.Script.Serialization.JavaScriptSerializer objSerializer =
					 new System.Web.Script.Serialization.JavaScriptSerializer();
			return objSerializer.Serialize(objResponse);
		}

		private string VerifyPassword()
		{
			StringBuilder Result = new StringBuilder();

			System.Web.HttpContext objContext = System.Web.HttpContext.Current;
			string Income = objContext.Request.ContentEncoding.GetString(objContext.Request.BinaryRead(objContext.Request.ContentLength));
			NameValueCollection htParams = System.Web.HttpUtility.ParseQueryString(Income);

			if ((htParams["password"]!="") && (!DotNetNuke.Entities.Users.UserController.ValidatePassword(htParams["password"])))
			{
				Result = Result.AppendFormat(Localization.GetString("PasswordIsNotValid", ResourceFile),
					UserController.GetUserCreateStatus(DotNetNuke.Security.Membership.UserCreateStatus.InvalidPassword));
			}

			ResponseInfo objResponse = new ResponseInfo();
			objResponse.success = true;
			objResponse.innerHTML = Result.ToString();

			System.Web.Script.Serialization.JavaScriptSerializer objSerializer =
					 new System.Web.Script.Serialization.JavaScriptSerializer();
			return objSerializer.Serialize(objResponse);
		}

	}
}
