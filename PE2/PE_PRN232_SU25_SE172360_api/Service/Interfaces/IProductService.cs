using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Service.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> SearchAsync(string? name, int? categoryId);
    Task<List<ProductDto>> GetAll();

    Task<ProductDto> GetById(int id);

    Task<ProductDto> CreateWithValidation(CreateProductDto item);
    Task<ProductDto> UpdateWithValidation(UpdateProductDto item);

    Task<bool> Delete(int id);

    //Task<List<Product>> Search(string? param1, string? param2);
}

public class ProductDto
{
    public int ProductId { get; set; }
    public int? CategoryId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? Material { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public DateOnly? ReleaseDate { get; set; }
}

public class CreateProductDto
{
    [DefaultValue(1)] public int? CategoryId { get; set; }

    [RegularExpression(@"^[A-Z][a-zA-Z0-9\s]{2,50}$", ErrorMessage = "Only letters are allowed.")]
    [DefaultValue("Laptop Pro 15")]
    public string ProductName { get; set; } = null!;

    [DefaultValue("Aluminum")] public string? Material { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only positive integers are allowed.")]
    [DefaultValue(1500)]
    public decimal? Price { get; set; }

    [RegularExpression(@"^\d+$", ErrorMessage = "Only non-negative integers are allowed.")]
    [DefaultValue(10)]
    public int? Quantity { get; set; }

    //public DateOnly? ReleaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}

public class UpdateProductDto
{
    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only positive integers are allowed.")]
    [DefaultValue(1)]
    public int? ProductId { get; set; }

    [DefaultValue(1)] public int? CategoryId { get; set; }

    [RegularExpression(@"^[A-Z][a-zA-Z0-9\s]{2,50}$", ErrorMessage = "Only letters are allowed.")]
    [DefaultValue("Lenovo Xiaoxin 3i")]
    public string ProductName { get; set; } = null!;

    [DefaultValue("Gold")] public string? Material { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only positive integers are allowed.")]
    [DefaultValue(1500)]
    public decimal? Price { get; set; }

    [RegularExpression(@"^\d+$", ErrorMessage = "Only non-negative integers are allowed.")]
    [DefaultValue(10)]
    public int? Quantity { get; set; }

    [DefaultValue("2024-01-01")] public DateOnly? ReleaseDate { get; set; }
}