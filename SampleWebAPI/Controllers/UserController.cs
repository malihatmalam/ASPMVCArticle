using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        public UserController(IUserBLL userBLL)
        {
            _articleBLL = articleBLL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var articles = _articleBLL.GetAll();
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var article = _articleBLL.GetArticleById(id);

                //var category = categories.SingleOrDefault(c => c.CategoryId == id);
                if (article == null)
                {
                    return NotFound($"ArticleID {id} not found!");
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Post(ArticleCreateDTO article)
        {
            try
            {
                _articleBLL.Insert(article);
                return Ok($"Article berhasil di tambahkan!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ArticleUpdateDTO article)
        {
            try
            {
                var result = _articleBLL.GetArticleById(id);
                if (result == null)
                {
                    return NotFound();
                }
                _articleBLL.Update(article);
                return Ok($"Data article berhasil diupdate !");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _articleBLL.GetArticleById(id);
                if (result == null)
                {
                    return NotFound();
                }
                _articleBLL.Delete(id);
                return Ok($"Data Article berhasil didelete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Category/{category_id}")]
        public IActionResult GetByCategoryId(int category_id)
        {
            try
            {
                var article = _articleBLL.GetArticleByCategory(category_id);

                //var category = categories.SingleOrDefault(c => c.CategoryId == id);
                if (article == null)
                {
                    return NotFound($"Article by category {category_id} not found!");
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
