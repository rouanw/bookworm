using System.Data.Entity;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Moq;

namespace BookWorm.Tests.Repository
{
    [TestClass]
    public class EntityFrameworkRepositoryTest
    {
        [TestMethod]
        public void ShouldKnowToTalkToContextAndDbSetOnCreateRHINO()
        {
            var mocks = new Rhino.Mocks.MockRepository();
            var dbContext = mocks.StrictMock<PukuDbContext>();
            var dbSet = mocks.StrictMock<IDbSet<Book>>();
            var newBook = new Book {Title = "Book"};
            
            With.Mocks(mocks).Expecting(() =>
                {
                    Expect.Call(dbContext.GetDbSet<Book>()).Return(dbSet);
                    Expect.Call(dbSet.Add(newBook));
                })
                .Verify(() =>
                {
                    var efRepo = new EntityFrameworkRepository(dbContext);
                    efRepo.Create(newBook);
                });
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
            dbSet.Verify(set => set.Find(book.Id), Times.Once());
            dbSet.Verify(set => set.Remove(book), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowHowToEditAModelByItsIdWhenItExists()
        {
            var book = new Book {Id = 1, Title = "The Book"};
            var dbContext = new Mock<PukuDbContext>();
            dbContext.Setup(ctx => ctx.SetModified(book));
            var efRepo = new EntityFrameworkRepository(dbContext.Object);

            efRepo.Edit(book);

            dbContext.Verify(ctx => ctx.SetModified(book));
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

        private class TestRepository : EntityFrameworkRepository
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