using ASP.NETWEBAPI.DTO;
using ASP.NETWEBAPI.Models;
using ASP.NETWEBAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            Category? CategoryData = _categoryRepository.GetById(id);
            if (CategoryData == null)
            {
                return NotFound();
            }
            CatID_Name_ProductsDTO CategoryDataModel = new CatID_Name_ProductsDTO
            {

                Id = id,
                Name = CategoryData.Name,
                productsName = CategoryData.products.Select(p => p.Name).ToList()
            };

            return Ok(CategoryDataModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories = _categoryRepository.GetAll();
            if (categories == null || categories.Count == 0)
                return NotFound();
            return Ok(categories);
        }

        [HttpGet("{name:regex(^[[a-zA-Z0-9-]]+$)}")]
        public IActionResult GetByName(string name)
        {
            Category category = _categoryRepository.GetByName(name);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Add(CategoryID_NameDTO category)
        {
            Category NewCategory1 = (new Category
            {
                Id = category.Id,
                Name = category.Name
            });
            _categoryRepository.Add(NewCategory1);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(int id, CategoryID_NameDTO category)
        {
            bool ISUpdated = true;
            Category categoryAfterUpdate = (new Category
            {
                Id = category.Id,
                Name = category.Name
            });
            ISUpdated = _categoryRepository.Update(id, categoryAfterUpdate);
            if (ISUpdated)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_categoryRepository.Delete(id))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
