using System;
using System.Data;
using System.Data.Entity;
using BookWorm.Models;

namespace BookWorm.Repository
{
    public class PukuDbContext : DbContext
    {
        protected PukuDbContext(string databaseName) : base(databaseName)
        {
            
        }

        public PukuDbContext()
        {
            
        }

        // TODO can this be private?
        public IDbSet<Book> Books { get; set; }
        public IDbSet<StaticPage> Pages { get; set; } 

        public virtual IDbSet<T> GetDbSet<T>() where T : Model<T>
        {
            if (typeof(T) == typeof(Book))
            {
                return (IDbSet<T>) Books;
            }
            else if (typeof(T) == typeof(StaticPage))
            {
                return (IDbSet<T>) Pages;
            }
            throw new ArgumentException("No DbSet exists for type " + typeof(T));
        }

        public virtual void SetModified<T>(T entity) where T : Model<T>
        {
            //TODO Write integration test
            Entry(entity).State = EntityState.Modified;
        }
    }
}