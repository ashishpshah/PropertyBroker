using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Location : EntitiesBase
{
	public override long Id { get; set; }

	public long LocationId { get; set; }

    public long AreaId { get; set; }

    public string LocationName { get; set; } = null!;

    public string? Pincode { get; set; }

    public virtual AreasMaster Area { get; set; } = null!;
}
