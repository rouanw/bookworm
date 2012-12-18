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
        public void AddReturnsAModelWithAnId()
        {
            var efRepo = new EntityFrameworkRepository(DbContext);
            var book = ABook("The book");
            var createdBook = (Book) efRepo.Create(book);
            DbContext.SaveChanges();

            Assert.AreEqual(book.Title, createdBook.Title);
            Assert.AreNotEqual(0, createdBook.Id);
        }

        [TestMethod]
        public void ShouldKnowHowToObtainList()
        {
            AddTwoBooks(DbContext);
            var efRepo = new EntityFrameworkRepository(DbContext);

            var results = efRepo.List<Book>();
            
            Assert.AreEqual(2, results.Count);
            Assert.IsInstanceOfType(results.First(), typeof(Book));
        }

        private static Book ABook(string title = null, string isbn = null, string author = null, string publisher = null,
            string editor = null, string country = null, string coverImageUrl = null, string genre = null,
            string description = null, string language = null, string recommendedAgeGroup = null)
        {
            return new Book
                {
                    Title = title ?? "The Other Book",
                    Isbn = isbn ?? "12345",
                    Author = author ?? "John Smith",
                    Publisher = publisher ?? "Vintage",
                    Editor = editor ?? "John Smith",
                    Country = country ?? "South Africa",
                    CoverImageUrl = coverImageUrl ??
                        "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg",
                    Genre = genre ?? "Literary Fiction",
                    Description = description ?? "A book about an English patient",
                    Language = language ?? "English",
                    RecommendedAgeGroup = recommendedAgeGroup ?? "18"
                };
        }

        private static void AddTwoBooks(PukuDbContext dbContext)
        {
            var dbSet = dbContext.GetDbSet<Book>();

            dbSet.Add(ABook());
            dbSet.Add(ABook(title:"Another book", isbn:"12346"));
            dbContext.SaveChanges();
        }
    }
}