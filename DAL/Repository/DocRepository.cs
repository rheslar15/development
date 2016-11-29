using DAL.DAL;
using DAL.DO;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public class DocRepository:IRepository<DocumentDO>
	{
		DBUtlity db;

		public DocRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}

		public DocumentDO GetEntity(int id)
		{
			DocumentDO entity = db.GetItem<DocumentDO>(id);
			return entity;
		}

		public IEnumerable<DocumentDO> GetEntities()
		{
			IEnumerable<DocumentDO> entities = db.GetItems<DocumentDO>();
			return entities;
		}

		public int SaveEntity(DocumentDO item)
		{
			return db.SaveItem<DocumentDO>(item);
		}

		public int UpdateEntity(DocumentDO item)
		{
			return db.UpdateItem<DocumentDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<DocumentDO>(id);
		}
	}
}

