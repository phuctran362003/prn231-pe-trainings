using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repository.Entities;

public partial class LeopardType
{
    [Key]
    public int LeopardTypeId { get; set; }

    public string? LeopardTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<LeopardProfile> LeopardProfiles { get; set; } = new List<LeopardProfile>();
}
