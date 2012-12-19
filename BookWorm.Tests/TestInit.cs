using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests
{
    [TestClass]
    public static class TestInit
    {
        // Maps to connection string in App.config
        public const string TestDatabaseName = "TestDatabase";

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Database.Delete(TestDatabaseName);
        }
    }
}
