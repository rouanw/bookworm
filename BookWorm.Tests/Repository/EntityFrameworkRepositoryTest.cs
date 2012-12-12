using System.Data.Entity;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Repository
{
    [TestClass]
    public class EntityFrameworkRepositoryTest
    {
        //TODO add integration test to confirm that dbset.add returns a model with an id

        [TestMethod]
        public void ShouldKnowToTalkToContextAndDbSetOnCreate()
        {
            var dbContext = new Mock<PukuDbContext>();
            var dbSet = new Mock<IDbSet<Book>>();
            dbContext.Setup(context => context.GetDbSet<Book>()).Returns(dbSet.Object);
            var efRepo = new EntityFrameworkRepository(dbContext.Object);

            var newBook = new Book {Title = "Book"};
            efRepo.Create(newBook);

            dbContext.Verify(context => context.GetDbSet<Book>(), Times.Once());
            dbSet.Verify(set => set.Add(newBook), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowToTalkToContextToSaveChanges()
        {
            var dbContext = new Mock<PukuDbContext>();
            dbContext.Setup(context => context.SaveChanges());
            var efRepo = new EntityFrameworkRepository(dbContext.Object);

            efRepo.SaveChanges();

            dbContext.Verify(context => context.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowHowToGetModelFromTheirId()
        {
            var book = new Book {Id = 1, Title = "The Book"};
            var dbContext = new Mock<PukuDbContext>();
            var dbSet = new Mock<IDbSet<Book>>();
            dbSet.Setup(set => set.Find(1)).Returns(book);
            dbContext.Setup(context => context.GetDbSet<Book>()).Returns(dbSet.Object);
            var efRepo = new EntityFrameworkRepository(dbContext.Object);

            var foundBook = efRepo.Get<Book>(1);
            Assert.AreEqual(book, foundBook);
            dbContext.Verify(context => context.GetDbSet<Book>(), Times.Once());
            dbSet.Verify(set => set.Find(1), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowHowToDeleteAModelByItsIdWhenItExists()
        {
            var book = new Book {Id = 1, Title = "The Book"};
            var dbContext = new Mock<PukuDbContext>();
            var dbSet = new Mock<IDbSet<Book>>();
            dbSet.Setup(set => set.Find(book.Id)).Returns(book);
            dbSet.Setup(set => set.Remove(book));
            dbContext.Setup(context => context.GetDbSet<Book>()).Returns(dbSet.Object);
            var efRepo = new EntityFrameworkRepository(dbContext.Object);

            efRepo.Delete<Book>(book.Id);

            dbContext.Verify(context => context.GetDbSet<Book>(), Times.Once());
            dbSet.Verify(set=>set.Find(book.Id), Times.Once());
            dbSet.Verify(set=>set.Remove(book), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowToInstantiateContext()
        {
            var dbSet = new Mock<IDbSet<Book>>();
            var dbContext = new Mock<PukuDbContext>();
            dbContext.Setup(ctx => ctx.GetDbSet<Book>()).Returns(dbSet.Object);

            var efRepo = new TestRepository();

            Assert.IsNull(efRepo.GetContextInstance());
            efRepo.DoStuffThatUsesContextPropery();
            Assert.IsNotNull(efRepo.GetContextInstance());
        }

        public class TestRepository : EntityFrameworkRepository
        {
            public DbContext GetContextInstance()
            {
                return _pukuDbContext;
            }

            public void DoStuffThatUsesContextPropery()
            {
                var stuff = PukuDbContext;
            }
        }
    }
}
