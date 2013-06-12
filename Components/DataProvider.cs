using System;
using System.Data;
using DotNetNuke;

namespace forDNN.Modules.ufRegistrationPlugin
{

	public abstract class DataProvider
	{

		#region "Shared/Static Methods"

		/// <summary>
		/// singleton reference to the instantiated object 
		/// </summary>
		private static DataProvider objProvider = null;

		/// <summary>
		/// constructor
		/// </summary>
		static DataProvider()
		{
			CreateProvider();
		}

		/// <summary>
		/// dynamically create provider 
		/// </summary>
		private static void CreateProvider()
		{
			objProvider = (DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "forDNN.Modules.ufRegistrationPlugin", "");
		}

		/// <summary>
		/// return the provider 
		/// </summary>
		/// <returns></returns>
		public static DataProvider Instance()
		{
			return objProvider;
		}

		#endregion

		#region "Abstract methods for verifycations"

		public abstract bool UserNameExists(int PortalID, string UserName);
		public abstract bool EmailExists(int PortalID, string Email);

		#endregion

		#region Abstract Methods ufRegistrationSettings

		public abstract int ufRegistrationSetting_Add(ufRegistrationSettingInfo objInfo);
		public abstract void ufRegistrationSetting_Delete(int ufRegistrationSettingID);
		public abstract void ufRegistrationSetting_Update(ufRegistrationSettingInfo objInfo);
		public abstract IDataReader ufRegistrationSetting_GetByPrimaryKey(int ufRegistrationSettingID);
		public abstract IDataReader ufRegistrationSetting_GetAllItems(string SortBy);
		public abstract IDataReader ufRegistrationSetting_GetByPortalID(int PortalID);

		#endregion

	}
}