using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LocationPropertyMap : EntitiesBase
{
	public override long Id { get; set; }

	public long? PropertyId { get; set; }

    public long? AreaId { get; set; }

    public virtual AreasMaster? Area { get; set; }

    public virtual Property? Property { get; set; }
}
