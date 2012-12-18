using System.Linq;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Repository
{
    [TestClass]
    public class EntityFrameworkRepositoryIntegrationTests : EntityFrameworkIntegrationTest
    {

        [TestMethod]
        public void ShouldKnowHowToObtainList()
        {
            AddTwoBooks(DbContext);
            var efRepo = new EntityFrameworkRepository(DbContext);

            var results = efRepo.List<Book>();
            
            Assert.AreEqual(2, results.Count);
            Assert.IsInstanceOfType(results.First(), typeof(Book));
        }

        private static void AddTwoBooks(PukuDbContext dbContext)
        {
            var dbSet = dbContext.GetDbSet<Book>();

            dbSet.Add(
                new Book
                    {
                        Title = "The Other Book",
                        Isbn = "12345",
                        Author = "John Smith",
                        Publisher = "Vintage",
                        Editor = "John Smith",
                        Country = "South Africa",
                        CoverImageUrl =
                            "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg",
                        Genre = "Literary Fiction",
                        Description = "A book about an English patient",
                        Language = "English",
                        RecommendedAgeGroup = "18"
                    }
                );
            dbSet.Add(new Book
                {
                    Title = "The Other Other Book",
                    Isbn = "12344",
                    Author = "Jane Doe",
                    Publisher = "Vintage",
                    Editor = "John Smith",
                    Country = "South Africa",
                    CoverImageUrl =
                        "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg",
                    Genre = "Literary Fiction",
                    Description = "A book about an English patient",
                    Language = "English",
                    RecommendedAgeGroup = "18"
                });
            dbContext.SaveChanges();
        }
    }
}