using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyType : EntitiesBase
{
	public override long Id { get; set; }
	public long ParentId { get; set; }

    public string? Name { get; set; }


    //public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
