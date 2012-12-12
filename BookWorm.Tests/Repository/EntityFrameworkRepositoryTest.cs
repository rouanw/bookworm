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
    }
}
