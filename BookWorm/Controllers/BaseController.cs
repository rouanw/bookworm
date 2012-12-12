using System.Web.Mvc;
using BookWorm.Models;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var allPages = _repository.List<StaticPage>();
            ViewBag.StaticPages = allPages;
            base.OnActionExecuting(filterContext);
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
