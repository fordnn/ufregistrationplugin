using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;

namespace forDNN.Modules.ufRegistrationPlugin
{

	partial class ViewufRegistrationPlugin : PortalModuleBase, IActionable
	{

		#region "Event Handlers"

		private void ExtraInitControl()
		{
			HtmlLink css = new HtmlLink();
			css.Href = ResolveUrl("ufRegistrationPlugin.css");
			css.Attributes["rel"] = "stylesheet";
			css.Attributes["type"] = "text/css";
			css.Attributes["media"] = "all";
			Page.Header.Controls.Add(css);

			btnActivatePlugin.Text = IsPluginInstalled() ?
				Localization.GetString("DeActivatePlugin", this.LocalResourceFile) : Localization.GetString("ActivatePlugin", this.LocalResourceFile);

			if(!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "knockout"))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "knockout",
					string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", ResolveUrl("js/knockout-2.2.1.js")));
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				ExtraInitControl();

				if (!IsPostBack)
				{
					LoadSettings();
				}
			}

			catch (Exception exc)
			{
				//Module failed to load 
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void LoadSettings()
		{
			ufRegistrationSettingInfo objSettings = ufRegistrationSettingController.ufRegistrationSetting_GetByPortalID(this.PortalId);
			if (objSettings == null)
			{
				cbSuggestUserName.Checked = true;
				tbSuggestionTemplates.Text = CommonController.DefaultSuggestionTemplates;
			}
			else
			{
				cbVerifyUserName.Checked = objSettings.VerifyUserName;
				cbSuggestUserName.Checked = objSettings.SuggestUserName;
				tbSuggestionTemplates.Text = objSettings.SuggestionTemplates;
				cbVerifyEmail.Checked = objSettings.VerifyEmail;
				cbVerifyPassword.Checked = objSettings.VerifyPassword;
			}
		}

		#endregion

		#region "Optional Interfaces"

		public ModuleActionCollection ModuleActions
		{
			get
			{
				ModuleActionCollection Actions = new ModuleActionCollection();
				//Actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
				//   ModuleActionType.AddContent, "", "add.gif", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
				//    true, false);
				return Actions;
			}
		}

		#endregion

		protected void btnSave_Click(object sender, EventArgs e)
		{
			ufRegistrationSettingInfo objSettings = ufRegistrationSettingController.ufRegistrationSetting_GetByPortalID(this.PortalId);
			bool IsNew = false;
			if (objSettings == null)
			{
				objSettings = new ufRegistrationSettingInfo(this.PortalId);
				IsNew = true;
			}

			objSettings.VerifyUserName = cbVerifyUserName.Checked;
			objSettings.SuggestUserName = cbSuggestUserName.Checked;
			objSettings.SuggestionTemplates = tbSuggestionTemplates.Text;
			objSettings.VerifyEmail = cbVerifyEmail.Checked;
			objSettings.VerifyPassword = cbVerifyPassword.Checked;

			if (IsNew)
			{
				ufRegistrationSettingController.ufRegistrationSetting_Add(objSettings);
			}
			else
			{
				ufRegistrationSettingController.ufRegistrationSetting_Update(objSettings);
			}
		}

		private bool IsPluginInstalled()
		{ 
			string FileName = Server.MapPath(ResolveUrl("Register.ascx"));
			System.IO.StreamReader sr = new System.IO.StreamReader(FileName);
			string Code = sr.ReadToEnd();
			return (Code.IndexOf("ufRegistrationPlugin.js") != -1);
		}

		protected void btnActivatePlugin_Click(object sender, EventArgs e)
		{
			string FileName = Server.MapPath(ResolveUrl("Register.ascx"));
			System.IO.StreamReader sr = new System.IO.StreamReader(FileName);
			string Code = sr.ReadToEnd();
			sr.Close();

			string InitPluginCode = @"
<!--ufRegistrationPlugin Begin-->
<script type=""text/javascript"" src=""{0}""></script>
<script type=""text/javascript"">
	$(document).ready(function()
	{
		ufRegistrationPlugin.initPlugin(""<%=userForm.ClientID%>"", ""{1}"");
	});
</script>
<!--ufRegistrationPlugin End-->"
				.Replace("{0}", this.ResolveUrl("js/ufRegistrationPlugin.js"))
				.Replace("{1}", this.ResolveUrl("ufRegistrationScript.aspx"));

			string ErrorMessage = "";

			if (Code.IndexOf("ufRegistrationPlugin.js") == -1)
			{ 
				//install plugin
				Code = Code + InitPluginCode;
				ErrorMessage = 
					string.Format(Localization.GetString("ActivateAccessDenied", this.LocalResourceFile),
						this.ResolveUrl("Register.ascx"),
						Server.HtmlEncode(InitPluginCode));
			}
			else
			{ 
				//uninstall plugin
				Regex objRegEx = new Regex(
					  "\\<\\!\\-\\-ufRegistrationPlugin\\x20Begin.+?ufRegistrationPlugin\\x20End\\-\\-\\>",
					RegexOptions.IgnoreCase
					| RegexOptions.Multiline
					| RegexOptions.Singleline
					| RegexOptions.CultureInvariant
					| RegexOptions.IgnorePatternWhitespace
					| RegexOptions.Compiled
					);
				Code = objRegEx.Replace(Code, "");
				ErrorMessage =
					string.Format(Localization.GetString("DeActivateAccessDenied", this.LocalResourceFile),
						this.ResolveUrl("Register.ascx"),
						Server.HtmlEncode(InitPluginCode));
			}

			try
			{
				System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, false);
				sw.Write(Code);
				sw.Close();
			}
			catch
			{
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this,
					ErrorMessage,
					DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
			}
		}

		protected void btnRestoreDefaultTemplates_Click(object sender, EventArgs e)
		{
			tbSuggestionTemplates.Text = CommonController.DefaultSuggestionTemplates;
		}

	}

}