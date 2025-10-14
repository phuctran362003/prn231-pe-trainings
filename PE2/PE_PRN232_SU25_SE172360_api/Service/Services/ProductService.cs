using Repository;
using Repository.Entities;
using Service.Interfaces;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepo _repo;

        public ProductService()
        {
            _repo = new ProductRepo();
        }

        public Task<int> Create(Product item)
        {
            return _repo.CreateAsync(item);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.GetById(id);
            return await _repo.RemoveAsync(item);
        }
    }
}