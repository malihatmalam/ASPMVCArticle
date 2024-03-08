using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleBLL _articleBLL;

        public ArticleController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 5, string search = "", string act = "" )
        {
            if (TempData["message"] != null) 
            {
                ViewData["message"] = TempData["message"];
            }

            ViewData["search"] = search;
            var models = _articleBLL.GetWithPaging(pageNumber, pageSize, search);
            var maxsize = _articleBLL.GetCountArticles(search);

            if (act == "next")
            {
                if (pageNumber * pageSize < maxsize)
                {
                    pageNumber += 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }
            else if (act == "prev")
            {
                if (pageNumber > 1)
                {
                    pageNumber -= 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }
            else
            {
                ViewData["pageNumber"] = 2;
            }

            ViewData["pageSize"] = pageSize;

            return View(models);
        }

        public IActionResult Detail(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreate)
        {
            try
            {
                _articleBLL.Insert(articleCreate);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Article Success !</div>";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, ArticleUpdateDTO articleEdit)
        {
            try
            {
                _articleBLL.Update(articleEdit);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Article Success !</div>";
            }
            catch (Exception ex)
            {
                ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(articleEdit);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id, CategoryDTO category)
        {
            try
            {
                _articleBLL.Delete(id);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Article Success !</div>";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(category);
            }
            return RedirectToAction("Index");
        }
    }
}
