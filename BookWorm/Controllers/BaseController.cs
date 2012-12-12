using System.Web.Mvc;
using BookWorm.Repository;
using Ninject;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IRepository _repository;

        public BaseController()
        {
        }

        [Inject]
        public BaseController(IRepository repository)
        {
            _repository = repository;
        }

        protected virtual IDocumentStore GetDocumentStore()
        {
            return MvcApplication.Store;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            if (filterContext.Exception != null)
                return;

            _repository.SaveChanges();
        }
    }
}
