using System;
using System.Data.Entity;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Repository
{
    [TestClass]
    public class PukuDbContextTest
    {
        [TestMethod]
        public void CanFindDbSetOfSpecifiedType()
        {
            var dbContext = new PukuDbContext();
            var dbSet = dbContext.GetDbSet<Book>();
            Assert.IsInstanceOfType(dbSet, typeof(IDbSet<Book>));
        }

        [ExpectedException(typeof(ArgumentException), "No DbSet exists for type PukuDbContextTest")]
        [TestMethod]
        public void ThrowsExceptionWhenTryToGetDbSetOfInvalidType()
        {
            var dbContext = new PukuDbContext();
            var dbSet = dbContext.GetDbSet<WrongClass>();
        }

        private class WrongClass : Model<WrongClass>
        {
            
        }
    }
}
