using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BOs;

public partial class CheetahProfile
{
    [Key]
    public int CheetahProfileId { get; set; }

    public int CheetahTypeId { get; set; }

    [Required]
    [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$", ErrorMessage = "CheetahName is required")]
    public string CheetahName { get; set; } = null!;

    [Required]
    [Range(15, int.MaxValue, ErrorMessage = "Weight must be greater than 15.")]
    public double Weight { get; set; }

    public string Characteristics { get; set; } = null!;

    public string CareNeeds { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual CheetahType CheetahType { get; set; } = null!;
}
