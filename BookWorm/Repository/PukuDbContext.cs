using System;
using System.Data;
using System.Data.Entity;
using BookWorm.Models;

namespace BookWorm.Repository
{
    public class PukuDbContext : DbContext
    {
        // TODO can this be private?
        public IDbSet<Book> Books { get; set; }
        public IDbSet<StaticPage> Pages { get; set; } 

        public virtual IDbSet<T> GetDbSet<T>() where T : Model<T>
        {
            throw new NotImplementedException();
        }

        public virtual void SetModified<T>(T entity) where T : Model<T>
        {
            //TODO Write integration test
            Entry(entity).State = EntityState.Modified;
        }
    }
}