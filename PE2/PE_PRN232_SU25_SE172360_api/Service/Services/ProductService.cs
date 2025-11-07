using FluentValidation;
using Repository;
using Repository.Entities;
using Service.Interfaces;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepo _repo;
        private readonly IValidator<Product> _validator;

        //public Task<int> Create(Product Product)
        //{
        //    Product.ReleaseDate = DateTime.UtcNow;
        //    return _repo.CreateAsync(Product);
        //}

        //public async Task<string> CreateWithValidation(Product Product)
        //{
        //    // Kiểm tra dữ liệu với FluentValidation
        //    var validationResult = await _validator.ValidateAsync(Product);
        //    if (!validationResult.IsValid)
        //    {
        //        return string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
        //    }

        //    Product.ProductId = GenerateId();
        //    var result = await _repo.CreateAsync(Product);
        //    if (result == 1)
        //    {
        //        return "Thêm Thành công";
        //    }
        //    return "Thêm thất bại";
        //}

        public string GenerateId()
        {
            return "WP" + DateTime.UtcNow.ToString("yyyyMMddHHmmss").Substring(0, 3) + Guid.NewGuid().ToString("N").Substring(0, 3);
        }

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

        public async Task<List<Product>> Search(string? name, int? categoryId)
        {
            return await _repo.SearchAsync(name, categoryId);
        }
    }
}