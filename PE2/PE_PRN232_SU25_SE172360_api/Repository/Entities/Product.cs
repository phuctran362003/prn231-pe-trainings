using System.Text.Json.Serialization;

namespace Repository.Entities;

public partial class Product
{
    [JsonIgnore]
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Material { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public virtual Category? Category { get; set; }
}