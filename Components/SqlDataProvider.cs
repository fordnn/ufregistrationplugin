using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace forDNN.Modules.ufRegistrationPlugin
{

	public class SqlDataProvider : DataProvider
	{

		#region "Private Members"

		private const string ProviderType = "data";
		private const string ModuleQualifier = "forDNN_";

		private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;

		#endregion

		#region "Constructors"

		public SqlDataProvider()
		{

			// Read the configuration specific information for this provider 
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

			// Read the attributes for this provider 

			//Get Connection string from web.config 
			_connectionString = Config.GetConnectionString();

			if (_connectionString == "")
			{
				// Use connection string specified in provider 
				_connectionString = objProvider.Attributes["connectionString"];
			}

			_providerPath = objProvider.Attributes["providerPath"];

			_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false)
			{
				_objectQualifier += "_";
			}

			_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false)
			{
				_databaseOwner += ".";
			}

		}

		#endregion

		#region "Properties"

		public string ConnectionString
		{
			get { return _connectionString; }
		}

		public string ProviderPath
		{
			get { return _providerPath; }
		}

		public string ObjectQualifier
		{
			get { return _objectQualifier; }
		}

		public string DatabaseOwner
		{
			get { return _databaseOwner; }
		}

		#endregion

		#region "Private Methods"

		private string GetFullyQualifiedName(string name)
		{
			return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
		}

		private object GetNull(object Field)
		{
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}

		#endregion

		#region "Verifycations Methods"

		public override bool UserNameExists(int PortalID, string UserName)
		{
			object objResult = SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("ufRegistrationSetting_UserNameExists"), PortalID, UserName);
			return (objResult != null);
		}

		public override bool EmailExists(int PortalID, string Email)
		{
			object objResult = SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("ufRegistrationSetting_EmailExists"), PortalID, Email);
			return (objResult != null);
		}

		#endregion

		#region ufRegistrationSettings Methods

		public override int ufRegistrationSetting_Add(ufRegistrationSettingInfo objufRegistrationSetting)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_Add"),
				objufRegistrationSetting.PortalID,
				objufRegistrationSetting.VerifyUserName,
				objufRegistrationSetting.SuggestUserName,
				objufRegistrationSetting.SuggestionTemplates,
				objufRegistrationSetting.VerifyEmail,
				objufRegistrationSetting.VerifyPassword));
			return intResID;
		}
		public override void ufRegistrationSetting_Update(ufRegistrationSettingInfo objufRegistrationSetting)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_Update"),
				objufRegistrationSetting.ufRegistrationSettingID,
				objufRegistrationSetting.PortalID,
				objufRegistrationSetting.VerifyUserName,
				objufRegistrationSetting.SuggestUserName,
				objufRegistrationSetting.SuggestionTemplates,
				objufRegistrationSetting.VerifyEmail,
				objufRegistrationSetting.VerifyPassword);
		}
		public override void ufRegistrationSetting_Delete(int ufRegistrationSettingID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_Delete"),
				ufRegistrationSettingID);
		}
		public override IDataReader ufRegistrationSetting_GetByPrimaryKey(int ufRegistrationSettingID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_GetByPrimaryKey"),
				ufRegistrationSettingID);
		}
		public override IDataReader ufRegistrationSetting_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_GetAllItems"), SortBy);
		}
		public override IDataReader ufRegistrationSetting_GetByPortalID(int PortalID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ufRegistrationSetting_GetByPortalID"),
				PortalID);
		}

		#endregion

	}
}