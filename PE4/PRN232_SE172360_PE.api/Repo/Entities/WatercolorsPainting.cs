using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repo.Entities;

public partial class WatercolorsPainting
{
    [Key]
    [JsonIgnore]
    public string? PaintingId { get; set; } = null!;

    public string PaintingName { get; set; } = null!;

    public string? PaintingDescription { get; set; }

    public string? PaintingAuthor { get; set; }

    public decimal? Price { get; set; }

    public int? PublishYear { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? StyleId { get; set; }

    public virtual Style? Style { get; set; }
}
