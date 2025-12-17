using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyAmenityMap : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long PropertyId { get; set; }

    public long AmenityId { get; set; }


    public virtual PropertyAmenity Amenity { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
