using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BO;

namespace SampleMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleBLL _articleBLL;
        private CategoryBLL _categoryBLL;

        public ArticleController(IArticleBLL articleBLL, ICategoryBLL categoryBLL)
        {
            _articleBLL = articleBLL;
            _categoryBLL = new CategoryBLL();
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 5, string search = "", string act = "", string category = "")
        {
            ViewBag.CategoryName = category;
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;

            if (TempData["message"] != null)
            {
                ViewData["message"] = TempData["message"];
            }

            ViewData["search"] = search;
            ViewData["categoryFilter"] = category;
            var models = _articleBLL.GetWithPaging(pageNumber, pageSize, search, category);
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
            var categoryOptions = _categoryBLL.GetAll();
            ViewBag.Categories = categoryOptions;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreate, IFormFile ImageArticle)
        {
            try
            {

                String fileName = "";
                if (ImageArticle != null)
                {
                    if (Helper.IsImageFile(ImageArticle.FileName))
                    {
                        //random file name based on GUID
                        fileName = $"{Guid.NewGuid()}_{ImageArticle.FileName}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageArticle.CopyTo(fileStream);
                        }
                    }
                }

                ArticleCreateDTO articleCreateDTO = new ArticleCreateDTO();
                articleCreateDTO.Title = articleCreate.Title;
                articleCreateDTO.Details = articleCreate.Details;
                articleCreateDTO.IsApproved = articleCreate.IsApproved;
                articleCreateDTO.CategoryID = articleCreate.CategoryID;
                
                if (ImageArticle != null) {
                    articleCreateDTO.Pic = fileName;
                }
                _articleBLL.Insert(articleCreateDTO);
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
            var categoryOptions = _categoryBLL.GetAll();
            ViewBag.Categories = categoryOptions;

            var model = _articleBLL.GetArticleById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, ArticleUpdateDTO articleEdit, IFormFile ImageArticle)
        {
            try
            {
                String fileName = "";
                if (ImageArticle != null)
                {
                    if (Helper.IsImageFile(ImageArticle.FileName))
                    {
                        //random file name based on GUID
                        fileName = $"{Guid.NewGuid()}_{ImageArticle.FileName}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageArticle.CopyTo(fileStream);
                        }
                    }
                }

                ArticleUpdateDTO articleUpdateDTO = new ArticleUpdateDTO();
                articleUpdateDTO.ArticleID = Convert.ToInt32(articleEdit.ArticleID);
                articleUpdateDTO.CategoryID = articleEdit.CategoryID;
                articleUpdateDTO.Title = articleEdit.Title;
                articleUpdateDTO.Details = articleEdit.Details;
                articleUpdateDTO.IsApproved = articleEdit.IsApproved;

                if (ImageArticle != null)
                {
                    articleUpdateDTO.Pic = fileName;
                }
                else {
                    articleUpdateDTO.Pic = articleEdit.Pic;
                }
                _articleBLL.Update(articleUpdateDTO);

                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Article Success !</div>";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                var categoryOptions = _categoryBLL.GetAll();
                ViewBag.Categories = categoryOptions;

                return View(articleEdit);
            }
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
