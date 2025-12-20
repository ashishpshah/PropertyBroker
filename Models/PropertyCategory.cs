using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyCategory : EntitiesBase
{
	public override long Id { get; set; }	

    public string? Name { get; set; }

   

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
