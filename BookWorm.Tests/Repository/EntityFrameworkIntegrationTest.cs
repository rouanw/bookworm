using System.Transactions;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Repository
{
    public class EntityFrameworkIntegrationTest
    {
        protected PukuDbContext DbContext;

        protected TransactionScope TransactionScope;

        [TestInitialize]
        public void TestSetup()
        {
            DbContext = new PukuDbContext(TestInit.TestDatabaseName);
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