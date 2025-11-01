using Repository.Entities;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();

        Task<Product> GetById(int id);

        Task<int> Create(Product watercolorsPainting);

        Task<bool> Delete(int id);

        Task<List<Product>> Search(string? name, int? categoryId);
    }
}