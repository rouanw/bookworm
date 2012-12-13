using BookWorm.Models;
using BookWorm.Models.Validations;

namespace BookWorm.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookWorm.Repository.PukuDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookWorm.Repository.PukuDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Books.AddOrUpdate(b => b.Title,
                                      new Book { Title = "The English Patient", Isbn = "12345", Author = "Michael Ondaantje", Publisher = "Vintage", Editor = "John Smith", Country = "South Africa", CoverImageUrl = "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg", Genre = "Literary Fiction", Description = "A book about an English patient", Language = "English", RecommendedAgeGroup = "18"},
                                      new Book { Title = "The Trial", Isbn = "12344", Author = "Michael Ondaantje", Publisher = "Vintage", Editor = "John Smith", Country = "South Africa", CoverImageUrl = "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg", Genre = "Literary Fiction", Description = "A book about an English patient", Language = "English", RecommendedAgeGroup = "18" },
                                      new Book { Title = "Notes From Underground", Isbn = "12343", Author = "Michael Ondaantje", Publisher = "Vintage", Editor = "John Smith", Country = "South Africa", CoverImageUrl = "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg", Genre = "Literary Fiction", Description = "A book about an English patient", Language = "English", RecommendedAgeGroup = "18" },
                                      new Book { Title = "The Sun Also Rises", Isbn = "12342", Author = "Michael Ondaantje", Publisher = "Vintage", Editor = "John Smith", Country = "South Africa", CoverImageUrl = "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg", Genre = "Literary Fiction", Description = "A book about an English patient", Language = "English", RecommendedAgeGroup = "18" },
                                      new Book {Title = "The Long Dark Tea Time of the Soul", Isbn = "12341", Author = "Michael Ondaantje", Publisher = "Vintage", Editor = "John Smith", Country = "South Africa", CoverImageUrl = "http://upload.wikimedia.org/wikipedia/en/thumb/f/f1/Englishpatient.jpg/200px-Englishpatient.jpg", Genre = "Literary Fiction", Description = "A book about an English patient", Language = "English", RecommendedAgeGroup = "18"});

            context.Pages.AddOrUpdate(p => p.Title,
                new StaticPage{Title = "Page 1", Content = "This is Content 1"},
                new StaticPage{Title = "Page 2", Content = "This is Content 2"},
                new StaticPage{Title = "Page 3", Content = "This is Content 3"},
                new StaticPage{Title = "Page 4", Content = "This is Content 4"},
                new StaticPage{Title = "Page 5", Content = "This is Content 5"},
                new StaticPage{Title = "Page 6", Content = "This is Content 6"},
                new StaticPage{Title = "Page 7", Content = "This is Content 7"});
        }
    }
}
