using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyCategory : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }
	public long CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
