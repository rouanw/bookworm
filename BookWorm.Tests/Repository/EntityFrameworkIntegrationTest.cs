using System.Transactions;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Repository
{
    public class EntityFrameworkIntegrationTest
    {
        protected TestDbContext DbContext;
        
        // Maps to connection string in App.config
        private const string DatabaseName = "TestDatabase";
        protected TransactionScope TransactionScope;
        public class TestDbContext : PukuDbContext
        {
            public TestDbContext() : base (DatabaseName)
            {
                
            }
        }

        [TestInitialize]
        public void TestSetup()
        {
            DbContext = new TestDbContext();
            DbContext.Database.CreateIfNotExists();
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TransactionScope.Dispose();
        }
    }
}