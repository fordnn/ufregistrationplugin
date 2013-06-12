using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public class ufRegistrationSettingController
	{
		#region Public Methods
		
		private static string KeyFormat = "forDNN.Modules.ufRegistrationSetting_ufRegistrationSettingID_{0}";

		public static int ufRegistrationSetting_Add(ufRegistrationSettingInfo objufRegistrationSetting)
		{
			objufRegistrationSetting.ufRegistrationSettingID = ((int)DataProvider.Instance().ufRegistrationSetting_Add(objufRegistrationSetting));
			CommonController.UpdateCache(KeyFormat, objufRegistrationSetting.ufRegistrationSettingID, objufRegistrationSetting, "");
			return objufRegistrationSetting.ufRegistrationSettingID;
		}
		public static void ufRegistrationSetting_Delete(ufRegistrationSettingInfo objufRegistrationSetting)
		{
			ufRegistrationSettingController.ufRegistrationSetting_Delete(objufRegistrationSetting.ufRegistrationSettingID);
		}
		public static void ufRegistrationSetting_Delete(int ufRegistrationSettingID)
		{
			DataProvider.Instance().ufRegistrationSetting_Delete(ufRegistrationSettingID);
			CommonController.UpdateCache(KeyFormat, ufRegistrationSettingID, null, "");
		}
		public static void ufRegistrationSetting_Update(ufRegistrationSettingInfo objufRegistrationSetting)
		{
			DataProvider.Instance().ufRegistrationSetting_Update(objufRegistrationSetting);
			CommonController.UpdateCache(KeyFormat, objufRegistrationSetting.ufRegistrationSettingID, objufRegistrationSetting, "");
		}
		public static ufRegistrationSettingInfo ufRegistrationSetting_GetByPrimaryKey(int ufRegistrationSettingID)
		{
			object objufRegistrationSetting = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, ufRegistrationSettingID));
			if (objufRegistrationSetting == null)
			{
				objufRegistrationSetting = ((ufRegistrationSettingInfo)CBO.FillObject(DataProvider.Instance().ufRegistrationSetting_GetByPrimaryKey(ufRegistrationSettingID), typeof(ufRegistrationSettingInfo)));
				DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, ufRegistrationSettingID), objufRegistrationSetting);
			}
			return (ufRegistrationSettingInfo)objufRegistrationSetting;
		}
		public static List<ufRegistrationSettingInfo> ufRegistrationSetting_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<ufRegistrationSettingInfo>(DataProvider.Instance().ufRegistrationSetting_GetAllItems(SortBy));
		}
		public static ufRegistrationSettingInfo ufRegistrationSetting_GetByPortalID(int PortalID)
		{
			object objufRegistrationSetting = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat + "_PortalID", PortalID));
			if (objufRegistrationSetting == null)
			{
				objufRegistrationSetting = ((ufRegistrationSettingInfo)CBO.FillObject(DataProvider.Instance().ufRegistrationSetting_GetByPortalID(PortalID), typeof(ufRegistrationSettingInfo)));
				DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat + "_PortalID", PortalID), objufRegistrationSetting);
			}
			return (ufRegistrationSettingInfo)objufRegistrationSetting;
		}
		#endregion
	}
}
