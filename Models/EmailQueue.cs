using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class EmailQueue : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long EmailId { get; set; }

    public string? ToEmail { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? Status { get; set; }

}
