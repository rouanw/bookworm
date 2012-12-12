using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookWorm.Repository
{
    public class EntityFrameworkRepository : IRepository
    {
        public Model<T> Create<T>(T model) where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int id) where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public ICollection<T> List<T>() where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(int id) where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public void Edit<T>(T editedModel)
        {
            throw new NotImplementedException();
        }
    }
}