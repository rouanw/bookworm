using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests
{
    [TestClass]
    public static class TestInit
    {
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Database.Delete("TestDatabase");
        }
    }
}
