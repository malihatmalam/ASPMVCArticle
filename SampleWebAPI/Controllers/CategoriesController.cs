using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BO;

namespace SampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryBLL _categoryBLL;
        public CategoriesController(ICategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var categories = _categoryBLL.GetAll();
                return Ok(categories);
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
                var category = _categoryBLL.GetById(id);

                //var category = categories.SingleOrDefault(c => c.CategoryId == id);
                if (category == null)
                {
                    return NotFound($"CategoryID {id} not found!");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Post(CategoryCreateDTO category)
        {
            try
            {
                _categoryBLL.Insert(category);
                return Ok($"Category berhasil di tambahkan!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, CategoryUpdateDTO category)
        {
            try
            {
                var result = _categoryBLL.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                _categoryBLL.Update(category);
                return Ok($"Data Category berhasil diupdate !");
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
                var result = _categoryBLL.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                _categoryBLL.Delete(id);
                return Ok($"Data Category berhasil didelete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByName")]
        public IEnumerable<CategoryDTO> GetByName(string name)
        {
            var results = _categoryBLL.GetByName(name);
            return results;
        }
    }
}
