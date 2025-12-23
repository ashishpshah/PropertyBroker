using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class City : EntitiesBase
{
	public override long Id { get; set; }

	public string Name { get; set; } = null!;

	public string? State { get; set; }

    public virtual ICollection<AreasMaster> Areas { get; set; } = new List<AreasMaster>();

    public virtual ICollection<Properties> Properties { get; set; } = new List<Properties>();
}
