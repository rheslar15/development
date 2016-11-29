using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using SQLite;

namespace DAL.DO
{
    [Table("Configuration")]
	public class ConfigurationDO:IDomianObject
	{
		int _ConfigurationID;

		[PrimaryKey, AutoIncrement]
        [Column("ConfigurationID")]
		public int ID
		{
			get { return _ConfigurationID; }
			set { _ConfigurationID = value; }
		}

		string _ConfigurationDesc;
        public string ConfigurationDesc
		{
			get { return _ConfigurationDesc; }
			set { _ConfigurationDesc = value; }
		}


		string _ConfigurationUrl;
        public string ConfigurationUrl
		{
			get
			{
				return _ConfigurationUrl;
			}
			set
			{
				_ConfigurationUrl = value;
			}
		}

		bool _Flag;
        public bool Flag
		{
			get
			{
				return _Flag;
			}
			set
			{
				_Flag = value;
			}
		}

		bool _Use;
        public bool Use
		{
			get
			{
				return _Use;
			}
			set
			{
				_Use = value;
			}
		}
        
		public static List<Configuration> getConfiguration(SQLiteConnection conn)
		{
			List<Configuration> URLConfiguration = new List<Configuration> ();
			List<ConfigurationDO> configList = conn.Table<ConfigurationDO>().ToList();
			foreach (var config in configList) {
				URLConfiguration.Add(
					new Configuration(){
						ID=config.ID,
						ConfigUrl=config.ConfigurationUrl,
						IsDefault=config.Flag,
						ConfigDesc=config.ConfigurationDesc,
						use=config.Use
					});
			}
			return URLConfiguration;
		}

		public static void InsertConfiguration(SQLiteConnection conn, Configuration con)
		{
			string Query = "insert into Configuration(ConfigurationDesc,ConfigurationUrl) values('" + con.ConfigDesc + "','" + con.ConfigUrl +  "');";
			conn.Execute(Query);
		}

		public static int UpdateConfiguration(SQLiteConnection con, Configuration conf)
		{
			int val = 0;
			if (conf.use) {
				val = 1;
			} else {
				val = 0;
			}
			string Query = "update Configuration  set ConfigurationUrl='" + conf.ConfigUrl + "',Use='" + val + "' Where ConfigurationID= '" + conf.ID  + "';";
			int x= con.Execute(Query);
			return x;
		}
	}
}