﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void ShouldReturnCreatePageOnGetCreate()
        {
            var booksController = new BooksController();

            var createABookView = booksController.Create();
            var model = createABookView.Model;

            Assert.AreEqual("Add a Book", booksController.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(Book));
        }

        [TestMethod]
        public void ShouldRedirectToDetailsPageWhenBookIsCreated()
        {
            var book = new Book { Title = "The Book" };

            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book { Id = 1, Title = "The Book"});
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Create(book);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book successfully", booksController.TempData["flash"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldUseRepositoryWhenCreatingABook()
        {
            var book = new Book();
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book());

            var booksController = new BooksController(mockedRepo.Object);

            booksController.Create(book);
            mockedRepo.Verify(repo => repo.Create(book));
        }

        [TestMethod]
        public void CreateBookShouldNotSaveWhenBookIsInvalid()
        {
            var book = new Book();
            var mockedRepo = new Mock<IRepository>();
            var booksController = new BooksController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Create(book)).Returns(book);
            booksController.ModelState.AddModelError("test error","test exception");

            var result = (ViewResult)booksController.Create(book);

            mockedRepo.Verify(repo => repo.Create(book), Times.Never(), "failing model validation should prevent creating book");
            Assert.AreEqual("There were problems saving this book", booksController.TempData["flash"]);
        }

        [TestMethod]
        public void ShouldDisplayBookDetails()
        {
            var book = new Book {Id = 1, Title = "A book"};
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.Details(1);

            var bookInView = (Book) view.Model;
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            mockedRepo.Verify(repo => repo.Get<Book>(book.Id), Times.Once());
            Assert.AreEqual(book, bookInView);
        }

        [TestMethod]
        public void ShouldListAllBooks()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 10).ToList().ForEach(i=>books.Add(new Book{Id = i}));
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.List<Book>()).Returns(books);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.List();

            var booksInView = (List<Book>) view.Model;
            mockedRepo.Verify(repo => repo.List<Book>(), Times.Once());
            Assert.AreEqual(books, booksInView);
        }

        [TestMethod]
        public void ShouldReturnEditPageOnGetEdit()
        {
            var book = new Book { Id = 1, Title = "A book" };
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var editABookView = booksController.Edit(1);
            var model = (Book)editABookView.Model;
           
            Assert.AreEqual("Edit a Book", booksController.ViewBag.Title);
            Assert.AreEqual(book.Title, model.Title);
        }

        [TestMethod]
        public void ShouldReturnDetailsPageOnGetDetails()
        {
            var book = new Book { Id = 1, Title = "A book" };
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var detailsOfABookView = booksController.Details(1);
            var model = (Book) detailsOfABookView.Model;

            Assert.AreEqual("A book", booksController.ViewBag.Title);
            Assert.AreEqual(book.Title, model.Title);
        }

        [TestMethod]
        public void ShouldUpdateBookOnEditPost()
        {
            var editedBook = new Book { Id = 1, Title = "A book" };
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Edit(editedBook));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Edit(editedBook);

            mockedRepo.Verify(repo => repo.Edit<Book>(editedBook), Times.Once());
            Assert.AreEqual("Updated A book successfully", booksController.TempData["flash"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
            
        }

        [TestMethod]
        public void EditBookShouldNotSaveWhenBookIsInvalid()
        {
            var book = new Book();
            var mockedRepo = new Mock<IRepository>();
            var booksController = new BooksController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Edit(book));
            booksController.ModelState.AddModelError("test error", "test exception");

            var result = (ViewResult)booksController.Edit(book);

            mockedRepo.Verify(repo => repo.Edit(book), Times.Never(), "failing model validation should prevent updating book");
            Assert.AreEqual("There were problems saving this book", booksController.TempData["flash"]);
        }

        [TestMethod]
        public void ShouldDeleteBookAndShowListOfBooks()
        {
            var mockedRepo = new Mock<IRepository>();
            mockedRepo.Setup(repo => repo.Delete<Book>(1));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = booksController.Delete(1);
            mockedRepo.Verify(repo => repo.Delete<Book>(1));
            Assert.AreEqual("Book successfully deleted", booksController.TempData["flash"]);
            Assert.AreEqual("List", viewResult.RouteValues["action"]);
        }

        [TestMethod]
        public void BooksControllerShouldRequireAuthorization()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetCustomAttributes(typeof(AuthorizeAttribute), false).Count());
        }
        [TestMethod]
        public void BooksControllerListShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "List")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerDetailsShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "Details")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }
    }
}
