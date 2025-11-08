using Repository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Service.Interfaces
{
    public class CreateLeopardProfileDto
    {
        public int LeopardTypeId { get; set; }
        [RegularExpression(@"^ ([A-z0-9] [a-zA-z0-9#]*\s) *([A-z0-9] [a-zA-Z0-9#]*)$", ErrorMessage = "Only letters are allowed.")]
        public string LeopardName { get; set; } = null!;
        [Range(15, int.MaxValue, ErrorMessage = "Value must be greater than 15.")]

        public double Weight { get; set; }

        public string Characteristics { get; set; } = null!;

        public string CareNeeds { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

        public virtual LeopardType LeopardType { get; set; } = null!;
    }
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAll();
        Task<LeopardProfile> GetById(int id);
        Task<LeopardProfile?> CreateWithValidation(CreateLeopardProfileDto item);
        Task<bool> Delete(int id);
        Task<List<LeopardProfile>> Search(string? param1, string? param2);
    }
}
