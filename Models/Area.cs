using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Area : EntitiesBase
{
	public override long Id { get; set; }

	public long CityId { get; set; }

	public string Name { get; set; } = null!;

	public virtual City City { get; set; } = null!;

	public virtual ICollection<LocationPropertyMap> LocationPropertyMaps { get; set; } = new List<LocationPropertyMap>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

}
