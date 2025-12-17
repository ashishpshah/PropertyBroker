using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyAmenity : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long AmenityId { get; set; }

    public string AmenityName { get; set; } = null!;

    public virtual ICollection<PropertyAmenityMap> PropertyAmenityMaps { get; set; } = new List<PropertyAmenityMap>();
}
