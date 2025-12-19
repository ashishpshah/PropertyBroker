using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyType : EntitiesBase
{
	public override long Id { get; set; }

	public long ParentId { get; set; }

    public string? Name { get; set; }
}
