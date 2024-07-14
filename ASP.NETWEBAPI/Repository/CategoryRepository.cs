using ASP.NETWEBAPI.DbContext;
using ASP.NETWEBAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETWEBAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        public CategoryRepository(AppDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public void Add(Category category)
        {
           _context.Categories.Add(category);
           _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            Category? category = _context.Categories.Include(c => c.products).FirstOrDefault(c => c.Id == id);

            return category;
        }

        public Category GetByName(string name)
        {
            Category? category = _context.Categories.FirstOrDefault(c => c.Name == name);
            return category;
        }

        public bool Update(int id, Category category)
        {
            Category categoryFromDB = GetById(id);
            if (categoryFromDB == null)
            {
                return false;

            }
            categoryFromDB.Name = category.Name;
            categoryFromDB.Id = id;
            _context.SaveChanges();
            return true;
        }
    }
}
