using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository
{
    public class ProductRepo : DataAccessObject<Product>
    {
        public ProductRepo()
            : base(new Spring2025productinventorydbContext())
        {
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var items = await _context.Products.Include(i => i.Category).ToListAsync();
            return items;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var item = await _context.Products.Include(i => i.Category).FirstOrDefaultAsync(t => t.ProductId == id);
            if (item == null)
            {
                _context.Entry(item).State = EntityState.Detached;
            }
            return item;
        }
    }
}