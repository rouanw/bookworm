using System.Data.Entity;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Repository
{
    public class EntityFrameworkIntegrationTest
    {
        protected TestDbContext DbContext;
        // Maps to connection string in App.config
        private const string DatabaseName = "TestDatabase";

        protected class TestDbContext : PukuDbContext
        {
            public TestDbContext() : base (DatabaseName)
            {
                
            }
        }

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestDbContext>());
            DbContext = new TestDbContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Delete(DatabaseName);
        }
    }
}