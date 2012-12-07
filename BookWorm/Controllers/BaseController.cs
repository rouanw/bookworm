﻿using System.Web.Mvc;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        private IDocumentSession _session;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _session = GetDocumentStore().OpenSession();
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

            using (_session)
            {
                if (filterContext.Exception != null)
                    return;

                if (_session != null)
                    _session.SaveChanges();
            }
        }
    }
}