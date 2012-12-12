using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;

namespace BookWorm.Tests.Repository
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void ShouldKnowThatCreatedModelIsAssignedAnId()
        {
            var testModel = new StaticPage();
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Store(testModel)).Callback<object>(arg => testModel.Id = 1);

            var repository = new BookWorm.Repository.Repository(documentSession.Object);
            repository.Create(testModel);

            documentSession.Verify(session => session.Store(testModel), Times.Once());
            Assert.AreNotEqual(0, testModel.Id);
        }

        [TestMethod]
        public void ShouldKnowHowToGetModelFromTheirId()
        {
            var persistedModel = new StaticPage {Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it."};
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new BookWorm.Repository.Repository(documentSession.Object);
            var retrievedModel = repository.Get<StaticPage>(persistedModel.Id);

            documentSession.Verify(session=>session.Load<StaticPage>(persistedModel.Id), Times.Once());
            Assert.AreEqual(persistedModel.Id , retrievedModel.Id);
        }

        //
        [TestMethod]
        [ExpectedException(typeof(Raven.Client.Exceptions.NonUniqueObjectException))]
        public void ShouldRethrowRavenNonUniqueObjectException()
        {
            var testModel = new StaticPage();
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Store(testModel))
                           .Throws(new Raven.Client.Exceptions.NonUniqueObjectException());

            var repository = new BookWorm.Repository.Repository(documentSession.Object);
            repository.Create(testModel);
        }

        [TestMethod]
        public void ShouldKnowHowToDeleteAModelByItsIdWhenItExists()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new BookWorm.Repository.Repository(documentSession.Object);

            repository.Delete<StaticPage>(persistedModel.Id);
            documentSession.Verify(session => session.Delete(persistedModel), Times.Once()); 
        }

        //
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void DeletingNonExistentDocumentShouldRethrowInvalidOperationException()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            documentSession.Setup(session => session.Delete(persistedModel))
                           .Throws(new System.InvalidOperationException());

            var repository = new BookWorm.Repository.Repository(documentSession.Object);
            repository.Delete<StaticPage>(persistedModel.Id);
        }

        [TestMethod]
        public void ShouldKnowHowToEditAModelByItsIdWhenItExists()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new BookWorm.Repository.Repository(documentSession.Object);

            repository.Edit<StaticPage>(persistedModel);

            documentSession.Verify(session => session.Store(persistedModel), Times.Once());
        }

        //
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void EditingNonExistentDocumentShouldRethrowInvalidOperationException()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes veggie burger. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            documentSession.Setup(session => session.Store(persistedModel))
                           .Throws(new System.InvalidOperationException());

            var repository = new BookWorm.Repository.Repository(documentSession.Object);
            repository.Edit<StaticPage>(persistedModel);
        }

        [TestMethod]
        public void ShouldKnowToTalkToSessionToSaveChanges()
        {
            var documentSession = new Mock<IDocumentSession>();
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);

            var repo = new BookWorm.Repository.Repository(documentSession.Object);

            repo.SaveChanges();

            documentSession.Verify(session => session.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowToOpenSessionOnceOnly()
        {
            var book = new Book();
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Store(book));
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);

            var repository = new TestRepository(documentStore.Object);
            repository.Create(book);

            documentStore.Verify(store => store.OpenSession(), Times.Once());
        }

        public class TestRepository : BookWorm.Repository.Repository 
        {
            private readonly IDocumentStore _documentStore;

            public TestRepository(IDocumentStore documentStore)
            {
                _documentStore = documentStore;
            }

            protected override IDocumentStore GetDocumentStore()
            {
                return _documentStore;
            }
        }
    }
}
