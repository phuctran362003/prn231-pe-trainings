using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BOs;

public partial class CheetahType
{
    [Key]
    public int CheetahTypeId { get; set; }

    public string? CheetahTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<CheetahProfile> CheetahProfiles { get; set; } = new List<CheetahProfile>();
}
