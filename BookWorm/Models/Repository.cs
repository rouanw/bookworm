using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace BookWorm.Models
{
    public class Repository : IRepository
    {
        private IDocumentSession _documentSession;

        public Repository()
        {
        }

        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        private IDocumentSession DocumentSession
        {
            get { return _documentSession ?? (_documentSession = GetDocumentStore().OpenSession()); }
        }

        protected virtual IDocumentStore GetDocumentStore()
        {
            return MvcApplication.Store;
        }

        public virtual Model<T> Create<T>(T model) where T : Model<T>
        {
            DocumentSession.Store(model);
            return model;
        }

        public virtual T Get<T>(int id) where T : Model<T>
        {
            return DocumentSession.Load<T>(id);
        }

        public virtual ICollection<T> List<T>() where T : Model<T>
        {
            return DocumentSession.Query<T>().ToList();
        }

        public virtual void Delete<T>(int id) where T : Model<T>
        {
            var model = Get<T>(id);
            DocumentSession.Delete(model);
        }

        public virtual void Edit<T>(T editedModel)
        {
            DocumentSession.Store(editedModel);
        }

        public virtual void SaveChanges()
        {
            if (_documentSession != null)
            {
                _documentSession.SaveChanges();
            }
        }
    }

    public class Model<T>
    {
        public int Id { get; set; }
    }
}