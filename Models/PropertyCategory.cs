using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyCategory : EntitiesBase
{
	public override long Id { get; set; }	

    public string? Name { get; set; }

   

    public virtual ICollection<Properties> Properties { get; set; } = new List<Properties>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
