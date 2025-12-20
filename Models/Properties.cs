using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Properties : EntitiesBase
{
	public override long Id { get; set; }

	public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public long? CityId { get; set; }

    public long? AreaId { get; set; }
    public string? Landmark { get; set; }
    public long? CategoryId { get; set; }
    public long? TypeId { get; set; }   

    public decimal? Price { get; set; }

    public decimal? AreaSqft { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerMobile { get; set; }

    public string? BuilderName { get; set; }
    public string? Property_Type { get; set; }
    public string? Property_Category { get; set; }
    public int? FloorNo { get; set; }   
    public int? TotalFloors { get; set; }   
    public string? Facing { get; set; }   
    public string? FurnishingStatus { get; set; }
    public string? FurnishingStatus_TEXT { get; set; }
    public string? AvailabilityStatus { get; set; }
    public string? AvailabilityStatus_TEXT { get; set; }
    public bool? IsFeatured { get; set; }
    public string? Remark { get; set; }
}
