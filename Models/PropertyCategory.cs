using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyCategory : EntitiesBase
{
	public override long Id { get; set; }	

    public string? Name { get; set; }

   
}
