using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class City : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string? State { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
