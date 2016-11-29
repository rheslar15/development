using System;

namespace Model
{
	public class UserSetting:IConfig
	{ 
		int _id;
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		string _settingName;
		public string SettingName
		{
			get { return _settingName; }
			set { _settingName = value; }
		}

		string _settingValue;
		public string SettingValue
		{
			get { return _settingValue; }
			set { _settingValue = value; }
		}
	}
}

