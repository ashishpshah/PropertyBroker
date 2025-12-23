using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Properties : EntitiesBase
{
	public override long Id { get; set; }

	public string Title { get; set; } = null!;

	public string? Description { get; set; }

	public long? TypeId { get; set; }

	public long? CategoryId { get; set; }

	public decimal? Price { get; set; }

	public decimal? AreaSqft { get; set; }

	public string? OwnerName { get; set; }

	public string? OwnerMobile { get; set; }

	public string? BuilderName { get; set; }

	public string? Status { get; set; }

	public long? CityId { get; set; }

	public long? AreaId { get; set; }

	public string? Landmark { get; set; }

	public string? Remark { get; set; }
	[NotMapped] public string? AvailabilityStatus { get; set; }
	[NotMapped] public string? City_Name { get; set; }
	[NotMapped] public string? Area_Name { get; set; }
	[NotMapped] public string? Property_Type { get; set; }
	[NotMapped] public string? Property_Category { get; set; }
	[NotMapped] public int? FloorNo { get; set; }
	[NotMapped] public int? TotalFloors { get; set; }
	[NotMapped] public string? Facing { get; set; }
	[NotMapped] public string? FurnishingStatus { get; set; }
    [NotMapped] public string? FurnishingStatus_TEXT { get; set; }
    [NotMapped] public string? AvailabilityStatus_TEXT { get; set; }
	[NotMapped] public bool? IsFeatured { get; set; }
}
