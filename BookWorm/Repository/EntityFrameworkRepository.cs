using System;
using System.Collections.Generic;

namespace BookWorm.Repository
{
    public class EntityFrameworkRepository : IRepository
    {
        private PukuDbContext _pukuDbContext;

        public EntityFrameworkRepository(PukuDbContext pukuDbContext)
        {
            _pukuDbContext = pukuDbContext;
        }

        public Model<T> Create<T>(T model) where T : Model<T>
        {
            return _pukuDbContext.GetDbSet<T>().Add(model);
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