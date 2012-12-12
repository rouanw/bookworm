using System;
using System.Collections.Generic;

namespace BookWorm.Repository
{
    public class EntityFrameworkRepository : IRepository
    {
        protected PukuDbContext _pukuDbContext;
        public PukuDbContext PukuDbContext { get { return _pukuDbContext ?? (_pukuDbContext = new PukuDbContext()); } }

        public EntityFrameworkRepository()
        {
        
        }

        public EntityFrameworkRepository(PukuDbContext pukuDbContext)
        {
            _pukuDbContext = pukuDbContext;
        }

        public Model<T> Create<T>(T model) where T : Model<T>
        {
            return PukuDbContext.GetDbSet<T>().Add(model);
        }

        public void SaveChanges()
        {
            PukuDbContext.SaveChanges();
        }

        public T Get<T>(int id) where T : Model<T>
        {
            return PukuDbContext.GetDbSet<T>().Find(id);
        }

        public ICollection<T> List<T>() where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(int id) where T : Model<T>
        {
            var dbSet = PukuDbContext.GetDbSet<T>();
            var modelToDelete = dbSet.Find(id);
            dbSet.Remove(modelToDelete);
        }

        public void Edit<T>(T editedModel)
        {
            throw new NotImplementedException();
        }
    }
}