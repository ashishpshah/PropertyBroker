using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyAmenity : EntitiesBase
{
	public override long Id { get; set; }

	public string Name { get; set; } = null!;

    public int? DisplayOrder { get; set; }
}
