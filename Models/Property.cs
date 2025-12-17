using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Property : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long PropertyId { get; set; }

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

    public virtual Area? Area { get; set; }

    public virtual PropertyCategory? Category { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<LocationPropertyMap> LocationPropertyMaps { get; set; } = new List<LocationPropertyMap>();

    public virtual ICollection<PropertyAmenityMap> PropertyAmenityMaps { get; set; } = new List<PropertyAmenityMap>();

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

    public virtual PropertyType? Type { get; set; }
}
