using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class PropertyType : EntitiesBase
{
	public override long Id { get; set; }
	public long ParentId { get; set; }

    public string? Name { get; set; }
    public string? ImagePath { get; set; }
    public int? Display_Seq_No { get; set; } = 0;


    //public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
