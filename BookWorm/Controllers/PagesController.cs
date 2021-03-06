﻿using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.Repository;
using MarkdownSharp;
using Raven.Client.Exceptions;

namespace BookWorm.Controllers
{
    [Authorize]
    public class PagesController : BaseController
    {
        public PagesController()
        {
        }

        public PagesController(IRepository repository) : base(repository)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new StaticPage());
        }

        [HttpPost]
        public ActionResult Create(StaticPage submittedStaticPage)
        {
            try
            {
                Model<StaticPage> savedPage = _repository.Create(submittedStaticPage);
                TempData["flash"] = string.Format("Added {0}", submittedStaticPage.Title);
                return RedirectToAction("Details", "Pages", new {id = savedPage.Id});
            }
            catch (NonUniqueObjectException ex)
            {
                TempData["flash"] = string.Format("Sorry, page {0} already exists.", submittedStaticPage.Title);
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var page = _repository.Get<StaticPage>(id);
            ViewBag.transformedContent = new Markdown().Transform(page.Content);
            return View(page);
        }


        [HttpGet]
        [AllowAnonymous]
        public ViewResult List()
        {
            return View(_repository.List<StaticPage>());
        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<StaticPage>(id);
            return RedirectToAction("List", "Pages");
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_repository.Get<StaticPage>(id));
        }

        [HttpPut]
        public RedirectToRouteResult Edit(StaticPage updatedPage)
        {
            _repository.Edit(updatedPage);
            return RedirectToAction("Details", new {id = updatedPage.Id});
        }
    }
}