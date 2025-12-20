using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class AreasMaster : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long AreaId { get; set; }

    public string? AreaName { get; set; }
    public string CityName { get; set; }

    public long CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<LocationPropertyMap> LocationPropertyMaps { get; set; } = new List<LocationPropertyMap>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
