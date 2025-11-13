using Repository;
using Repository.Entities;
using Service.Interfaces;

namespace Service.Services;

public class ProductService : IProductService
{
    private readonly ProductRepo _repo;

    public ProductService()
    {
        _repo = new ProductRepo();
    }

    public async Task<List<ProductDto>> SearchAsync(string? name, int? categoryId)
    {
        var products = await _repo.SearchAsync(name, categoryId);
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto> CreateWithValidation(CreateProductDto dto)
    {
        var product = new Product
        {
            ProductId = await GenerateNextIdAsync(),
            ProductName = dto.ProductName,
            CategoryId = dto.CategoryId,
            Material = dto.Material,
            Price = dto.Price,
            Quantity = dto.Quantity,
            ReleaseDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var result = await _repo.CreateAsync(product);

        if (result == 1) return await GetById(product.ProductId);

        throw new Exception("Failed to create product.");
    }

    public async Task<ProductDto> UpdateWithValidation(UpdateProductDto dto)
    {
        var item = _repo.GetById(dto.ProductId);
        if (item == null) throw new Exception($"Product with ID {dto.ProductId} not found.");

        // Update trực tiếp vào item EF đang track
        item.ProductName = dto.ProductName;
        item.CategoryId = dto.CategoryId;
        item.Material = dto.Material;
        item.Price = dto.Price;
        item.Quantity = dto.Quantity;
        item.ReleaseDate = dto.ReleaseDate;

        var result = await _repo.UpdateAsync(item);

        if (result == 1) return await GetById(item.ProductId);

        throw new Exception("Update failed due to unknown reasons.");
    }

    public async Task<bool> Delete(int id)
    {
        var item = _repo.GetById(id);
        return await _repo.RemoveAsync(item);
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _repo.GetAllAsync();
        return products
            .OrderByDescending(p => p.ReleaseDate)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<ProductDto> GetById(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return MapToDto(product);
    }

    private async Task<int> GenerateNextIdAsync()
    {
        var allProducts = await _repo.GetAllAsync();
        if (allProducts == null || allProducts.Count == 0) return 1;
        return allProducts.Max(h => h.ProductId) + 1;
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            Material = product.Material,
            Price = product.Price,
            Quantity = product.Quantity,
            ReleaseDate = product.ReleaseDate
        };
    }
}