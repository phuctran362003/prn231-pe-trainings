using Repository.Entities;

namespace Service.Interfaces
{
    public class CreateProductDto
    {
        public int? CategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Material { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public virtual Category? Category { get; set; }
    }

    public interface IProductService
    {
        Task<List<Product>> GetAll();

        Task<Product> GetById(int id);

        Task<Product?> CreateWithValidation(CreateProductDto item);

        Task<bool> Delete(int id);

        //Task<List<Product>> Search(string? param1, string? param2);
    }
}