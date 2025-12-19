using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyAmenityMap : EntitiesBase
{
	public override long Id { get; set; }

	public long PropertyId { get; set; }

    public long AmenityId { get; set; }

}
