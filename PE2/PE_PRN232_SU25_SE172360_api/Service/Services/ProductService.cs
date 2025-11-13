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

        public ProductService(IValidator<Product> validator)
        {
            _repo = new ProductRepo();
            _validator = validator;
        }

        public async Task<Product?> CreateWithValidation(CreateProductDto dto)
        {
            var Product = MapToDto(dto);

            var validationResult = await _validator.ValidateAsync(Product);
            if (!validationResult.IsValid)
            {
                return null;
            }

            Product.ProductId = await GenerateNextIdAsync();

            var result = await _repo.CreateAsync(Product);
            if (result == 1)
            {
                return Product;
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.GetById(id);
            return await _repo.RemoveAsync(item);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        private async Task<int> GenerateNextIdAsync()
        {
            var allProducts = await _repo.GetAllAsync();
            if (allProducts == null || allProducts.Count == 0)
            {
                return 1;
            }
            return allProducts.Max(h => h.ProductId) + 1;
        }

        private Product MapToDto(CreateProductDto dto)
        {
            return new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                Material = dto.Material,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ReleaseDate = dto.ReleaseDate
            };
        }
    }
}