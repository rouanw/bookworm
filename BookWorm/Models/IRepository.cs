using System.Collections.Generic;
using Raven.Client;

namespace BookWorm.Models
{
    public interface IRepository
    {
        Model<T> Create<T>(T model) where T : Model<T>;
        void SaveChanges();
        T Get<T>(int id) where T : Model<T>;
        ICollection<T> List<T>() where T : Model<T>;
        void Delete<T>(int id) where T : Model<T>;
        void Edit<T>(T editedModel);
    }
}