using Repo.Entities;

namespace Service.Interfaces
{
    public class WatercolorsPaintingCreateResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? PaintingId { get; set; } = null!;

        public string PaintingName { get; set; } = null!;

        public string? PaintingDescription { get; set; }

        public string? PaintingAuthor { get; set; }

        public decimal? Price { get; set; }

        public int? PublishYear { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? StyleId { get; set; }
    }

    public interface IWatercolorsPaintingService
    {
        Task<List<WatercolorsPainting>> GetAll();
        Task<WatercolorsPainting> GetById(string id);
        Task<bool> Delete(string id);
        Task<WatercolorsPaintingCreateResultDto> CreateWithValidation(WatercolorsPainting watercolorsPainting);
        //Task<string> UpdateWithValidation(WatercolorsPainting watercolorsPainting);
    }
}
