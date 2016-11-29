using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;

namespace DAL.DO
{
	[Table("UserSetting")]
	public class UserSettingDO:IDomianObject
	{
		int id;
		[PrimaryKey, AutoIncrement]
		[Column("ID")]
		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		string _SettingName;
		public string SettingName
		{
			get { return _SettingName; }
			set { _SettingName = value; }
		}

		string _SettingValue;
		public string SettingValue
		{
			get { return _SettingValue; }
			set { _SettingValue = value; }
		}
	}
}