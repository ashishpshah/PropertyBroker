using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LeadTimeline : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long TimelineId { get; set; }

    public long? LeadId { get; set; }

    public string? Action { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public virtual Lead? Lead { get; set; }
}
