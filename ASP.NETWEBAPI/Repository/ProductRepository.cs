using ASP.NETWEBAPI.DbContext;
using ASP.NETWEBAPI.Models;

namespace ASP.NETWEBAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            Product ProductFromDB = GetById(id);
            if (ProductFromDB != null)
            {
                _context.Remove(ProductFromDB);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Product> GetAll()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public Product GetById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public List<Product> GetByName(string name)
        {
            List<Product> products = _context.Products.Where(products => products.Name == name).ToList();
            return products;
        }

        public bool Update(int id, Product product)
        {
            Product ProductFromDb = GetById(id);
            bool IsValid = true;
            if (ProductFromDb == null)
            {
                IsValid = false;
            }
            else if (ProductFromDb.Id != id)
            {

                IsValid = false;
            }

            if (IsValid)
            {
                ProductFromDb.Name = product.Name;
                ProductFromDb.Description = product.Description;
                ProductFromDb.Price = product.Price;
                _context.SaveChanges();
            }
            return IsValid;
        }
    }
}
