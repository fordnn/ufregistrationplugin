using System;
using System.Data;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public class ufRegistrationSettingInfo
	{
		#region Private Members

		private int _ufRegistrationSettingID;
		private int _PortalID;
		private bool _VerifyUserName;
		private bool _SuggestUserName;
		private string _SuggestionTemplates;
		private bool _VerifyEmail;
		private bool _VerifyPassword;

		private string _VerifyingMessage;

		#endregion

		#region Constructors

		public ufRegistrationSettingInfo()
		{
		}

		public ufRegistrationSettingInfo(int initPortalID)
		{
			this.PortalID = initPortalID;
		}

		#endregion

		#region Public Properties


		public int ufRegistrationSettingID
		{
			get
			{
				return _ufRegistrationSettingID;
			}
			set
			{
				_ufRegistrationSettingID= value;
			}
		}

		public int PortalID
		{
			get
			{
				return _PortalID;
			}
			set
			{
				_PortalID= value;
			}
		}

		public bool VerifyUserName
		{
			get
			{
				return _VerifyUserName;
			}
			set
			{
				_VerifyUserName= value;
			}
		}

		public bool SuggestUserName
		{
			get
			{
				return _SuggestUserName;
			}
			set
			{
				_SuggestUserName= value;
			}
		}

		public string SuggestionTemplates
		{
			get
			{
				return _SuggestionTemplates;
			}
			set
			{
				_SuggestionTemplates= value;
			}
		}

		public bool VerifyEmail
		{
			get
			{
				return _VerifyEmail;
			}
			set
			{
				_VerifyEmail= value;
			}
		}

		public bool VerifyPassword
		{
			get
			{
				return _VerifyPassword;
			}
			set
			{
				_VerifyPassword= value;
			}
		}

		public string VerifyingMessage
		{
			get
			{
				return _VerifyingMessage;
			}
			set
			{
				_VerifyingMessage = value;
			}
		}


		#endregion
	}
}		
