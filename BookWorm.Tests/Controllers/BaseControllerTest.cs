using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;

namespace BookWorm.Tests.Controllers
{
    internal class TestBaseController : BaseController
    {
        private IDocumentStore _documentStore;

        public TestBaseController(IDocumentStore documentStore, Repository repository) :base(repository)
        {
            _documentStore = documentStore;
        }
        public TestBaseController(Repository repository) : base(repository)
        {
        }

        protected override IDocumentStore GetDocumentStore()
        {
            if (_documentStore == null)
            {
                var documentSession = new Mock<IDocumentSession>();
                var documentStore = new Mock<IDocumentStore>();
                documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);
                _documentStore = documentStore.Object;
            }
            return _documentStore;
        }

        public new void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public new void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }

    [TestClass]
    public class BaseControllerTest
    {
        [TestMethod]
        public void ShouldKnowToNotSaveChangesOnActionExecutedWhenRunOnChildAction()
        {
            var repository = new Mock<Repository>();
            repository.Setup(repo => repo.SaveChanges());
            var documentStore = new Mock<IDocumentStore>();
  
            var testBaseController = new TestBaseController(documentStore.Object, repository.Object);

            var actionExecutedContext = new Mock<ActionExecutedContext>();
            actionExecutedContext.SetupGet(context => context.IsChildAction).Returns(true);
            actionExecutedContext.Object.Exception = null;

            testBaseController.OnActionExecuted(actionExecutedContext.Object);

            repository.Verify(repo => repo.SaveChanges(), Times.Never());
        }

        [TestMethod]
        public void ShouldKnowToNotSaveChangesOnActionExecutedWhenExceptionPresentInContext()
        {
            var repository = new Mock<Repository>();
            repository.Setup(repo => repo.SaveChanges());
            var documentStore = new Mock<IDocumentStore>();
            var testBaseController = new TestBaseController(documentStore.Object, repository.Object);
            var actionExecutedContext = new Mock<ActionExecutedContext>();
            actionExecutedContext.SetupGet(context => context.IsChildAction).Returns(false);
            actionExecutedContext.Setup(context => context.Exception).Returns(new Exception("test exception"));

            testBaseController.OnActionExecuted(actionExecutedContext.Object);

            repository.Verify(repo => repo.SaveChanges(), Times.Never());   
        }
    }
}
