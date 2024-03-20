using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;
        private readonly IValidator<ArticleCreateDTO> _validatorArticleCreate;
        private readonly IValidator<ArticleUpdateDTO> _validatorArticleUpdate;

        public ArticleController(IArticleBLL articleBLL,
            IValidator<ArticleCreateDTO> validatorArticleCreate,
            IValidator<ArticleUpdateDTO> validatorArticleUpdate
            )
        {
            _articleBLL = articleBLL ?? throw new ArgumentNullException(nameof(articleBLL));
            _validatorArticleCreate = validatorArticleCreate;
            _validatorArticleUpdate = validatorArticleUpdate;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> Get()
        {
            var articles = await _articleBLL.GetArticleWithCategory();
            return Ok(articles);
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> Get(int id)
        {
            var article = await _articleBLL.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            return article;
        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<ActionResult<ArticleDTO>> Post([FromBody] ArticleCreateDTO article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            try
            {
                var validatorResult = await _validatorArticleCreate.ValidateAsync(article);
                if (!validatorResult.IsValid)
                {
                    Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                    return BadRequest(ModelState);
                }
                var insertedArticle = await _articleBLL.Insert(article);
                return CreatedAtAction(nameof(Get), new { id = insertedArticle.ArticleID }, insertedArticle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleUpdateDTO article)
        {
            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            try
            {
                var validatorResult = await _validatorArticleUpdate.ValidateAsync(article);
                if (!validatorResult.IsValid)
                {
                    Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                    return BadRequest(ModelState);
                }
                var updatedArticle = await _articleBLL.Update(article);
                if (updatedArticle == null)
                {
                    return NotFound();
                }

                return Ok("Update data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleBLL.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET api/<ArticleController>/paged
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetWithPaging(int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var articles = await _articleBLL.GetWithPaging(categoryId, pageNumber, pageSize);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
