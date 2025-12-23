using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class AreasMaster : EntitiesBase
{
	public override long Id { get; set; }

	public long CityId { get; set; }

    public string? Name { get; set; }
    public string CityName { get; set; }

    public virtual City City { get; set; } = null!;

	public virtual ICollection<LocationPropertyMap> LocationPropertyMaps { get; set; } = new List<LocationPropertyMap>();

    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();

}
