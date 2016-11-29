using DAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> //where T : IDomianObject
    {
        T GetEntity(int id);
        IEnumerable<T> GetEntities();
        int SaveEntity(T item);
		int UpdateEntity(T item);
        int DeleteEntity(int id);
        //T SaveEntity(Action action);
    }
}
